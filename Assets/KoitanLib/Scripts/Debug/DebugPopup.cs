using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class DebugPopup : ButtonBase
{
    public OpenState openState { get; private set; }    
    public Vector2 size { get; }
    public Func<string> message { get; private set; }
    private TextMeshProUGUI text;
    Sequence seq = DOTween.Sequence();

    public DebugPopup(Func<string> m)
    {
        message = m;
    }

    public void SetPopup(Func<string> m)
    {
        message = m;
        openState = OpenState.Opening;
        gameObject.SetActive(true);
        seq = DOTween.Sequence()
        .Append(rectTransform.DOSizeDelta(new Vector2(800, 600), 0.2f))        
        .OnComplete(() =>
        {
            text.gameObject.SetActive(true);
            openState = OpenState.Opened;
        });
    }

    protected override void Start()
    {
        base.Start();
        openState = OpenState.Closed;
        rectTransform.sizeDelta = new Vector2(0, 0);
        text = GetComponentInChildren<TextMeshProUGUI>();        
        text.gameObject.SetActive(false);        

        onClick = () =>
        {
            if(openState == OpenState.Opened)
            {
                seq.Kill();
                text.gameObject.SetActive(false);
                openState = OpenState.Closing;
                seq = DOTween.Sequence()
                .Append(rectTransform.DOSizeDelta(new Vector2(0, 0), 0.2f))
                .OnComplete(() =>
                {
                    openState = OpenState.Closed;
                    gameObject.SetActive(false);
                });
            }            
        };
    }

    protected override void Update()
    {
        base.Update();
        if (openState == OpenState.Opened)
        {
            text.text = message();
        }
    }

    public enum OpenState
    {
        Closed,
        Opening,
        Opened,
        Closing
    }
}
