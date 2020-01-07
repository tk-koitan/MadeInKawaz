using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace yoke
{
    public class PlayerController : MonoBehaviour
    {
        Vector3 oldPos;
        [SerializeField]
        private float width, height;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Vector3 touchScreenPosition = Input.mousePosition;

            // 10.0fに深い意味は無い。画面に表示したいので適当な値を入れてカメラから離そうとしているだけ.
            touchScreenPosition.z = 10.0f;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(touchScreenPosition);
            //mousePos.z = 0;            

            if (Input.GetMouseButtonDown(0))
            {
                oldPos = mousePos;
            }

            if (Input.GetMouseButton(0))
            {
                transform.Translate(mousePos - oldPos);

                //画面外に出ないように
                transform.position = new Vector3(Mathf.Clamp(transform.position.x, -width, width), Mathf.Clamp(transform.position.y, -height, height));
                oldPos = mousePos;
            }
        }
    }
}


