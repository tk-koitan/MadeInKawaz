using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using KoitanLib;

[RequireComponent(typeof(RectTransform))]
public class ButtonListBase : MonoBehaviour
{
    private Vector3 mousePos;
    protected RectTransform rectTransform;
    protected Action[] onClick;
    protected Action[] onTouchEnter;
    protected Action[] onTouchStay;
    protected Action[] onTouchExit;
    private bool isTouching;
    int oldIndex = -1;
    int index = -1;
    [SerializeField]
    protected int size;

    protected void SetList(int s)
    {        
        onClick = new Action[s];
        onTouchEnter = new Action[s];
        onTouchStay = new Action[s];
        onTouchExit = new Action[s];
    }

    // Start is called before the first frame update
    virtual protected void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        SetList(size);
        DebugTextManager.Display(() => "index:" + index.ToString());
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        mousePos = Input.mousePosition;
        float scale = Screen.width / 1920f;
        Vector2 center = rectTransform.position;
        Rect rect = rectTransform.rect;
        Vector2 topLeft = center;
        index = GetListIndex(mousePos, topLeft, rect.width * scale, rect.height * scale, size);
        if (index != -1)
        {
            if (oldIndex == index)
            {
                onTouchStay[index]?.Invoke();
            }
            else
            {
                onTouchEnter[index]?.Invoke();
                if (oldIndex != -1) onTouchExit[oldIndex]?.Invoke();
            }

            if (Input.GetMouseButtonDown(0))
            {
                onClick[index]?.Invoke();
            }
        }
        else if (oldIndex != -1) onTouchExit[oldIndex]?.Invoke();
        oldIndex = index;
    }

    private int GetListIndex(Vector3 pos, Vector3 topLeft, float width, float height, int s)
    {
        int i;
        if (pos.x > topLeft.x && pos.x < topLeft.x + width)
        {
            i = Mathf.FloorToInt((topLeft.y - pos.y) / (height / s));
            if (i >= 0 && i < s)
            {
                return i;
            }
        }
        return -1;
    }
}