using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yoke
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private float interval = 0.5f;

        [SerializeField]
        private BulletController bullet;

        [SerializeField]
        private float height;
        [SerializeField]
        private float moveSpeed = 1.0f;

        private float dir = 1f;
        private float timer;

        // Start is called before the first frame update
        void Start()
        {
            timer = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            Move();

            timer += Time.deltaTime;
            if(timer >= interval)
            {
                timer = 0f;
                BulletController bulletObj = Instantiate(bullet, transform.position, Quaternion.identity);
                bulletObj.Init(new Vector2(-1, Random.Range(-0.5f, 0.5f)));
                //Instantiate(bullet, transform);
            }
        }

        private void Move()
        {
            transform.position += Vector3.up * moveSpeed * dir * Time.deltaTime;
            if(transform.position.y >= height)
            {
                dir = -1f;
            }
            else if(transform.position.y <= -height)
            {
                dir = 1f;
            }
        }
    }
}