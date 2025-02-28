using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour
{
    public GameObject LookAtPoint;
    public float rotateSpeed = 3.0f;
    public float distance = 5.0f;
    public bool ReverseVertical = false;
    [SerializeField] float angle_x= 0;
    [SerializeField] float angle_y = 0;
    [SerializeField] float angle_y_min = 5;
    [SerializeField] float angle_y_max = 60;

    private Vector2 oldTouchPos;
    private Vector2 delta;
    public float touchSpeed = 0.3f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //注目する対象の位置を取得
        Vector3 pos = LookAtPoint.transform.position;

        //入力取得
        float input_x = Input.GetAxis("Horizontal");
        float input_y = Input.GetAxis("Vertical");

        //タッチ対応
        if(Input.touchCount==2)
        {
            if(Input.touches[1].phase == TouchPhase.Began)
            {
                oldTouchPos = (Input.touches[0].position + Input.touches[1].position) / 2;
                delta = Vector2.zero;
            }
            else
            {
                Vector2 touchPos = (Input.touches[0].position + Input.touches[1].position) / 2;
                delta = (touchPos - oldTouchPos) * touchSpeed;
                oldTouchPos = touchPos;
            }
            input_x = delta.x;
            input_y = delta.y;
        }

        if (ReverseVertical)
        {
            //必要なら反転
            input_y *= -1;
        }

        //回転
        angle_x = Mathf.Repeat(angle_x - input_x * rotateSpeed, 360);
        angle_y = Mathf.Clamp(angle_y + input_y * rotateSpeed, angle_y_min, angle_y_max);


        //位置を調整
        var dx = angle_x * Mathf.Deg2Rad;
        var dy = angle_y * Mathf.Deg2Rad;
        transform.position = new Vector3(pos.x + distance * Mathf.Sin(dy) * Mathf.Cos(dx), pos.y + distance * Mathf.Cos(dy), pos.z + distance * Mathf.Sin(dx) * Mathf.Sin(dy));

        //対象の方を向く
        transform.LookAt(LookAtPoint.transform);

    }
}
