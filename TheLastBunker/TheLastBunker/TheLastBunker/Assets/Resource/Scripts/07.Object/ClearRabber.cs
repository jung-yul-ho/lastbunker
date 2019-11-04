using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearRabber : Object
{
    public override void ActiveObj()
    {
        //윗문체크
        if (PlayInformation.Instance.PlayingRoom.UpDoor != null)
        {
            if (PlayInformation.Instance.PlayingRoom.UpDoor.CanUsingThis == false)
            {
                StartCoroutine(ActiveRabber((PlayInformation.Instance.PlayingRoom.UpDoor.gameObject)));
            }
        }
        //아랫문체크
        if (PlayInformation.Instance.PlayingRoom.DownDoor != null)
        {
            if (PlayInformation.Instance.PlayingRoom.DownDoor.CanUsingThis == false)
            {
                StartCoroutine(ActiveRabber((PlayInformation.Instance.PlayingRoom.DownDoor.gameObject)));
            }
        }
        //오른문체크
        if (PlayInformation.Instance.PlayingRoom.RightDoor != null)
        {
            if (PlayInformation.Instance.PlayingRoom.RightDoor.CanUsingThis == false)
            {
                StartCoroutine(ActiveRabber((PlayInformation.Instance.PlayingRoom.RightDoor.gameObject)));
            }
        }
        //왼문체크
        if (PlayInformation.Instance.PlayingRoom.LeftDoor != null)
        {
            if (PlayInformation.Instance.PlayingRoom.LeftDoor.CanUsingThis == false)
            {
                StartCoroutine(ActiveRabber((PlayInformation.Instance.PlayingRoom.LeftDoor.gameObject)));
            }
        }
        this.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.ActiveRabber;
    }

    public IEnumerator ActiveRabber(GameObject target)
    {
        Player.Instance.PlayerTargetTransform = Player.Instance.transform.position;
        EffectViewCam.Instance.TargetView = target;
        AreaViewCam.Instance.TargetView = target;
        FollowCam.Instance.TargetView = target;
        PlayInformation.Instance.GameActive = false;
        PlayerControl.Instance.playerstate = PlayerState.PLAYER_VIEWAREA;
        PlayInformation.Instance.PlayingRoom.CanMoveRoom = true;
        yield return new WaitForSeconds(3.0f);
        target.GetComponent<Door>().CanUsingThis = true;
        PlayInformation.Instance.GameActive = true;
        PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
    }
}
