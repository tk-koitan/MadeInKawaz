using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{

    public GameObject EnemyRock;
    public GameObject EnemyScissors;
    public GameObject EnemyPaper;

    public GameObject Rock;
    public GameObject Scissors;
    public GameObject Paper;

    public GameObject RockButton;
    public GameObject ScissorsButton;
    public GameObject PaperButton;

    public GameObject ClearBackground;
    public GameObject FailureBackGround;

    private int n;
    private int Select;

    // Start is called before the first frame update
    void Start()
    {
        Select = 100;
        n = Random.Range(0, 3)+1 ;

        switch (n)
        {
            case 1:
                EnemyRock.SetActive(true);
                break;

            case 2:
                EnemyScissors.SetActive(true);
                break;

            case 3:
                EnemyPaper.SetActive(true);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (n == Select)
        {
            GameManager.Clear();
            ClearBackground.SetActive(true);
        }
        else
        {
            if (Select != 100)
            {
                FailureBackGround.SetActive(true);
            }
        }
        /*
        if (Input.GetMouseButtonDown(0))
        {
            GameManager.Clear();
        }
        */

    }

    void ClearButtons()
    {
        RockButton.SetActive(false);
        ScissorsButton.SetActive(false);
        PaperButton.SetActive(false);
    }

    public void PushRockButton()
    {
        ClearButtons();
        Select = 3;
        Rock.SetActive(true);
    }

    public void PushScissorsButton()
    {
        ClearButtons();
        Select = 1;
        Scissors.SetActive(true);
    }

    public void PushPaperButton()
    {
        ClearButtons();
        Select = 2;
        Paper.SetActive(true);
    }
}
