using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField]
    Ball mBall;
    Rigidbody2D mRigidbody2D;

    float mCatRadius;

    ZakkyLib.Timer mClearTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        ComponentSetter();

        IniPos();

        mClearTimer = new ZakkyLib.Timer(3.8f);
    }

    void ComponentSetter()
    {
        //猫の半径求める
        mCatRadius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;
        mRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void IniPos()
    {
        transform.position = new Vector3(
            Random.Range(0, 2) == 0 ? Random.Range(-0.2f, -0.1f) : Random.Range(0.1f, 0.2f) + mBall.transform.position.x,
            mBall.transform.position.y + 5f,
            0f
            );
    }

    // Update is called once per frame
    void Update()
    {
        //猫移動
        CatMove();

        //クリアチェック
        ClearCheck();
    }

    void CatMove()
    {
        //ボールに猫が乗ってるとき
        if (IsOnBall())
        {
            //猫が浮いてるときボールと猫の距離常に一定
            if (IsCatFlying())
            {
                //にする
                CatClamp();
            }

            //猫をボールの中心向かせる
            CatRotation();
        }
    }

    Vector3 Vec3FromBallToCat()
    {
        return transform.position - mBall.transform.position;
    }

    bool IsOnBall()
    {
        return transform.position.y > mBall.transform.position.y;
    }

    bool IsCatFlying()
    {
        return Vec3FromBallToCat().magnitude > mBall.mBallRadius + mCatRadius;
    }

    void CatClamp()
    {
        //移動
        transform.position = Vec3FromBallToCat().normalized * (mBall.mBallRadius + mCatRadius)
                + mBall.transform.position;

        //ボールから外側への速度を内積とって計算して0にする
        Vector2 vec = Vec3FromBallToCat().normalized * Vector2.Dot(Vec3FromBallToCat().normalized, mRigidbody2D.velocity);
        mRigidbody2D.velocity -= vec;
    }

    void CatRotation()
    {
        //猫をボールに合わせて回転
        transform.rotation = Quaternion.Euler(
            new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(Vec3FromBallToCat().y, Vec3FromBallToCat().x) - 90f)
            );
    }

    void ClearCheck()
    {
        //とりあえずクリア書いた
        if (mClearTimer.IsTimeout() && transform.position.y > -3.5f) GameManager.Clear();
    }
}
