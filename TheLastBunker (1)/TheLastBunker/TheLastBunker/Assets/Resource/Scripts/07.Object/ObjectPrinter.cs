using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPrinter : Object
{
    public int EventNum;
    public override void ActiveObj()
    {
        if(EventNum > 0)
        {
            EventControl.Instance.StartEvent(EventNum);
        }
        PlayerAniControl.Instance.SpineSelect("총장비");
    }
}
