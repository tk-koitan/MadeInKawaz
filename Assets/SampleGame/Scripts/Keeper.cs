using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SampleGame;

namespace SampleGame
{
    public class Keeper : MonoBehaviour
    {
        [SerializeField]
        private float minX;
        [SerializeField]
        private float maxX;
        [SerializeField]
        private float speed;
        [SerializeField]
        private Sprite spr;

        private float velocity;
        // Start is called before the first frame update
        void Start()
        {
            float rand = Random.Range(0, 1f);
            Vector3 pos = transform.position;
            pos.x = minX + (maxX - minX) * rand;
            transform.position = pos;
            if (rand < 0.5f)
            {
                velocity = speed;
            }
            else
            {
                velocity = -speed;
            }

            GameManager.ClearActionQueue.Enqueue(() =>
            {
                velocity = 0;
                GetComponent<SpriteRenderer>().sprite = spr;
                transform.Translate(0, -2, 0);
            });

            //デバッグ用
            //DebugBoxManager.Display(this).SetSize(200, 200);
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += Vector3.right * velocity * Time.deltaTime;
            if (velocity > 0 && transform.position.x > maxX)
            {
                velocity = -speed;
            }
            else if (velocity < 0 && transform.position.x < minX)
            {
                velocity = speed;
            }

            /*
            if (GameManager.ClearFlag)
            {
                velocity = 0;
                GetComponent<SpriteRenderer>().sprite = spr;
                transform.Translate(0, -2, 0);
            }
            */
        }

        public override string ToString()
        {
            return velocity.ToString();
        }
    }
}
