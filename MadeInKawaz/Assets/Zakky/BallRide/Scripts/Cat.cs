using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    [SerializeField]
    Ball mBall;

    float mBallRadius;
    float mCatRadius;

    float time;
    // Start is called before the first frame update
    void Start()
    {
        //ボールと猫の半径求める
        mBallRadius = mBall.GetComponent<CircleCollider2D>().radius * mBall.transform.localScale.x;
        mCatRadius = GetComponent<CircleCollider2D>().radius * transform.localScale.x;

        transform.position = new Vector3(
            Random.Range(0, 2) == 0 ? Random.Range(-0.2f, -0.1f) : Random.Range(0.1f, 0.2f)
            + mBall.transform.position.x,
            mBall.transform.position.y + 4f,
            0f
            );

        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //ボールに猫が乗ってるとき
        if (transform.position.y > mBall.transform.position.y)
        {
            //ボールから猫へのベクトル
            Vector3 vec = transform.position - mBall.transform.position;
            //猫が浮いてるときボールと猫の距離常に一定
            if (vec.magnitude > mBallRadius + mCatRadius)
            {
                transform.position = vec.normalized * (mBallRadius + mCatRadius)
                + mBall.transform.position;
            }
            
            //猫をボールに合わせて回転
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, Mathf.Rad2Deg * Mathf.Atan2(vec.y, vec.x) - 90f));
        }

        //とりあえずクリア書いた
        time += Time.deltaTime;
        if (time > 3.8f && transform.position.y > -3.5f) GameManager.Clear();
    }
}
