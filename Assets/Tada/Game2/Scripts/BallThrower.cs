using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

// バスケットボールを投げるクラス

namespace TadaGame2
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallThrower : MonoBehaviour
    {
        [SerializeField]
        private float throw_power_;

        [SerializeField]
        private GameObject arrow_;

        [SerializeField]
        private float gravity_;

        [SerializeField]
        private GameObject predict_ball_prefab_;

        // どの方向に飛んでいくかの予測
        private GameObject[] predict_balls_;

        [SerializeField]
        private float predict_interval_;
        [SerializeField]
        private float predict_time_;

        [SerializeField]
        private float rotate_period_;

        private int ball_num_;

        private Rigidbody2D rigidbody_;

        private bool is_throwed_;

        private Vector3 default_pos_;

        // Start is called before the first frame update
        void Start()
        {
            rigidbody_ = GetComponent<Rigidbody2D>();

            is_throwed_ = false;
            default_pos_ = transform.position;

            // 予測ボールを出す
            // 出す数
            ball_num_ = (int)(predict_time_ / predict_interval_ + 0.5f);
            predict_balls_ = new GameObject[ball_num_];
            for(int i = 0; i < ball_num_; ++i)
            {
                predict_balls_[i] = Instantiate(predict_ball_prefab_, new Vector3(0f, 0f, 0f), Quaternion.identity);
            }

            transform.DORotate(new Vector3(0, 0, -35f), rotate_period_).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        // Update is called once per frame
        void Update()
        {
            if (is_throwed_) return;

            if (Input.GetMouseButtonDown(0))
            {
                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
                is_throwed_ = true;
                // ボールを飛ばす
                EnDisableArrow();
                EnDisablePredictBalls();
                rigidbody_.gravityScale = gravity_;
                // バスケットゴールの淵の部分を現実っぽくしたいのでrigidbodyで管理
                rigidbody_.AddForce(transform.rotation * Vector2.up * throw_power_, ForceMode2D.Impulse);
                transform.DOKill();
                return;
            }
        }

        private void FixedUpdate()
        {
            if (is_throwed_) return;

            // 予測のボールを正しい位置に置く
            // これ大変そう
            for(int i = 0; i < ball_num_; ++i)
            {
                float t = i * predict_interval_;
                Vector3 pos = GetPredictBallPos(t);
                predict_balls_[i].transform.position = pos + default_pos_;
            }
        }

        // t秒後のボールの予測位置を求める
        private Vector2 GetPredictBallPos(float t)
        {
            Vector2 force = transform.rotation * Vector2.up * throw_power_ / 3.17f;

            float x = force.x * t;
            float y = force.y * t - 0.5f * gravity_ * (t * t + t * Time.fixedDeltaTime);

            return new Vector2(x, y);
        }

        // 予測のボールをすべて消す
        private void EnDisablePredictBalls()
        {
            for(int i = 0; i < ball_num_; ++i)
            {
                //predict_balls_[i].SetActive(false);
                predict_balls_[i].GetComponent<SpriteRenderer>().DOFade(0f, 1.0f);
            }
        }
        // 矢印を消す
        private void EnDisableArrow()
        {
            //arrow_.SetActive(false);
            arrow_.GetComponent<SpriteRenderer>().DOFade(0f, 0.5f);
        }
    }
}