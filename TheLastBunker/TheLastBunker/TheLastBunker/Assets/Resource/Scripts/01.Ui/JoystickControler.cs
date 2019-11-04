using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickControler : MonoBehaviour
{
    public Transform Stick;
    //조이스틱커서
    public Vector3 StickFierstpos;

    public Vector3 JoyVec;
    public float Radius;
    public bool moveflag;
    public BoxCollider2D playercollider;
    //플레이어 캐릭터의 방향전환을 위한 기초 백터값
    public Vector3 BasicRotation;

    //플레이어 무브 체크
    public bool checkmove;

    //플레이어 실질 이동방향
    public Vector2 MoveArea;

    //사물을 밀고있는가를 체크
    public bool push;
    static JoystickControler instance;
    public static JoystickControler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<JoystickControler>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<JoystickControler>();
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.4f;
        StickFierstpos = Stick.transform.position;

        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;

        moveflag = false;

        BasicRotation = new Vector3(0, 0, 0);
        push = false;
    }

    public void Update()
    {
        if (moveflag == true && PlayInformation.Instance.GameOver == false && (PlayerControl.Instance.playerstate == PlayerState.PLAYER_IDLE || PlayerControl.Instance.playerstate == PlayerState.PLAYER_MOVE || PlayerControl.Instance.playerstate == PlayerState.PLAYER_LIFTING) && PlayerControl.Instance.Damaged == false)
        {
            //기초방향전환
            //Vector3 aa = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);
            //Player.Instance.transform.eulerAngles = BasicRotation + aa;
            //플레이어 이동
            if(PlayerControl.Instance.playerstate != PlayerState.PLAYER_DIE)
            {
                push = false;
                MoveArea = JoyVec;
                if (JoyVec.x > 0)
                {
                    PlayerControl.Instance.viewdirection = Viewdirection.VIEW_RIGHT;
                    CanMoveCheckRight();
                }
                else if (JoyVec.x < 0)
                {
                    PlayerControl.Instance.viewdirection = Viewdirection.VIEW_LEFT;
                    CanMoveCheckLeft();
                }
                if (JoyVec.y > 0)
                {
                    CanMoveCheckUp();
                }
                else if (JoyVec.y < 0)
                {
                    CanMoveCheckDown();
                }
                //CharacterData.Instance.CheckSupport.SetActive(false);
                if(PlayInformation.Instance.ModOpen == true && Player.Instance.playermod == PlayerMod.MOD_POWER)
                {
                    Player.Instance.transform.Translate(MoveArea * 0.5f);
                }
                else
                {
                    Player.Instance.transform.Translate(MoveArea * 1.0f);
                }
            }
        }
    }

    public void OnDragForMain(BaseEventData Data)
    {
        if (PlayInformation.Instance.GameActive == true && playercollider && PlayerControl.Instance.playerstate != PlayerState.PLAYER_ATTACK && PlayInformation.Instance.GameOver == false && PlayerControl.Instance.Damaged == false)
        {
            OnDragForStick(Data);
        }
    }

    public void OnEndDragForMain()
    {
        if(PlayerControl.Instance.playerstate != PlayerState.PLAYER_DIE)
        {
            if(PlayerControl.Instance.playerstate != PlayerState.PLAYER_LIFTING)
            {
                PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
            }
            //이동체크를 false
            moveflag = false;
        }
        //무브커서가 원상복귀
        Stick.position = StickFierstpos;
    }

    public void OnDragForStick(BaseEventData Data)
    {
        if (PlayInformation.Instance.GameActive == true && PlayerControl.Instance.playerstate != PlayerState.PLAYER_ATTACK && PlayerControl.Instance.playerstate != PlayerState.PLAYER_DIE && PlayInformation.Instance.GameOver == false && PlayerControl.Instance.Damaged == false)
        {
            if(PlayerControl.Instance.playerstate != PlayerState.PLAYER_LIFTING)
            {
                PlayerControl.Instance.playerstate = PlayerState.PLAYER_MOVE;
            }
            PointerEventData data = Data as PointerEventData;
            Vector3 pos = data.position;
            moveflag = true;
            //조이스틱을 이동시킬 방향을 구함 4등분중 하나
            JoyVec = (pos - StickFierstpos).normalized;
            //조이스틱의 정 중앙에서 현제 터지하고있는곳의 거리를 구한다.
            float dis = Vector3.Distance(pos, StickFierstpos);

            //조이스틱ui조작
            if (dis < Radius)
            {
                Stick.position = StickFierstpos + JoyVec * dis;
            }
            //반지름 초과시의 코드이지만 초과할 일이 거의 없다 
            else
            {
                Stick.position = StickFierstpos + JoyVec * Radius;
            }
        }
    }

    public void OnEndDragForStick()
    {
        if(PlayerControl.Instance.playerstate != PlayerState.PLAYER_DIE)
        {
            PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
        }
        //무브커서가 원상복귀
        Stick.position = StickFierstpos;
        //이동체크를 false
        moveflag = false;
    }

    public void CanMoveCheckUp()
    {
        //CharacterData.Instance.CheckSupport.SetActive(true);
        Vector3 pos = new Vector3(0, 6, 0);
        Vector2 Size = new Vector2(4, 1);
        Vector2 CharacterFront = Player.Instance.transform.position + pos;
        var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject.layer == 12 || Colliders[i].gameObject.layer == 13 || Colliders[i].gameObject.layer == 18)
            {
                MoveArea.y = 0;
                break;
            }
        }
        Size = new Vector2(6, 6);
        //CharacterData.Instance.CheckSupport.transform.parent = Player.Instance.transform;
        //CharacterData.Instance.CheckSupport.transform.position = CharacterFront;
        //CharacterData.Instance.CheckSupport.GetComponent<BoxCollider2D>().size = Size;
        Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject.layer == 13)
            {
                if(Colliders[i].gameObject.tag == "door")
                {
                    Colliders[i].GetComponent<Door>().ActiveObj();
                    break;
                }
                else if(Colliders[i].gameObject.tag != "Wall")
                {
                    PlayerControl.Instance.TargetObj = Colliders[i].gameObject.GetComponent<Object>();
                    PlayerControl.Instance.Movedirection = DIRECTION.NORTH;
                    push = true;
                    break;
                }
            }
            if(push == false)
            {
                PlayerControl.Instance.TargetObj = null;
            }
        }
    }

    public void CanMoveCheckDown()
    {
        //CharacterData.Instance.CheckSupport.SetActive(true);
        Vector3 pos = new Vector3(0, -1, 0);
        Vector2 Size = new Vector2(4, 1);
        Vector2 CharacterFront = Player.Instance.transform.position + pos;
        var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject.layer == 12 || Colliders[i].gameObject.layer == 13 || Colliders[i].gameObject.layer == 18)
            {
                MoveArea.y = 0;
                break;
            }
        }
        Size = new Vector2(6, 6);
        //CharacterData.Instance.CheckSupport.transform.parent = Player.Instance.transform;
        //CharacterData.Instance.CheckSupport.transform.position = CharacterFront;
        //CharacterData.Instance.CheckSupport.GetComponent<BoxCollider2D>().size = Size;
        Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject.layer == 13)
            {
                if (Colliders[i].gameObject.tag == "door")
                {
                    Colliders[i].GetComponent<Door>().ActiveObj();
                    break;
                }
                else if (Colliders[i].gameObject.tag != "Wall")
                {
                    PlayerControl.Instance.TargetObj = Colliders[i].gameObject.GetComponent<Object>();
                    PlayerControl.Instance.Movedirection = DIRECTION.SOUTH;
                    push = true;
                    break;
                }
            }
            if (push == false)
            {
                PlayerControl.Instance.TargetObj = null;
            }
        }
    }

    public void CanMoveCheckRight()
    {
        //CharacterData.Instance.CheckSupport.SetActive(true);
        Vector3 pos = new Vector3(3, 3, 0);
        Vector2 Size = new Vector2(3, 5);
        Vector2 CharacterFront = Player.Instance.transform.position + pos;
        var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject.layer == 12 || Colliders[i].gameObject.layer == 13 || Colliders[i].gameObject.layer == 18)
            {
                MoveArea.x = 0;
                break;
            }
        }
        //CharacterData.Instance.CheckSupport.transform.parent = Player.Instance.transform;
        //CharacterData.Instance.CheckSupport.transform.position = CharacterFront;
        //CharacterData.Instance.CheckSupport.GetComponent<BoxCollider2D>().size = Size;
        Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject.layer == 13)
            {
                if (Colliders[i].gameObject.tag == "door")
                {
                    Colliders[i].GetComponent<Door>().ActiveObj();
                    break;
                }
                else if (Colliders[i].gameObject.tag != "Wall")
                {
                    PlayerControl.Instance.TargetObj = Colliders[i].gameObject.GetComponent<Object>();
                    PlayerControl.Instance.Movedirection = DIRECTION.EAST;
                    push = true;
                    break;
                }
            }
            if (push == false)
            {
                PlayerControl.Instance.TargetObj = null;
            }
        }
    }

    public void CanMoveCheckLeft()
    {
        //CharacterData.Instance.CheckSupport.SetActive(true);
        Vector3 pos = new Vector3(-3, 3, 0);
        Vector2 Size = new Vector2(3, 5);
        Vector2 CharacterFront = Player.Instance.transform.position + pos;
        var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject.layer == 12 || Colliders[i].gameObject.layer == 13 || Colliders[i].gameObject.layer == 18)
            {
                MoveArea.x = 0;
                break;
            }
        }
        //CharacterData.Instance.CheckSupport.transform.parent = Player.Instance.transform;
        //CharacterData.Instance.CheckSupport.transform.position = CharacterFront;
        //CharacterData.Instance.CheckSupport.GetComponent<BoxCollider2D>().size = Size;

        Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject.layer == 13)
            {
                if (Colliders[i].gameObject.tag == "door")
                {
                    Colliders[i].GetComponent<Door>().ActiveObj();
                    break;
                }
                else if (Colliders[i].gameObject.tag != "Wall")
                {
                    PlayerControl.Instance.TargetObj = Colliders[i].gameObject.GetComponent<Object>();
                    PlayerControl.Instance.Movedirection = DIRECTION.WEST;
                    push = true;
                    break;
                }
            }
            if (push == false)
            {
                PlayerControl.Instance.TargetObj = null;
            }
        }
    }
}
