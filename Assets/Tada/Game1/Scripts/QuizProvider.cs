using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaGame1;
using DG.Tweening;

/// <summary>
/// 同じ絵を当てるクイズの内容を決めるクラス
/// </summary>

namespace TadaGame1
{
    public class QuizProvider : MonoBehaviour
    {
        // ゲームが終了したかどうか
        public bool IsFinished { private set; get; }

        #region field
        // 対象のスプライト
        [SerializeField]
        private Sprite[] sprites_;

        // スプライトを割り当てる対称
        [SerializeField]
        private ItemController[] items_;

        // 今回の問題の答えとなる対象
        [SerializeField]
        private SpriteRenderer answer_;

        // 背景
        [SerializeField]
        private SpriteRenderer background_;
        // 当たった時の背景とはずれの時の背景
        [SerializeField]
        private Color clear_background_;
        [SerializeField]
        private Color failed_background_;
        #endregion

        // Start is called before the first frame update
        void Start()
        {
            IsFinished = false;

            int item_num = items_.Length;
            int sprite_num = sprites_.Length;

            Assert.IsFalse(sprite_num <= 1);

            // すべてのスプライトを使わざるを得ない場合
            if (sprite_num < item_num)
            {
                // スプライトの中から一つあたりを決める
                int answer_index = Random.Range(0, sprite_num);

                answer_.sprite = sprites_[answer_index];

            }
            else
            {
                int[] selected_indicies = new int[item_num];

                // 乱数でitem_num個選ぶ
                int[] indicies = new int[sprite_num];
                for (int i = 0; i < sprite_num; ++i)
                {
                    indicies[i] = i;
                }
                for (int i = 0; i < item_num; ++i)
                {
                    int j = Random.Range(0, item_num - i);
                    selected_indicies[i] = indicies[j];
                    indicies[j] = indicies[sprite_num - 1 - i];
                    indicies[sprite_num - 1 - i] = selected_indicies[i];
                }

                // スプライトの中から一つあたりを決める
                int answer_index = Random.Range(0, item_num);
                answer_.sprite = sprites_[selected_indicies[answer_index]];

                for (int i = 0; i < item_num; ++i)
                {
                    items_[i].ChangeSprite(sprites_[selected_indicies[i]]);
                    items_[i].SetIsAtari(i == answer_index);
                }
            }
        }

        public void RequestResult(bool clear)
        {
            IsFinished = true;
            Debug.Log("end");

            if (clear)
            {
                GameManager.Clear();
                // アイテムにアニメーションを
                for (int i = 0; i < items_.Length; ++i)
                {
                    items_[i].DoAnimate(true);
                }
                background_.color = clear_background_;
                answer_.DOColor(new Color(1.0f, 1.0f, 1.0f, 1.0f), 1.0f);
            }
            else
            {
                // アイテムにアニメーションを
                for (int i = 0; i < items_.Length; ++i)
                {
                    items_[i].DoAnimate(false);
                }
                background_.color = failed_background_;
            }
        }
    }
} // namespace TadaGame1