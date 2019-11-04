using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DIRECTION
{
    NORTH,
    SOUTH,
    EAST,
    WEST
}

public enum RoomMoveTerms
{
    MONSTER_DESTROY,
    OBJECT_ACTIVE,
    SECRET_OBJ_ACTIVE,
    FREE,

}

public class Room : MonoBehaviour
{
    public List<Tile> AreaTiles;
    public Door LeftDoor;
    public Door RightDoor;
    public Door UpDoor;
    public Door DownDoor;
    public string AreaName;
    public int x;
    public int y;

    public Room n;
    public Room s;
    public Room e;
    public Room w;

    //방안의 모든 적캐릭터
    public List<Enermy> enermies;

    //방안의 모든 오브젝트
    public List<Object> objs;

    //룸이동 가능 여부
    public bool CanMoveRoom;

    //일반레버가당겨졌는지 안당겨졌는지 여부 없을경우 무조건 false
    public bool RabberDown;

    //룸이동 가능 여부가 오브젝트일경우 필요 체크 오브젝트
    public Object targetObj;

    //각방이 가지고있는 방이 움직이는 것이 오픈될 조건
    public RoomMoveTerms roommoveterm;

    //실질이미지 
    public GameObject myImage;

    //이룸의 메쉬 렌더러
    public SpriteRenderer spriterender;

    //룸고유의 이벤트가 존재하는지 체크
    public int Event;

    //룸에서 사용되는 레이저들
    public List<LaserControler> LaserControler;

    public void Start()
    {
        RabberDown = false;
        spriterender = myImage.GetComponent<SpriteRenderer>();
    }

    public void MoveRoom(DIRECTION direction)
    {
        PlayInformation.Instance.UsingDoor = true;
        JoystickControler.Instance.OnEndDragForMain();
        JoystickControler.Instance.OnEndDragForStick();
        if(PlayInformation.Instance.GameActive == true)
        {
            PlayInformation.Instance.GameActive = false;
            switch (direction)
            {
                case DIRECTION.NORTH:
                    Enermy_Obj_ResetTransForm();
                    PlayInformation.Instance.PlayingRoom.Event = 0;
                    PlayInformation.Instance.PlayingRoom = n;
                    PlayInformation.Instance.spriterender = n.spriterender;
                    StartCoroutine(PlayerMoving(DIRECTION.NORTH));
                    //PlayInformation.Instance.PlayingRoom.eventcheck();
                    break;
                case DIRECTION.SOUTH:
                    Enermy_Obj_ResetTransForm();
                    PlayInformation.Instance.PlayingRoom.Event = 0;
                    PlayInformation.Instance.PlayingRoom = s;
                    PlayInformation.Instance.spriterender = s.spriterender;
                    StartCoroutine(PlayerMoving(DIRECTION.SOUTH));
                    //PlayInformation.Instance.PlayingRoom.eventcheck();
                    break;
                case DIRECTION.EAST:
                    Enermy_Obj_ResetTransForm();
                    PlayInformation.Instance.PlayingRoom.Event = 0;
                    PlayInformation.Instance.PlayingRoom = e;
                    PlayInformation.Instance.spriterender = e.spriterender;
                    StartCoroutine(PlayerMoving(DIRECTION.EAST));
                    //PlayInformation.Instance.PlayingRoom.eventcheck();
                    break;
                case DIRECTION.WEST:
                    Enermy_Obj_ResetTransForm();
                    PlayInformation.Instance.PlayingRoom.Event = 0;
                    PlayInformation.Instance.PlayingRoom = w;
                    PlayInformation.Instance.spriterender = w.spriterender;
                    StartCoroutine(PlayerMoving(DIRECTION.WEST));
                    //PlayInformation.Instance.PlayingRoom.eventcheck();
                    break;
            }
        }
        //레이저가 꺼진상태인지 켜진상태인지 체크후 상호작용하는 코드
        int count = 0;
        for (int i = 0; i < PlayInformation.Instance.PlayingRoom.objs.Count; i++)
        {
            if(PlayInformation.Instance.PlayingRoom.objs[i] != null)
            {
                if (PlayInformation.Instance.PlayingRoom.objs[i].name == "레버(Clone)")
                {
                    if (PlayInformation.Instance.PlayingRoom.objs[i].GetComponent<Rabber>().LaserActive == false)
                    {
                        for (int ii = 0; ii < PlayInformation.Instance.PlayingRoom.LaserControler.Count; ii++)
                        {
                            if (PlayInformation.Instance.PlayingRoom.LaserControler[ii] != null && count < 2)
                            {
                                PlayInformation.Instance.PlayingRoom.LaserControler[ii].GetComponent<ParticleSystem>().Stop();
                                count++;
                            }
                        }
                    }
                }
            }
        }
        //
    }

