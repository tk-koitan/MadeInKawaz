using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class MenuButton : MonoBehaviour
{
    private Vector3 mousePos;
    private Vector3 mousePosOnCanvas;
    private RectTransform rectTransform;
    private RawImage rawImage;
    private Action onClick;
    private Action onTouchEnter;
    private Action onTouchStay;
    private Action onTouchExit;
    private bool isTouching;

    // Start is called before the first frame update
    void Start()
    {
        DebugTextManager.Display(() => "MousePos(Screen):" + Input.mousePosition.ToString() + "\n");
        DebugTextManager.Display(() => "MousePos(Canvas):" + (Input.mousePosition * 1920 / Screen.width).ToString() + "\n");
        DebugTextManager.Display(() => "Position(World):" + rectTransform.position.ToString() + "\n");
        onClick = () => { transform.localScale = Vector3.one; transform.DOPunchScale(Vector3.one, 1); };
        onTouchEnter = () => rawImage.color = Color.red;
        onTouchStay = () => rawImage.color = Color.blue;
        onTouchExit = () => rawImage.color = Color.white;
        rectTransform = GetComponent<RectTransform>();
        rawImage = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePosOnCanvas = mousePos * 1920 / Screen.width;
        float scale = Screen.width / 1920f;
        Vector2 pos = rectTransform.position;
        Rect rect = rectTransform.rect; 
        //if (mousePosOnCanvas.x > pos.x && mousePosOnCanvas.x < pos.x + rect.width && mousePosOnCanvas.y > pos.y && mousePosOnCanvas.y < pos.y + rect.height)        
        if (mousePos.x > pos.x - rect.width / 2 * scale && mousePos.x < pos.x + rect.width / 2 * scale && mousePos.y > pos.y - rect.height / 2 * scale && mousePos.y < pos.y + rect.height / 2 * scale)
        {
            if (isTouching)
            {
                onTouchStay();
            }
            else
            {
                onTouchEnter();
            }

            if (Input.GetMouseButtonDown(0))
            {
                onClick();
            }

            isTouching = true;
        }
        else
        {
            if (isTouching)
            {
                onTouchExit();
            }

            isTouching = false;
        }
    }
}
