using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TadaLib.Save;

/// <summary>
/// スコアデータを管理するクラス
/// 
/// 以下使いかた
/// 1. RegisterScore(score, game_scene_name) でスコアを登録
/// 2. Score score = GetScoreData(game_scene_name) でそのゲームのスコアデータを取得
/// 3. score.Scoresでランキング順に並んだスコアのリストを取得できる
/// 
/// </summary>

// 1からn位までのスコアデータ これを作ったのは2次元配列をJsonUtilityで保存できないから
[System.Serializable]
public class Score
{
    #region field
    [SerializeField]
    private List<int> scores_;
    public List<int> Scores { private set { scores_ = value; } get { return scores_; } }

    private int rank_length_;
    #endregion

    public Score()
    {
        scores_ = new List<int>();
    }

    // スコアを追加する ランクインしているなら順位(1から数える)を返す していないなら -1 を返す
    public int AddScore(int score)
    {
        // ランクインしているかどうか
        if(scores_[rank_length_ - 1] > score)
        {
            return -1;
        }

        int rank = 0;
        // 2分探索使いたいけどデータ少ないので愚直に
        for(int i = 0, n = scores_.Count; i < n; ++i)
        {
            if(scores_[i] <= score)
            {
                // 順位更新
                rank = i;
                break;
            }
        }

        Debug.Log("rank : " + rank);

        for(int i = scores_.Count - 1; i > rank; --i)
        {
            scores_[i] = scores_[i - 1];
        }
        scores_[rank] = score;

        return rank + 1;
    }

    // 何位まで保存するかを変更する
    public void ChangeDataLength(int length)
    {
        rank_length_ = length;

        if (scores_.Count > length)
        {
            //scores_.RemoveRange(length, length - scores_.Count);
            for(int i = scores_.Count - 1; i >= length; --i)
            {
                scores_.RemoveAt(i);
            }
        }
        else if (scores_.Count < length)
        {
            // 0点のデータを追加する
            for (int i = 0, n = length - scores_.Count; i < n; ++i)
            {
                scores_.Add(0);
            }
        }
    }
}

// 全てのゲームのスコアデータを管理するクラス
[System.Serializable]
public class ScoreData : BaseSaver<ScoreData>
{
    private const string kFileName = "Score";

    #region field
    [SerializeField]
    private List<Score> scores_;
    public List<Score> Scores { private set { scores_ = value; } get { return scores_; } }

    private int rank_length_;
    #endregion

    public ScoreData()
    {
        scores_ = new List<Score>();
    }

    // 始めに必ず呼び出すこと
    public void Init(int game_num, int display_rank_length)
    {
        ScoreData data = Load(kFileName);
        if (data == null)
        {
            scores_ = new List<Score>();
            Debug.Log("データ読み込み失敗");
        }
        else scores_ = data.Scores;

        rank_length_ = display_rank_length;

        // 新しいゲームが増えている場合はその分増やす
        if(scores_.Count < game_num)
        {
            for(int i = 0, n = game_num - scores_.Count; i < n; ++i)
            {
                scores_.Add(new Score());
            }
        }

        // 逆に減っている場合はスコアをすべてリセット (整合性が合わなくなる)
        if(scores_.Count > game_num)
        {
            scores_.Clear();
            for(int i = 0; i < game_num; ++i)
            {
                scores_.Add(new Score());
            }
        }

        // 何位まで登録するかを変更する (同じの場合はそのまま)
        foreach(Score score in scores_)
        {
            score.ChangeDataLength(rank_length_);
        }
    }

    // スコアを登録する 順位(1から数える)を返す ランクインしていないなら-1を返す
    public int RegisterScore(int score, int game_index)
    {
        int rank = scores_[game_index].AddScore(score);
        if (rank != -1)
        {
            // スコアが更新されたのでセーブ申請を出す
            Save();
        }
        return rank;
    }

    // スコアを取得する
    public Score GetScoreData(int game_index)
    {
        return scores_[game_index];
    }

    // セーブ申請を出す 実際にセーブするときはSaveManager.Save()を使う
    public void Save()
    {
        if (save_completed_)
        {
            save_completed_ = false;
            SaveManager.Instance.RequestSave(() => { Save(kFileName); save_completed_ = true; });
        }
    }
}

public class ScoreManager : MonoBehaviour
{
#region field
    // 保存するスコアデータ
    [SerializeField]
    [HideInInspector]
    private ScoreData data_;

    [SerializeField]
    private GamePackageSet game_set_;

    // 何位まで保存するか
    [SerializeField]
    private int display_rank_length_ = 5;
    public static ScoreManager Instance { private set; get; }

    private const string kGotyamaze = "all";

    // ゲームのシーン名とゲームIDを対応させる
    private Dictionary<string, int> dic_to_id_;
    // ゲームのシーン名とゲームのタイトル名を対応させる
    private Dictionary<string, string> dic_to_name_;
    #endregion

    // 登録された最新のゲームシーン名
    public string LatestGameScene { private set; get; }
    // 登録された最新のゲームタイトル名
    public string LatestGame { private set; get; }
    // 登録された最新のスコアの順位
    public int LatestRank { private set; get; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            dic_to_id_ = new Dictionary<string, int>();
            dic_to_name_ = new Dictionary<string, string>();
            data_ = new ScoreData();
            data_.Init(game_set_.games.Length + 1, display_rank_length_);
            LatestGameScene = kGotyamaze;
            LatestGame = "ごちゃまぜ";
            LatestRank = -1;
            AssouciateGame();
            DontDestroyOnLoad(this);
        }
        else Destroy(this);
    }

    // スコアを取得する 引数は @game_scene_name=ゲームのシーン名
    public Score GetScoreData(string game_scene_name = kGotyamaze)
    {
        return data_.GetScoreData(dic_to_id_[game_scene_name]);
    }

    // スコアを登録する ランクを返す(ランク外の場合は-1) 引数は @score=得点，@game_scene_name=ゲームのシーン名
    public int RegisterScore(int score, string game_scene_name = kGotyamaze)
    {
        LatestGameScene = game_scene_name;
        LatestGame = dic_to_name_[game_scene_name];
        LatestRank = data_.RegisterScore(score, dic_to_id_[game_scene_name]);
        return LatestRank;
    }

    // スコアデータをリロードする
    public void Reload()
    {
        data_.Init(game_set_.games.Length + 1, display_rank_length_);
    }

    private void AssouciateGame()
    {
        // all(ごちゃまぜは0にする)
        dic_to_id_.Add(kGotyamaze, 0);
        dic_to_name_.Add(kGotyamaze, "ごちゃまぜ");

        for(int i = 0; i < game_set_.games.Length; ++i)
        {
            dic_to_id_.Add(game_set_.games[i].sceneName, i + 1);
            dic_to_name_.Add(game_set_.games[i].sceneName, game_set_.games[i].titleName);
        }

    }
}
