using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearRabber : Object
{
    public override void ActiveObj()
    {
        CanActive = true;
        this.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.ActiveRoomRabber;
        bool pass = true;
        for(int i = 0; i< PlayInformation.Instance.PlayingRoom.clearrabeers.Count; i++)
        {
            if(PlayInformation.Instance.PlayingRoom.clearrabeers[i].CanActive == false)
            {
                pass = false;
            }
        }
        if(pass == true)
        {
            //윗문체크
            if (PlayInformation.Instance.PlayingRoom.UpDoor != null)
            {
                if (PlayInformation.Instance.PlayingRoom.UpDoor.CanUsingThis == false)
                {
                    StartCoroutine(ActiveRabber((PlayInformation.Instance.PlayingRoom.UpDoor.gameObject)));
                    PlayInformation.Instance.PlayingRoom.n.DownDoor.CanActive = true;
                    PlayInformation.Instance.PlayingRoom.n.DownDoor.CanUsingThis = true;
                }
            }
            //아랫문체크
            if (PlayInformation.Instance.PlayingRoom.DownDoor != null)
            {
                if (PlayInformation.Instance.PlayingRoom.DownDoor.CanUsingThis == false)
                {
                    StartCoroutine(ActiveRabber((PlayInformation.Instance.PlayingRoom.DownDoor.gameObject)));
                    PlayInformation.Instance.PlayingRoom.s.UpDoor.CanActive = true;
                    PlayInformation.Instance.PlayingRoom.s.UpDoor.CanUsingThis = true;
                }
            }
            //오른문체크
            if (PlayInformation.Instance.PlayingRoom.RightDoor != null)
            {
                if (PlayInformation.Instance.PlayingRoom.RightDoor.CanUsingThis == false)
                {
                    StartCoroutine(ActiveRabber((PlayInformation.Instance.PlayingRoom.RightDoor.gameObject)));
                    PlayInformation.Instance.PlayingRoom.e.LeftDoor.CanActive = true;
                    PlayInformation.Instance.PlayingRoom.e.LeftDoor.CanUsingThis = true;
                }
            }
            //왼문체크
            if (PlayInformation.Instance.PlayingRoom.LeftDoor != null)
            {
                if (PlayInformation.Instance.PlayingRoom.LeftDoor.CanUsingThis == false)
                {
                    StartCoroutine(ActiveRabber((PlayInformation.Instance.PlayingRoom.LeftDoor.gameObject)));
                    PlayInformation.Instance.PlayingRoom.w.LeftDoor.CanActive = true;
                    PlayInformation.Instance.PlayingRoom.w.LeftDoor.CanUsingThis = true;
                }
            }
        }
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
        target.GetComponent<Door>().CanActive = true;
        if(target.GetComponent<Door>().doortype == Doortype.Down)
        {
            target.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.DowndoorOpenSprite;
            PlayInformation.Instance.PlayingRoom.s.UpDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.UpdoorOpenSprite;
        }
        else if(target.GetComponent<Door>().doortype == Doortype.Up)
        {
            target.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.UpdoorOpenSprite;
            PlayInformation.Instance.PlayingRoom.n.DownDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.DowndoorOpenSprite;
        }
        else if (target.GetComponent<Door>().doortype == Doortype.Left)
        {
            target.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.LeftdoorOpenSprite;
            PlayInformation.Instance.PlayingRoom.w.RightDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.RightdoorOpenSprite;
        }
        else if (target.GetComponent<Door>().doortype == Doortype.Right)
        {
            target.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.RightdoorOpenSprite;
            PlayInformation.Instance.PlayingRoom.e.LeftDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.LeftdoorOpenSprite;
        }
        PlayInformation.Instance.GameActive = true;
        PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
    }
}
