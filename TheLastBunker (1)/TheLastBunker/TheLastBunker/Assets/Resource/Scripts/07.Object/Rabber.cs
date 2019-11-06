using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabber : Object
{
    public bool LaserActive;
    public int LaserNumber;
    public void Start()
    {
        //LaserActive = true;
    }
    public void settingLaser()
    {
        if(LaserActive == false)
        {
            for(int i = 0; i< PlayInformation.Instance.PlayingRoom.LaserControler.Count; i++)
            {
                PlayInformation.Instance.PlayingRoom.LaserControler[i].GetComponent<ParticleSystem>().Stop();
            }
        }
        else
        {
            for (int i = 0; i < PlayInformation.Instance.PlayingRoom.LaserControler.Count; i++)
            {
                PlayInformation.Instance.PlayingRoom.LaserControler[i].GetComponent<ParticleSystem>().Play();
            }
        }
    }
    public override void ActiveObj()
    {
        if(LaserActive == true)
        {
            LaserActive = false;
        }
        else
        {
            LaserActive = true;
        }
        for (int i = 0; i< PlayInformation.Instance.PlayingRoom.LaserControler.Count; i++)
        {
            if(PlayInformation.Instance.PlayingRoom.LaserControler[i] != null && PlayInformation.Instance.PlayingRoom.LaserControler[i].ControlerNumber == LaserNumber)
            {
                if (PlayInformation.Instance.PlayingRoom.LaserControler[i].GetComponent<ParticleSystem>().isPlaying == true)
                {
                    this.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.ActiveLaserRabber;
                    PlayInformation.Instance.PlayingRoom.LaserControler[i].GetComponent<ParticleSystem>().Stop();
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.NoActiveLaserRabber;
                    PlayInformation.Instance.PlayingRoom.LaserControler[i].GetComponent<ParticleSystem>().Play();
                }
            }
        }
        //이전버전
        //ControlerRoom.RabberDown = true;
        //if (ControlerRoom.roommoveterm == RoomMoveTerms.OBJECT_ACTIVE)
        //{
        //    ControlerRoom.CanMoveRoom = true;
        //    PlayInformation.Instance.StageClearCheck();
        //}
        //else if(ControlerRoom.roommoveterm == RoomMoveTerms.SECRET_OBJ_ACTIVE)
        //{
        //    for(int i = 0; i< ControlerRoom.objs.Count; i++)
        //    {
        //        if(ControlerRoom.objs[i] != null)
        //        {
        //            if(ControlerRoom.objs[i].gameObject.tag == "Secretrabber")
        //            {
        //                ControlerRoom.objs[i].gameObject.GetComponent<SecretSwitch>().ActiveObj();
        //                break;
        //            }
        //        }
        //    }
        //}
    }
}
