using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yoke
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField]
        private int n_way = 3;
        [SerializeField]
        private float dir_diff = 30f;
        [SerializeField]
        private float interval = 0.5f;

        [SerializeField]
        private BulletController bullet;

        [SerializeField]
        private float height;
        [SerializeField]
        private Vector2 moveSpeedRange = new Vector2(0.5f, 1.5f);
        private float moveSpeed;

        private Pool pool;

        //by koitan
        private AudioSource audioSource;


        private float dir = -1f;

        // Start is called before the first frame update
        void Start()
        {
            pool = GetComponent<Pool>();
            audioSource = GetComponent<AudioSource>();

            moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
            StartCoroutine(BulletShot(n_way, dir_diff, interval));
            transform.position = new Vector2(transform.position.x, Random.Range(-height, height));
        }

        // Update is called once per frame
        void Update()
        {
            Move();
        }

        private IEnumerator BulletShot(int n, float dir, float interval)
        {
            while (true)
            {
                yield return new WaitForSeconds(interval);
                // n way shot
                //++n;
                audioSource.Play();
                for (int i = -n / 2; i <= n / 2; ++i)
                {
                    GameObject bulletObj = pool.GetInstance();
                    Vector3 pos = transform.position;
                    bulletObj.transform.position = new Vector3(pos.x, pos.y, 0f);
                    bulletObj.GetComponent<BulletController>().Init(Quaternion.Euler(0f, 0f, dir * i) * Vector2.left);
                }
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