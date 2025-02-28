using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    // マウスの一フレーム前の座標
    Vector3 oldPos;
    // 画面幅，高さ
    [SerializeField]
    private float width, height;

    [SerializeField]
    private GameObject gestureHand;
    [SerializeField]
    private GameObject explotion;

    float time;
    bool death;
    // Start is called before the first frame update
    void Start()
    {
        time = 0f;
        death = false;
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
            gestureHand.SetActive(false);
        }

        if (Input.GetMouseButton(0))
        {
            transform.Translate(mousePos - oldPos);

            //画面外に出ないように
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -width, width), Mathf.Clamp(transform.position.y, -height, height));
            oldPos = mousePos;
        }
        time += Time.deltaTime;
        if (time > 7.8f && !death) GameManager.Clear();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        death = true;
        //爆破エフェクトだす
        Instantiate(explotion, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
