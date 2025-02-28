using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class Buttons : MonoBehaviour
{
    public GameObject numberButton;

    [System.NonSerialized]
    public int buttonNumber;

    NumberButton numberButtonScript;

    // Start is called before the first frame update
    void Start()
    {
        numberButtonScript = numberButton.GetComponent<NumberButton>();

        foreach (Transform child in transform)
        {
            //if (child.name == "Text(TMP)")
            //{
            child.GetComponent<TextMeshProUGUI>().text = buttonNumber.ToString();
            //}
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!numberButtonScript.canPush)
        {
            //Button btn = GetComponent<Button>();
            //btn.interactable = false;
            gameObject.SetActive(false);
        }
    }

    public void OnClick()
    {
        if (numberButtonScript.canPush)
        {
            if (buttonNumber == numberButtonScript.correctNumber)
            {
                //ゲームクリア
                GameManager.Clear();
                numberButtonScript.stageClear = true;
            }
            //押せなくする
            numberButtonScript.canPush = false;
        }
    }
}
