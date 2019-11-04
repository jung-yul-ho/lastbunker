using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum PlayerState
{
    PLAYER_PAUSE,
    PLAYER_IDLE,
    PLAYER_MOVE,
    PLAYER_ATTACK,
    PLAYER_DIE,
    PLAYER_LIFTING,
    PLAYER_VIEWAREA
}

public enum DieType
{
    DIE_DOWN,
    DIE_DAMAGE
}

public enum Viewdirection
{
    VIEW_LEFT,
    VIEW_RIGHT,
}

public class PlayerControl : MonoBehaviour
{
    public bool Die;
    public bool Dash;
    public bool Damaged;
    public bool Invincibility;
    public PlayerState playerstate;
    public Viewdirection viewdirection;
    public DieType dietype;
    //메뉴이미지
    public GameObject MenuButton;
    //메뉴패널
    public GameObject Menu;
    //캐릭터정보패널
    public GameObject CharacterInfo;

    public Button AttackButton;
    public Button SpecialOne;
    public Button SpecialTwo;

    //모드 스프라이트
    public Sprite Mod_Power;
    public Sprite Mod_Speed;
    public Sprite ActiveRabber;
    public Sprite NoActiveRabber;
    public Sprite ActiveSecretRabber;
    public Sprite NoActiveSecretRabber;

    //HP이미지
    public Image HpImage;
    public Sprite HpOne;
    public Sprite HpTwo;
    public Sprite HpThree;
    public Sprite HpFour;
    public Sprite HpFive;

    //오브젝트 상호작용 보조
    public Object TargetObj;
    public DIRECTION Movedirection;

    //임시
    public GameObject MuJuck;

