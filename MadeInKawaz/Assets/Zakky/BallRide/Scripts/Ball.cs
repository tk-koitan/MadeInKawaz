using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    Rigidbody2D mRigidbody2D;
    public float mBallRadius
    {
        get;
        private set;
    }

    // マウスの一フレーム前の座標(クラスにして勝手に取ってほしい？)
    Vector3 oldPos;

    void Awake()
    {
        transform.position = new Vector3(Random.Range(-1f, 1f), -1f, 0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        mRigidbody2D = GetComponent<Rigidbody2D>();
        mBallRadius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        BallMove();
    }

    void BallMove() 
    {
        AddVelocity();

        ScreenClamp();

        BallRotation();
    }

    void AddVelocity()
    {
        mRigidbody2D.velocity += new Vector2(100f * xInput() * Time.deltaTime, 0f);
    }

    //返り値は-1, 0, 1のいずれか
    float xInput()
    {
        float vel = SignOrZero(Input.GetAxis("Horizontal"));

        Vector3 touchScreenPosition = Input.mousePosition;

        // 10.0fに深い意味は無い。画面に表示したいので適当な値を入れてカメラから離そうとしているだけ.
        touchScreenPosition.z = 10.0f;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(touchScreenPosition);

        if (Input.GetMouseButtonDown(0))
        {
            oldPos = mousePos;
        }

        if (Input.GetMouseButton(0))
        {
            vel = SignOrZero(mousePos.x - oldPos.x);
            oldPos = mousePos;
        }

        return vel;
    }

    void ScreenClamp()
    {
        if (-11f + mBallRadius > transform.position.x && mRigidbody2D.velocity.x < 0f)
        {
            Vector3 vec = transform.position;
            vec.x = -11f + mBallRadius;
            transform.position = vec;
            mRigidbody2D.velocity = Vector2.zero;
        }
        else if (11f - mBallRadius < transform.position.x && mRigidbody2D.velocity.x > 0f)
        {
            Vector3 vec = transform.position;
            vec.x = 11f - mBallRadius;
            transform.position = vec;
            mRigidbody2D.velocity = Vector2.zero;
        }
    }

    void BallRotation()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Rad2Deg * -transform.position.x / mBallRadius);
    }

    float SignOrZero(float a)
    {
        if (a == 0) return 0;
        else return Mathf.Sign(a);
    }
}
