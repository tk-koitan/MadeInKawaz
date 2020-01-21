using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TadaLib.Save;
using DG.Tweening;

namespace Test
{
    public class ScoreChanger : MonoBehaviour
    {

        [SerializeField]
        private TextMeshProUGUI text_;
        [SerializeField]
        private InputField input_field;
        // もう一度，やめるのボタンを誤って押してしまうのを防ぐのに使う
        [SerializeField]
        private Button button_blocker_;

        [SerializeField]
        private GamePackageSet set_;

        private ScoreManager score_manager_;

        private string current_game = "all";

        // Start is called before the first frame update
        void Start()
        {
            score_manager_ = ScoreManager.Instance;

            text_.rectTransform.DOShakePosition(1, 3).SetLoops(-1);

            StartCoroutine(NamaeOmoitukan(2.0f));

            Display(ScoreManager.Instance.LatestGame, ScoreManager.Instance.LatestRank);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Display(set_.games[0].sceneName);
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                Display(set_.games[1].sceneName);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Display(set_.games[2].sceneName);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                Display(set_.games[3].sceneName);
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveManager.Instance.Save();
            }
        }

        // inputfieldからの入力
        public void ChangeScore()
        {
            int score = 0;
            Int32.TryParse(input_field.text, out score);
            AddScore(score, current_game);
        }

        private void AddScore(int score, string game_scene_name)
        {
            int rank = score_manager_.RegisterScore(score, game_scene_name);
            Display(game_scene_name, rank);
        }

        private void Display(string game_scene_name, int rank = -1)
        {
            current_game = game_scene_name;

            string res = "";
            res += game_scene_name;
            res += "\nスコアランキング\n";
            Score score = score_manager_.GetScoreData(game_scene_name);
            for(int i = 0, n = score.Scores.Count; i < n; ++i)
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
    }
}