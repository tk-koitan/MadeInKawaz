using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hitode
{
    public class Fairy : MonoBehaviour
    {
        // 1フレーム前のマウス座標
        Vector3 prevPos;
        // 画面の幅と高さ
        [SerializeField]float width, height;
        // プレイヤーの位置
        [SerializeField] float playerHeight;

        //
        Rigidbody2D rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 touchScreenPos = Input.mousePosition;
            touchScreenPos.z = 10.0f;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(touchScreenPos);
            if(Input.GetMouseButtonDown(0))
            {
                prevPos = mousePos;
            }

            if(Input.GetMouseButton(0))
            {
                //Vector3 moveDir = mousePos - prevPos;
                //rb.velocity = moveDir * Time.deltaTime * 100.0f;
                transform.Translate(mousePos - prevPos);

                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -width - 2.0f, width), playerHeight);
                prevPos = mousePos;
            }
        }
    }
}
