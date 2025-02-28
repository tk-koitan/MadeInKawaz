using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems; // 追加
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private bool buttonDownFlag = false;
    Vector3 mouseInitPos;
    Vector3 playerInitPos;
    Vector3 pos_before;
    Vector3 diff;
    Vector3 pos;
    bool flag = true;
    public Sprite Hitsugi;
    public Sprite Joy;
    float x_min = -8.86f;
    float x_max = 8.86f;
    float y_min = -2.9f;
    float y_max = 4.83f;

    private void Start()
    {
        playerInitPos = new Vector3(8.86f, -2.9f, 0);
        pos_before = new Vector3(8.86f, -2.9f, 0);
    }

    private void Update()
    {
        

        if (flag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 objectScreenPoint =
                    new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);

                mouseInitPos = Camera.main.ScreenToWorldPoint(objectScreenPoint);
            }

            if (Input.GetMouseButton(0))
            {
                //マウスカーソル及びオブジェクトのスクリーン座標を取得
                Vector3 objectScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);

                //スクリーン座標をワールド座標に変換
                Vector3 objectWorldPoint = Camera.main.ScreenToWorldPoint(objectScreenPoint);

                diff = objectWorldPoint - mouseInitPos;
                pos = playerInitPos + diff;
                
                pos.x = Mathf.Clamp(pos.x, x_min, x_max);
                pos.y = Mathf.Clamp(pos.y, y_min, y_max);

                this.gameObject.transform.position = pos;
                

            }

            if (Input.GetMouseButtonUp(0))
            {
                playerInitPos = this.gameObject.transform.position;
            }
        }
        


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            Debug.Log("ゴール");
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Joy;
            flag = false;
            GameManager.Clear();
            
        }
        else if (other.gameObject.CompareTag("Fire"))
        {
            Debug.Log("接触");
            flag = false;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = Hitsugi;
        }
    }
}
