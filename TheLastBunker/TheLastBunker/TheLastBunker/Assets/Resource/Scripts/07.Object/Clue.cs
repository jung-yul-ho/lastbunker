using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : Object
{
    public int EventNum;
    public override void ActiveObj()
    {
        if(EventNum != 0)
        {
            EventControl.Instance.StartEvent(EventNum);
        }
        if(PlayInformation.Instance.PlayingRoom.roommoveterm == RoomMoveTerms.OBJECT_ACTIVE)
        {
            PlayInformation.Instance.PlayingRoom.CanMoveRoom = true;
        }
    }
}