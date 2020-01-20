using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hitode
{
    public class HitodeContoroller : MonoBehaviour
    {
        // ヒトデの速さ
        [SerializeField] float speed;
        // ヒトデの進行方向
        Vector3 dir;
        // 画面の幅と高さ
        float screenW, screenH;
        // Start is called before the first frame update
        void Start()
        {
            screenH = Camera.main.orthographicSize + 0.5f;
            screenW = screenH * Camera.main.aspect + 0.5f;
            dir = new Vector3(-1.0f, -1.0f).normalized;
        }

        public void Initialize(Vector3 pos, Vector3 direction, float speed)
        {
            transform.position = pos;
            dir = direction;
            this.speed = speed;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position += speed * Time.deltaTime * dir;
            if(transform.position.x < -screenW || transform.position.x > screenW + 5.0f || transform.position.y < -screenH || transform.position.y > screenH + 5.0f)
            {
                Destroy(this.gameObject);
            }

            transform.Rotate(0.0f, 0.0f, 60.0f * Time.deltaTime, Space.Self);
        }

        public void Catch()
        {
            speed = 0.0f;

        }
    }
}