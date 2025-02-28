using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuButtonList : ButtonListBase
{
    protected override void Start()
    {
        base.Start();       
        for (int i = 0; i < size; i++)
        {
            onClick[i] = () => DebugNotificationGenerator.Notify(i.ToString);
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
