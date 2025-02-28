using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yoke
{
    public class BulletController : MonoBehaviour
    {
        [SerializeField]
        private float init_speed = 5.0f;
        [SerializeField]
        private float accel_power = 1.0f;

        private float width, height;

        private Vector2 dir;

        private Vector2 accel;

        private Vector2 speed;

        public void Init(Vector2 _dir)
        {
            //accel = new Vector2(Random.Range(0f, accel_power), Random.Range(-accel_power, accel_power));
            accel = new Vector2(0, 0);
            dir = _dir;

            height = Camera.main.orthographicSize + 0.5f;
            width = height * Camera.main.aspect + 0.5f;

            speed = dir * init_speed;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            speed += accel * Time.deltaTime;

            transform.position += (Vector3)speed* Time.deltaTime;

            if(IsOut())
            {
                Destroy(this.gameObject);
            }
        }

        private bool IsOut()
        {
            return (transform.position.x < -width || transform.position.x > width ||
                   transform.position.y < -height || transform.position.y > height);
        }
    }
}