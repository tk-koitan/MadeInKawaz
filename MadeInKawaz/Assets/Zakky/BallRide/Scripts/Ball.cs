using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D mRigidbody2D;
    float mBallRadius;

    // マウスの一フレーム前の座標
    Vector3 oldPos;

    void Awake()
    {
        transform.position = new Vector3(Random.Range(-1f, 1f), -1f, 0f);
    }
    // Start is called before the first frame update
    void Start()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();
        mBallRadius = GetComponent<CircleCollider2D>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 touchScreenPosition = Input.mousePosition;

        // 10.0fに深い意味は無い。画面に表示したいので適当な値を入れてカメラから離そうとしているだけ.
        touchScreenPosition.z = 10.0f;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(touchScreenPosition);

        float vel = SignOrZero(Input.GetAxis("Horizontal"));
        if (Input.GetMouseButtonDown(0))
        {
            oldPos = mousePos;
        }

        if (Input.GetMouseButton(0))
        {
            //transform.Translate(new Vector3(mousePos.x - oldPos.x, 0f, 0f));
            vel = SignOrZero(mousePos.x - oldPos.x);
            //画面外に出ないように
            //transform.position = new Vector3(Mathf.Clamp(transform.position.x, -width, width), Mathf.Clamp(transform.position.y, -height, height));
            oldPos = mousePos;
        }

        mRigidbody2D.velocity += new Vector2(100f *  vel * Time.deltaTime, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * -transform.position.x / mBallRadius);

        
    }

    float SignOrZero(float a)
    {
        if (a == 0) return 0;
        else return Mathf.Sign(a);
    }
}
