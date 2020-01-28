using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TadaLib.Save;
using DG.Tweening;
using UnityEngine.SceneManagement;

namespace Result
{
    public class ScoreDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI text_;
        // もう一度，やめるのボタンを誤って押してしまうのを防ぐのに使う
        [SerializeField]
        private Button button_blocker_;

        // 背景を徐々に暗くする
        [SerializeField]
        private SpriteRenderer background_;

        [SerializeField]
        private GamePackageSet set_;

        private ScoreManager score_manager_;

        // Start is called before the first frame update
        void Start()
        {
            score_manager_ = ScoreManager.Instance;

            text_.rectTransform.DOShakePosition(1, 3).SetLoops(-1);
            background_.DOFade(0.5f, 1.0f);

            StartCoroutine(NamaeOmoitukan(2.0f));

            Display(score_manager_.LatestGameScene, score_manager_.LatestGame, score_manager_.LatestRank);
        }

        private void Display(string game_scene_name, string game_name, int rank = -1)
        {
            string res = "";
            res += game_name;
            res += "\nスコアランキング\n";
            Score score = score_manager_.GetScoreData(game_scene_name);
            for (int i = 0, n = score.Scores.Count; i < n; ++i)
            {
                if (i == rank - 1) res += "<color=red>";
                res += (i + 1).ToString() + "位";
                res += String.Format("{0, 6}", score.Scores[i].ToString());
                if (i == rank - 1) res += "</color>";
                res += "\n";
            }
            text_.text = res;

            // 順位の表示量に応じてテキストのフォントサイズを変更する
            text_.fontSize = 50 - 3 * score.Scores.Count;
        }

        private IEnumerator NamaeOmoitukan(float time)
        {
            yield return new WaitForSeconds(time);

            DisableButtonBlocker();
        }

        public void DisableButtonBlocker()
        {
            button_blocker_.gameObject.SetActive(false);
        }

        public void Retry()
        {
            //ゲームモードの変更タイミングを変えた by koitan
            //GameManager.mode = GameManager.PlayMode.Normal;
            GameManager.Instance.Initialization();
            //GameManager.TrasitionScene("ResultScene", "ManagerScene");            
            SceneManager.UnloadSceneAsync("ResultScene");
        }

        public void End()
        {
            //このタイミングでゲームモードをNoneにする by koitan
            GameManager.mode = GameManager.PlayMode.None;
            //GameManager.TrasitionScene("ResultScene", "Title");
            SceneManager.LoadSceneAsync("Title", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("ResultScene");
        }
    }
} // namespace Result