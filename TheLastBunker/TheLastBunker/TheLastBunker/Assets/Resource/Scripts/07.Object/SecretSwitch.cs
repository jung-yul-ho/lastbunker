using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretSwitch : Object
{
    public override void ActiveObj()
    {
        ControlerRoom.CanMoveRoom = true;
        PlayInformation.Instance.StageClearCheck();
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
        //if (CanActive == true)
        //{
        //    ControlerRoom.CanMoveRoom = true;
        //    PlayInformation.Instance.StageClearCheck();
        //    MessageBox.Instance.ShowMessage("장치가 작동한다");
        //}
        //else if(ControlerRoom.RabberDown == false)
        //{
        //    MessageBox.Instance.ShowMessage("뭔가 부족한듯 하다. 레버를 찾아서 당겨보자");
        //}
        //else if(CanActive == false)
        //{
        //    MessageBox.Instance.ShowMessage("어디엔가 비밀 스위치가 있는듯하다");
        //}
    }

    public IEnumerator ActiveRabber(GameObject target)
    {
        Player.Instance.PlayerTargetTransform = Player.Instance.transform.position;
        EffectViewCam.Instance.TargetView = target;
        AreaViewCam.Instance.TargetView = target;
        FollowCam.Instance.TargetView = target;
        PlayInformation.Instance.GameActive = false;
        PlayerControl.Instance.playerstate = PlayerState.PLAYER_VIEWAREA;
        yield return new WaitForSeconds(5.0f);
        target.GetComponent<Door>().CanUsingThis = true;
        PlayInformation.Instance.GameActive = true;
        PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
    }

    public void ReView()
    {
        this.GetComponent<SpriteRenderer>().enabled = true;
    }
}
