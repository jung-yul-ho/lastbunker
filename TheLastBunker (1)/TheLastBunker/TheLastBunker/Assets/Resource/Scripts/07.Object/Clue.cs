using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClueType
{
    CLUE_DESK,
    CLUE_PAPER
}

public class Clue : Object
{
    public int EventNum;
    public ClueType cluetype;
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
        //이벤트넘버가 맞을경우 모드오픈
        if(EventNum == 11)
        {
            PlayInformation.Instance.ModOpen = true;
        }
    }
}