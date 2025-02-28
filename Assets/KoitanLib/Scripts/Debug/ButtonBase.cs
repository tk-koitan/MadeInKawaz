using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(RectTransform))]
public class ButtonBase : MonoBehaviour
{
    private Vector3 mousePos;
    private Vector3 mousePosOnCanvas;
    protected RectTransform rectTransform;
    protected Action onClick;
    protected Action onTouchEnter;
    protected Action onTouchStay;
    protected Action onTouchExit;
    private bool isTouching;

    // Start is called before the first frame update
    virtual protected void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        mousePos = Input.mousePosition;
        mousePosOnCanvas = mousePos * 1920 / Screen.width;
        float scale = Screen.width / 1920f;
        Vector2 pos = rectTransform.position;
        Rect rect = rectTransform.rect;      
        if (mousePos.x > pos.x - rect.width / 2 * scale && mousePos.x < pos.x + rect.width / 2 * scale && mousePos.y > pos.y - rect.height / 2 * scale && mousePos.y < pos.y + rect.height / 2 * scale)
        {
            if (isTouching)
            {
                onTouchStay?.Invoke();
            }
            else
            {
                onTouchEnter?.Invoke();
            }

            if (Input.GetMouseButtonDown(0))
            {
                onClick?.Invoke();
            }

            isTouching = true;
        }
        else
        {
            if (isTouching)
            {
                onTouchExit?.Invoke();
            }

            isTouching = false;
        }
    }
}