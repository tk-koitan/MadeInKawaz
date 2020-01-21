using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TadaLib.Save;
using UnityEngine.SceneManagement;
using TMPro;

namespace Test
{
    public class ResultInvoker : MonoBehaviour
    {
        [SerializeField]
        private InputField field_;

        [SerializeField]
        private TextMeshProUGUI text_;

        [SerializeField]
        private GamePackageSet set_;

        private Scene additive_scene_;

        private bool is_additive_ = false;

        private string game_name_ = "all";

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                game_name_ = set_.games[0].sceneName;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                game_name_ = set_.games[1].sceneName;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                game_name_ = set_.games[2].sceneName;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                game_name_ = set_.games[3].sceneName;
            }

            text_.text = game_name_;

            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveManager.Instance.Save();
            }
        }

        public void ResultInvoke()
        {
            int score = 0;
            Int32.TryParse(field_.text, out score);
            int rank = ScoreManager.Instance.RegisterScore(score, game_name_);

            //if(is_additive_) SceneManager.UnloadSceneAsync(additive_scene_);

            SceneManager.LoadScene("ResultScene", LoadSceneMode.Additive);
            additive_scene_ = SceneManager.GetSceneByName("ResultScene");
            is_additive_ = true;
        }
    }
}