using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knifecontoller : MonoBehaviour
{
    public static int success = 0;
    public static int fail = 0;
    static bool clearOT = true;
    public AudioClip cut;
    public AudioClip suc;
    public AudioClip fai;
    public AudioSource A;
    void Start()
    {
        A = GetComponent<AudioSource>();
        clearOT = true;
        success = 0;
        fail = 0;
    }

    void Update()
    {
        if (timekeeper.time>1.9f)
        {
            transform.Translate(7.998f * Time.deltaTime, 1.5f * Time.deltaTime, 0);
            if (timekeeper.time > 1.9f)
            {
                if (Input.GetMouseButtonDown(0))
                {
                     A.PlayOneShot(cut);
                    //当たり判定はこっちで設定するよ
                    if (2.4f<=timekeeper.time&&timekeeper.time<=2.6f|2.65f<=timekeeper.time&&timekeeper.time<=2.85f|2.9f<=timekeeper.time&&timekeeper.time<=3.1f)
                    {
                        success +=cutJ();
                    }
                    else
                    {
                        fail += cutJ();
                    }

                }
            }
            
        }
        //このままクリア判定も作っちゃう
        if (timekeeper.time >= 3.5f && clearOT)
        {
            Debug.Log(success);
            Debug.Log(fail);
            if (success >= 2 && fail <= 1)
            {
                GameManager.Clear();
                A.PlayOneShot(suc);
                clearOT = false;
            }
            else
            {
                A.PlayOneShot(fai);
                clearOT = false;
            }
        }
    }
    public int cutJ()
    {
        transform.Translate(0, -2, 0);
        StartCoroutine(Up());
        IEnumerator Up()
        {
            yield return new WaitForSeconds(0.1f);
            transform.Translate(0, 2, 0);
        }
        return 1;
        //ナイフが動く＆1返す
    }
}
