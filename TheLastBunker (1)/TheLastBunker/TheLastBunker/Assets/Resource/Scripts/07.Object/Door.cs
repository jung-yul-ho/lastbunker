using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Doortype
{
    Up,
    Down,
    Left,
    Right
}

public class Door : Object
{
    public Doortype doortype;
    public bool CanUsingThis;

    public override void ActiveObj()
    {
        if (/*PlayInformation.Instance.PlayingRoom.CanMoveRoom*/CanUsingThis == true)
        {
            if (ControlerRoom.LeftDoor == this)
            {
                PlayInformation.Instance.PlayingRoom.MoveRoom(DIRECTION.WEST);
            }
            else if (ControlerRoom.RightDoor == this)
            {
                PlayInformation.Instance.PlayingRoom.MoveRoom(DIRECTION.EAST);
            }
            else if (ControlerRoom.UpDoor == this)
            {
                PlayInformation.Instance.PlayingRoom.MoveRoom(DIRECTION.NORTH);
            }
            else if (ControlerRoom.DownDoor == this)
            {
                PlayInformation.Instance.PlayingRoom.MoveRoom(DIRECTION.SOUTH);
            }
        }
    }
}
