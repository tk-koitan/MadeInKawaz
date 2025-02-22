using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TadaGame2
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ClearChecker : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer finish_obj_;
        [SerializeField]
        private Vector2 finish_move_pos_;
        [SerializeField]
        private float move_duration_;

        // ゲーム後に流す画像
        [SerializeField]
        private Sprite clear_sprite_;
        [SerializeField]
        private Sprite failed_sprite_;

        [SerializeField]
        private SpriteRenderer background_;

        // Start is called before the first frame update
        void Start()
        {
           // finish_obj_.transform.position = finish_move_pos_;
           // GameManager.ClearActionQueue.Enqueue(() =>
           //{
           //    if (GameManager.ClearFlag)
           //    {
           //        finish_obj_.sprite = clear_sprite_;
           //         // 背景を明るくする
           //         background_.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), move_duration_ / 4f);
           //    }
           //    else
           //    {
           //        finish_obj_.sprite = failed_sprite_;
           //    }

           //     // 左から右へと画像を表示させる
           //     finish_obj_.transform.position = finish_move_pos_;
           //    finish_obj_.transform.DOMoveX(0f, move_duration_ / 2f).SetEase(Ease.OutQuart).OnComplete(() =>
           //    finish_obj_.transform.DOMoveX(-finish_move_pos_.x, move_duration_ / 2f).SetEase(Ease.InQuart));
           //});
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (GameManager.ClearFlag) return;

            float goal_y = transform.position.y + GetComponent<CircleCollider2D>().offset.y * transform.localScale.y;

            // 下向きの速度があり，かつゴールの上から入ったらクリア
            if (collision.GetComponent<Rigidbody2D>().linearVelocity.y < 0f && goal_y < collision.transform.position.y)
            {
                GameManager.Clear();
                finish_obj_.transform.position = finish_move_pos_;
                finish_obj_.sprite = clear_sprite_;
                // 背景を明るくする
                background_.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), move_duration_ / 4f);
                // 左から右へと画像を表示させる
                finish_obj_.transform.position = finish_move_pos_;
                finish_obj_.transform.DOMoveX(0f, move_duration_ / 2f).SetEase(Ease.OutQuart).OnComplete(() =>
                finish_obj_.transform.DOMoveX(-finish_move_pos_.x, move_duration_ / 2f).SetEase(Ease.InQuart));

                AudioSource audio = GetComponent<AudioSource>();
                audio.Play();
            }
        }
    }
} // namespace TadaGame2