    static PlayerControl instance;
    public static PlayerControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerControl>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<PlayerControl>();
                }
            }
            return instance;
        }
    }

    public void Start()
    {
        Die = false;
        Dash = false;
        Damaged = false;
        Invincibility = false;
    }

    public void Attack()
    {
        if (PlayInformation.Instance.GameOver == false)
        {
            bool attack = false;
            Vector3 pos = new Vector3();
            Vector2 Size = new Vector2(20, 20);
            if (viewdirection == Viewdirection.VIEW_LEFT)
            {
                pos = new Vector3(-3, 5, 0);
            }
            else if (viewdirection == Viewdirection.VIEW_RIGHT)
            {
                pos = new Vector3(3, 5, 0);
            }
            Vector2 CharacterFront = Player.Instance.transform.position + pos;
            var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
            for (int i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i].gameObject.layer == 10)
                {
                    if (viewdirection == Viewdirection.VIEW_LEFT)
                    {
                        attack = true;

                    }
                    else if (viewdirection == Viewdirection.VIEW_RIGHT)
                    {
                        attack = true;
                    }
                }
            }
            if (attack == true)
            {
                StartCoroutine(Player.Instance.Attack());
            }
            else if (TargetObj != null)
            {
                if (TargetObj.tag == "box")
                {
                    TargetObj.MoveObj(Movedirection);
                }
                else if (TargetObj.tag == "rabber")
                {
                    TargetObj.ActiveObj();
                }
                else if(TargetObj.tag == "proviso")
                {
                    TargetObj.ActiveObj();
                }
                else if(TargetObj.tag == "Secretrabber")
                {
                    MessageBox.Instance.ShowMessage("좀더 무거운것이 필요할거 같다");
                }
                else if(TargetObj.tag == "Clue")
                {
                    TargetObj.GetComponent<Clue>().ActiveObj();
                }
                else if(TargetObj.tag == "ObjPrinter")
                {
                    TargetObj.GetComponent<ObjectPrinter>().ActiveObj();
                }
                else if(TargetObj.tag == "EasyDoor")
                {
                    TargetObj.GetComponent<EasyDoor>().ActiveObj();
                }
                else
                {
                    StartCoroutine(Player.Instance.Attack());
                }
                TargetObj = null;
            }
            else
            {
                StartCoroutine(Player.Instance.Attack());
            }
        }
    }

    public void PlayerModChange()
    {
        if(PlayInformation.Instance.GameOver == false)
        {
            if(Player.Instance.playermod == PlayerMod.MOD_POWER)
            {
                SpecialTwo.GetComponent<Image>().sprite = Mod_Speed;
                Player.Instance.playermod = PlayerMod.MOD_SPEED;
            }
            else
            {
                SpecialTwo.GetComponent<Image>().sprite = Mod_Power;
                Player.Instance.playermod = PlayerMod.MOD_POWER;
            }
        }
    }

    public void ItemGetting()
    {
        if (PlayerAniControl.Instance.SpineSetName == "총장비")
        {
            PlayerAniControl.Instance.SpineSelect("기본");
        }
        else
        {
            PlayerAniControl.Instance.SpineSelect("총장비");
        }
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
        PlayInformation.Instance.GameActive = false;
        playerstate = PlayerState.PLAYER_PAUSE;
        Menu.SetActive(true);
    }

    public void Restart()
    {
        Time.timeScale = 1.0f;
        if (EventControl.Instance.dowmmessagebox.activeSelf == true)
        {

        }
        else
        {
            PlayInformation.Instance.GameActive = true;
        }
        playerstate = PlayerState.PLAYER_IDLE;
        Menu.SetActive(false);
    }

    public void EXIT()
    {
        Time.timeScale = 1.0f;
        Destroy(RoomData.Instance.gameObject);
        Destroy(ObjectData.Instance.gameObject);
        Destroy(CharacterData.Instance.gameObject);
        Destroy(PlayInformation.Instance.gameObject);
        SceneManager.LoadScene("Stage_Scene");
        SceneManager.LoadScene("Data_Scene", LoadSceneMode.Additive);
        SaveLoadControl.Instance.Save();
    }

    public void GetDash()
    {
        if(Dash == false)
        {
            StartCoroutine(DashStart());
        }
    }

    public IEnumerator DashStart()
    {
        Dash = true;
        Invincibility = true;
        Damaged = true;
        PlayInformation.Instance.GameActive = false;
        Vector2 newvector = new Vector2(Player.Instance.transform.position.x + (JoystickControler.Instance.MoveArea.x * 30), Player.Instance.transform.position.y + (JoystickControler.Instance.MoveArea.y * 30));
        //Vector2 newvector = new Vector2(Player.Instance.transform.position.x + 15, Player.Instance.transform.position.y);
        Player.Instance.PlayerTargetTransform = newvector;
        yield return new WaitForSeconds(0.6f);
        Dash = false;
        Invincibility = false;
        Damaged = false;
        PlayInformation.Instance.GameActive = true;
    }

    public void CharacterInfoView()
    {
        CharacterInfo.SetActive(true);
    }

    public void CharacterInfoClose()
    {
        CharacterInfo.SetActive(false);
    }

    public void IAMIMMORTAL()
    {
        StopAllCoroutines();
        if(Invincibility == false)
        {
            Invincibility = true;
            MuJuck.SetActive(true);
        }
        else
        {
            Invincibility = false;
            MuJuck.SetActive(false);
        }
    }

    //public void Lifting()
    //{
    //    if (TargetObj != null)
    //    {
    //        if (TargetObj.tag == "box")
    //        {
    //            StartCoroutine(GetLifting());
    //        }
    //    }
    //}

    //public IEnumerator GetLifting()
    //{
    //    if (JoystickControler.Instance.JoyVec.x > 0)
    //    {
    //        PlayerAniControl.Instance.AnimationLifting(-1);
    //    }
    //    else
    //    {
    //        PlayerAniControl.Instance.AnimationLifting(1);
    //    }
    //    playerstate = PlayerState.PLAYER_LIFTING;
    //    yield return new WaitForSeconds(1.0f);
    //    TargetObj.transform.parent = Player.Instance.transform;
    //    Vector2 aa = new Vector2(Player.Instance.transform.position.x, Player.Instance.transform.position.y + 15);
    //    TargetObj.transform.position = aa;
    //    TargetObj.tag = "Player";
    //    TargetObj.gameObject.layer = 9;
    //}
}