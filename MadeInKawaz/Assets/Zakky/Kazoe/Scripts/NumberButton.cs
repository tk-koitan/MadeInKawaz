using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberButton : MonoBehaviour
{
    public GameObject movingThingSpawner;

    [SerializeField]
    GameObject[] buttons;

    [System.NonSerialized]
    public bool canPush = true;
    [System.NonSerialized]
    public int correctNumber;
    [System.NonSerialized]
    public bool stageClear = false;

    int numberIndex;
    int leftNumber;

    // Start is called before the first frame update
    void Start()
    {
        numberIndex = Random.Range(0, buttons.Length);
        correctNumber = movingThingSpawner.GetComponent<MovingThingSpawner>().correctNumber;
        //一番左の数字を正解の数字から-0~-2する
        leftNumber = correctNumber - numberIndex;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetComponent<Buttons>().buttonNumber = leftNumber + i;
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    
    //}

    /*
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
                    if (leftNumber + i  == movingThingSpawner.GetComponent<MovingThingSpawner>().correctNumber)
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
    */
}
