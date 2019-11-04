using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyDoor : Object
{
    public bool Opendoor;
    public Sprite OpendoorSprite;
    public Sprite LockdoorSprite;
    public int EventNum;
    public override void ActiveObj()
    {
        if(EventNum != 0)
        {
            EventControl.Instance.StartEvent(EventNum);
        }
        if(Opendoor == true)
        {
            Opendoor = false;
            this.GetComponent<SpriteRenderer>().sprite = LockdoorSprite;
            this.gameObject.layer = 13;
            this.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            Opendoor = true;
            this.GetComponent<SpriteRenderer>().sprite = OpendoorSprite;
            this.gameObject.layer = 17;
            this.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
