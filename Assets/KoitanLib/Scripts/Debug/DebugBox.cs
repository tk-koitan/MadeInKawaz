using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class DebugBox : ButtonBase
{
    private OpenState openState = OpenState.Closed;
    public Transform parent { get; private set; }
    public Vector2 size { get; private set; } = new Vector2(600, 400);
    public Func<string> message { get; private set; }
    private TextMeshProUGUI text;
    private Image image;
    private bool isAlwaysOpen = false;
    private Vector3 offset;
    Sequence seq = DOTween.Sequence();
    private GameObject parentObj;

    public DebugBox(Func<string> m, Transform t)
    {
        message = m;
        parent = t;
    }

    public DebugBox SetBox(Func<string> m, Transform t)
    {
        message = m;
        parent = t;
        text = GetComponentInChildren<TextMeshProUGUI>();        
        return this;
    }    

    public DebugBox SetSize(Vector2 s)
    {
        size = s;        
        return this;
    }

    public DebugBox SetSize(float width, float height)
    {
        return SetSize(new Vector2(width, height));
    }

    public DebugBox SetOffset(Vector2 o)
    {
        offset = o;
        return this;
    }

    public DebugBox SetAlignment(TextAlignmentOptions o)
    {
        text.alignment = o;
        return this;
    }

    public DebugBox SetDefaultOpen()
    {        
        return this;
    }

    protected override void Start()
    {
        base.Start();
        parentObj = parent.gameObject;        
        //offset = new Vector3(0, -100);
        rectTransform.sizeDelta = new Vector2(100, 100);
        text = GetComponentInChildren<TextMeshProUGUI>();
        text.rectTransform.sizeDelta = size;
        image = GetComponent<Image>();
        text.gameObject.SetActive(false);
        onTouchEnter = () =>
        {
            if (!isAlwaysOpen)
            {
                seq.Kill();
                openState = OpenState.Opening;
                seq = DOTween.Sequence()
                .Append(rectTransform.DOSizeDelta(size, 0.2f))
                //.Join(DOTween.To(() => offset, (t) => offset = t, new Vector3(0, -250), 0.2f))
                .OnComplete(() =>
                {
                    text.gameObject.SetActive(true);
                    openState = OpenState.Opened;
                });
            }
        };

        onTouchExit = () =>
        {
            if (!isAlwaysOpen)
            {
                seq.Kill();
                text.gameObject.SetActive(false);
                openState = OpenState.Closing;
                seq = DOTween.Sequence()
                .Append(rectTransform.DOSizeDelta(new Vector2(100, 100), 0.2f))
                //.Join(DOTween.To(() => offset, (t) => offset = t, new Vector3(0, -100), 0.2f))
                .OnComplete(() =>
                {
                    openState = OpenState.Closed;
                });
            }
        };

        onClick = () =>
        {
            if (openState == OpenState.Opened)
            {
                seq.Kill();
                rectTransform.localScale = Vector3.one;
                isAlwaysOpen = !isAlwaysOpen;
                seq = DOTween.Sequence()
                .Append(rectTransform.DOPunchScale(Vector3.one * 0.1f, 0.2f))
                .Join(image.DOColor(isAlwaysOpen ? Color.black : Color.white, 0.2f));
            }
        };
    }

    protected override void Update()
    {
        base.Update();
        if (parent == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 pos = Camera.main.WorldToScreenPoint(parent.position);
        transform.position = pos;
        //rectTransform.transform.localPosition += offset;
        if (openState == OpenState.Opened)
        {
            text.text = message();
        }
    }

    enum OpenState
    {
        Closed,
        Opening,
        Opened,
        Closing
    }
}