    private void Enermy_Obj_ResetTransForm()
    {
        for(int i = 0; i< PlayInformation.Instance.PlayingRoom.enermies.Count; i++)
        {
            if(PlayInformation.Instance.PlayingRoom.enermies[i] != null)
            {
                PlayInformation.Instance.PlayingRoom.enermies[i].GoMyTile();
            }
        }
        for (int i = 0; i < PlayInformation.Instance.PlayingRoom.objs.Count; i++)
        {
            if(PlayInformation.Instance.PlayingRoom.objs[i] != null)
            {
                PlayInformation.Instance.PlayingRoom.objs[i].GoMyTile();
            }
        }
    }


    IEnumerator PlayerMoving(DIRECTION direction)
    {
        PlayInformation.Instance.PlayingRoom.gameObject.SetActive(true);
        var a = PlayInformation.Instance.PlayingRoom;
        if (direction == DIRECTION.EAST)
        {
            Player.Instance.PlayerTargetTransform = new Vector3(a.LeftDoor.transform.position.x + 15, a.LeftDoor.transform.position.y - 5, a.LeftDoor.transform.position.z);
            PlayInformation.Instance.playstate = PlayState.PLAY_STATE_ROOMMOVE_EAST;
        }
        else if(direction == DIRECTION.WEST)
        {
            Player.Instance.PlayerTargetTransform = new Vector3(a.RightDoor.transform.position.x - 15, a.RightDoor.transform.position.y - 5, a.RightDoor.transform.position.z);
            PlayInformation.Instance.playstate = PlayState.PLAY_STATE_ROOMMOVE_WEST;
        }
        else if (direction == DIRECTION.SOUTH)
        {
            Player.Instance.PlayerTargetTransform = new Vector3(a.UpDoor.transform.position.x, a.UpDoor.transform.position.y - 10, a.UpDoor.transform.position.z);
            PlayInformation.Instance.playstate = PlayState.PLAY_STATE_ROOMMOVE_EAST;
        }
        else if (direction == DIRECTION.NORTH)
        {
            Player.Instance.PlayerTargetTransform = new Vector3(a.DownDoor.transform.position.x, a.DownDoor.transform.position.y + 10, a.DownDoor.transform.position.z);
            PlayInformation.Instance.playstate = PlayState.PLAY_STATE_ROOMMOVE_EAST;
        }
        yield return new WaitForSeconds(2.0f);
        PlayInformation.Instance.UsingDoor = false;
        Player.Instance.MyRoom = PlayInformation.Instance.PlayingRoom;
        Player.Instance.transform.parent = PlayInformation.Instance.transform;
        PlayInformation.Instance.ResetRooms();
        PlayInformation.Instance.GameActive = true;
        PlayInformation.Instance.playstate = PlayState.PLAY_STATE_PLAY;
        PlayInformation.Instance.PlayingRoom.eventcheck();
    }

    public void eventcheck()
    {
        if(Event > 0)
        {
            EventControl.Instance.StartEvent(PlayInformation.Instance.PlayingRoom.Event);
        }
        else
        {
            PlayInformation.Instance.GameActive = true;
        }
    }
}