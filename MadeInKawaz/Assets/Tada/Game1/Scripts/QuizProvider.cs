using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using TadaGame1;

/// <summary>
/// 同じ絵を当てるクイズの内容を決めるクラス
/// </summary>

namespace TadaGame1
{
    public class QuizProvider : MonoBehaviour
    {
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
        private Sprite clear_background_;
        [SerializeField]
        private Sprite failed_background_;

        // Start is called before the first frame update
        void Start()
        {
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

            GameManager.ClearActionQueue.Enqueue(() =>
            {
                // アイテムにアニメーションを
                for(int i = 0; i < item_num; ++i)
                {
                    items_[i].DoAnimate(true);
                }
                background_.sprite = clear_background_;
            });
            //GameManager.FailedActionQueue.Enqueue(() =>
            //{
            //    // アイテムにアニメーションを
            //    for (int i = 0; i < item_num; ++i)
            //    {
            //        items_[i].DoAnimate(false);
            //    }
            //    background_.sprite = failed_background_;
            //});
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                // アイテムにアニメーションを
                for (int i = 0; i < items_.Length; ++i)
                {
                    items_[i].DoAnimate(true);
                }
                background_.sprite = clear_background_;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                // アイテムにアニメーションを
                for (int i = 0; i < items_.Length; ++i)
                {
                    items_[i].DoAnimate(false);
                }
                background_.sprite = failed_background_;
            }
        }
    }
} // namespace TadaGame1