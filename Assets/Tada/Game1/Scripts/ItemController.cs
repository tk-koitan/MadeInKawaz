using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace TadaGame1
{
    //[RequireComponent(typeof(SpriteRenderer))]
    //[RequireComponent(typeof(Animator))]
    public class ItemController : MonoBehaviour
    {
        #region field
        [SerializeField]
        private GameObject body;

        // SpriteRendererだけなぜか正しく取得できない・・・なんで？
        [SerializeField]
        private SpriteRenderer sprite_ren_;
        private Animator animator_;
        private BoxCollider2D hit_box_;

        private bool is_atari_ = false;

        [SerializeField]
        private QuizProvider provider_;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            //本ビルドと区別がないので今は無効
            //DebugBoxManager.Display(this).SetSize(200, 200).SetAlignment(TMPro.TextAlignmentOptions.Center);
            DebugTextManager.Display(() => transform.position.x.ToString() + ((is_atari_) ? "当たり" : "はずれ") + "\n").AddRemoveTrigger(this);
            // sprite_ren_ = body.GetComponent<SpriteRenderer>();
            animator_ = body.GetComponent<Animator>();
            hit_box_ = body.GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            // 思いつかなかった
            if (!provider_.IsFinished)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (IsClicked())
                    {
                        if (is_atari_)
                        {
                            // 持ち上げる
                            transform.position += Vector3.up * 0.5f;
                            transform.DOMoveY(transform.position.y + 1f, 1f).SetEase(Ease.OutQuad);
                        }
                        provider_.RequestResult(is_atari_);
                    }
                }
            }
        }

        // 範囲内をクリックされたかどうか
        private bool IsClicked()
        {
            float left = body.transform.position.x - (hit_box_.size.x * body.transform.localScale.x / 2f);
            float right = body.transform.position.x + (hit_box_.size.x * body.transform.localScale.x / 2f);
            float top = body.transform.position.y + (hit_box_.size.y * body.transform.localScale.y / 2f);
            float bottom = body.transform.position.y - (hit_box_.size.y * body.transform.localScale.y / 2f);

            // マウスの場所
            Vector3 mouse_position = Input.mousePosition;
            Vector3 world_mosue_position = Camera.main.ScreenToWorldPoint(mouse_position);

            // 範囲内かどうか
            if (left > world_mosue_position.x) return false;
            if (right < world_mosue_position.x) return false;
            if (top < world_mosue_position.y) return false;
            if (bottom > world_mosue_position.y) return false;

            return true;
        }


        // クリアしたときのアニメーション
        public void DoAnimate(bool is_clear)
        {
            if (is_clear)
            {
                animator_.Play("Clear");
                return;
            }
            animator_.Play("Failed");
        }

        // スプライトを変更する
        public void ChangeSprite(Sprite spr)
        {
            sprite_ren_.sprite = spr;
        }

        // 当たりのアイテムかどうかセットする
        public void SetIsAtari(bool atari)
        {
            is_atari_ = atari;
        }

        public override string ToString()
        {
            return (is_atari_) ? "正解" : "不正解";
        }
    }
} // namespace TadaGame1