using Spine.Unity;
using Spine.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[System.Serializable]
public enum PlayState
{
    PLAY_STATE_TITLE,
    PLAY_STATE_STAGE,
    PLAY_STATE_PLAY,
    PLAY_STATE_ROOMMOVE_EAST,
    PLAY_STATE_ROOMMOVE_WEST,
}

public class PlayInformation : MonoBehaviour
{
    public bool UsingDoor;
    public bool GameActive;
    public bool GameOver;
    public int StageNum;
    public int SecurityLevel;
    public int RoomLevel;
    public PlayState playstate;
    public Room PlayingRoom;
    public bool ModOpen;

    //점점흐려지게하기
    public SpriteRenderer spriterender;
    float color;

    static PlayInformation instance;
    public static PlayInformation Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayInformation>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<PlayInformation>();
                }
            }
            return instance;
        }
    }

    public void Start()
    {
        UsingDoor = false;
        color = 1;
        GameOver = false;
        GameActive = false;
    }

    //플레이하지 않는 룸들을 전부 꺼버리는 코드
    public void ResetRooms()
    {
        for(int i = 0; i< RoomControler.Instance.InGameArea.Count; i++)
        {
            if(RoomControler.Instance.InGameArea[i] != PlayingRoom)
            {
                RoomControler.Instance.InGameArea[i].gameObject.SetActive(false);
            }
            else
            {
                RoomControler.Instance.InGameArea[i].gameObject.SetActive(true);
            }
        }

        spriterender = PlayingRoom.spriterender;
    }

    public void StageClearCheck()
    {
        bool check = true;
        for (int i = 0; i < RoomControler.Instance.InGameArea.Count; i++)
        {
            if(RoomControler.Instance.InGameArea[i].CanMoveRoom == false)
            {
                check = false;
            }
        }
        if(check == true)
        {
            if(RoomLevel > 4)
            {
                SecurityLevel++;
                RoomLevel = 1;
            }
            else
            {
                RoomLevel++;
            }
            StartCoroutine(Victory());
            MessageBox.Instance.ShowMessageLong("Stage Clear");
        }
    }

    IEnumerator Victory()
    {
        if (spriterender == null)
        {
            spriterender = PlayingRoom.myImage.GetComponent<SpriteRenderer>();
        }
        GameOver = true;
        MessageBox.Instance.GetComponent<Text>().color = new Color(0,0,0);
        float Yellow = 0.0f;
        while (Yellow < 1.0f)
        {
            Color Yellowcolor = new Color(Yellow, Yellow, 0);
            Color newcolor = new Color(color, color, color);
            spriterender.color = newcolor;
            for (int i = 0; i < PlayingRoom.enermies.Count; i++)
            {
                PlayingRoom.enermies[i].anicontrol.skeletonAnimation.skeleton.SetColor(newcolor);
            }
            for (int i = 0; i < PlayingRoom.objs.Count; i++)
            {
                if(PlayingRoom.objs[i] != null)
                {
                    PlayingRoom.objs[i].GetComponent<SpriteRenderer>().color = newcolor;
                }
            }
            PlayerControl.Instance.AttackButton.GetComponent<Image>().color = newcolor;
            PlayerControl.Instance.SpecialOne.GetComponent<Image>().color = newcolor;
            PlayerControl.Instance.SpecialTwo.GetComponent<Image>().color = newcolor;
            PlayerControl.Instance.HpImage.GetComponent<Image>().color = newcolor;
            PlayerControl.Instance.MenuButton.GetComponent<Image>().color = newcolor;
            JoystickControler.Instance.gameObject.GetComponent<Image>().color = newcolor;
            JoystickControler.Instance.Stick.gameObject.GetComponent<Image>().color = newcolor;
            Player.Instance.anicontrol.skeletonAnimation.skeleton.SetColor(newcolor);
            MessageBox.Instance.GetComponent<Text>().color = Yellowcolor;
            Yellow += 0.05f;
            if (color > 0.35f)
            {
                color -= 0.05f;
            }
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.0f);
        PlayerControl.Instance.EXIT();
    }

    public IEnumerator Lose()
    {
        if (spriterender == null)
        {
            spriterender = PlayingRoom.myImage.GetComponent<SpriteRenderer>();
        }
        GameOver = true;
        float red = 0.0f;
        while (red < 1.0f)
        {
            Color redcolor = new Color(red, 0, 0);
            Color newcolor = new Color(color, color, color);
            spriterender.color = newcolor;
            for (int i = 0; i < PlayingRoom.enermies.Count; i++)
            {
                PlayingRoom.enermies[i].anicontrol.skeletonAnimation.skeleton.SetColor(newcolor);
            }
            for (int i = 0; i < PlayingRoom.objs.Count; i++)
            {
                PlayingRoom.objs[i].GetComponent<SpriteRenderer>().color = newcolor;
            }
            PlayerControl.Instance.AttackButton.GetComponent<Image>().color = newcolor;
            PlayerControl.Instance.SpecialOne.GetComponent<Image>().color = newcolor;
            PlayerControl.Instance.SpecialTwo.GetComponent<Image>().color = newcolor;
            PlayerControl.Instance.HpImage.GetComponent<Image>().color = newcolor;
            PlayerControl.Instance.MenuButton.GetComponent<Image>().color = newcolor;
            JoystickControler.Instance.gameObject.GetComponent<Image>().color = newcolor;
            JoystickControler.Instance.Stick.gameObject.GetComponent<Image>().color = newcolor;
            Player.Instance.anicontrol.skeletonAnimation.skeleton.SetColor(newcolor);
            MessageBox.Instance.GetComponent<Text>().color = redcolor;
            red += 0.05f;
            if(color > 0.35f)
            {
                color -= 0.05f;
            }

            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2.0f);
        PlayerControl.Instance.EXIT();
    }
}
