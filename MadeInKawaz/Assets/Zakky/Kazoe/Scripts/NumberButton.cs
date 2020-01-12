using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberButton : MonoBehaviour
{
    // Start is called before the first frame update
    bool canPush = true;
    int numberIndex;
    int leftNumber;

    public GameObject movingThingSpawner;
    void Start()
    {
        numberIndex = Random.Range(0, 3);
        //一番左の数字を正解の数字から-0~-2する
        leftNumber = movingThingSpawner.GetComponent<MovingThingSpawner>().correctNumber - numberIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        //ボタンが押せる
        if (canPush)
        {
            //ボタン三つ分ループ
            for (int i = 0; i < 3; i++)
            {
                //ボタンが押された時
                if (GUI.Button(new Rect(Screen.width / 2 + 200 * (i - 1) - 80, Screen.height - 200, 150, 150), (leftNumber + i).ToString()))
                {
                    //ボタンに表示されてる数字と一致したら
                    if (leftNumber +i  == movingThingSpawner.GetComponent<MovingThingSpawner>().correctNumber)
                    {
                        //ゲームクリア
                        GameManager.Clear();
                    }
                    //一回おしたらボタン押せなくなる
                    canPush = false;
                }
            }
        }
    }
}
