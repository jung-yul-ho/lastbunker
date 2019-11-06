using Spine.Unity;
using Spine.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PlayerMod
{
    MOD_NULL,
    MOD_POWER,
    MOD_SPEED
}

public class Player : Character
{
    public Vector3 PlayerTargetTransform;
    static Player instance;
    public Vector3 NuckBackDirection;
    public float NuckBackAccelaration;
    public PlayerMod playermod;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Player>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<Player>();
                }
            }
            return instance;
        }
    }

    public override void Start()
    {
        Hp = 3;
        JoystickControler.Instance.playercollider = this.GetComponent<BoxCollider2D>();
        anicontrol = this.gameObject.GetComponent<PlayerAniControl>();
        MyRoom = PlayInformation.Instance.PlayingRoom;
        HpImageReset();
    }

    public void OnParticleCollision(GameObject other)
    {
        if(PlayerControl.Instance.Invincibility == false)
        {
            StartCoroutine(DamagePlayer());
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "FirstKit")
        {
            Hp++;
            HpImageReset();
            Destroy(collision.gameObject);
        }
    }

    public override void Update()
    {
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        //데미지 입었을시 넉벡시키기
        if(PlayerControl.Instance.Damaged == true)
        {
            transform.Translate(NuckBackDirection * NuckBackAccelaration * Time.deltaTime);
            NuckBackAccelaration -= 0.12f;
        }

        //부드럽게 이동
        if (PlayerControl.Instance.playerstate == PlayerState.PLAYER_DIE)
        {
            if(PlayerControl.Instance.dietype == DieType.DIE_DOWN)
            {

            }
        }
        else if (PlayInformation.Instance.GameActive == false)
        {
            if(this.transform.position.x > PlayerTargetTransform.x)
            {
                if(LerpSupportLeft() == false)
                {
                    PlayerTargetTransform.x = this.transform.position.x;
                }
            }
            else
            {
                if (LerpSupportRight() == false)
                {
                    PlayerTargetTransform.x = this.transform.position.x;
                }
            }
            if(this.transform.position.y > PlayerTargetTransform.y)
            {
                if (LerpSupportDown() == false)
                {
                    PlayerTargetTransform.y = this.transform.position.y;
                }
            }
            else
            {
                if (LerpSupportUp() == false)
                {
                    PlayerTargetTransform.y = this.transform.position.y;
                }
            }
            transform.position = Vector3.Lerp(transform.position, PlayerTargetTransform, Time.deltaTime * 1.2f);
        }
    }

    //공격
    public override IEnumerator Attack()
    {
        //공격조건
        if(PlayerControl.Instance.playerstate == PlayerState.PLAYER_IDLE || PlayerControl.Instance.playerstate == PlayerState.PLAYER_MOVE)
        {
            PlayerControl.Instance.playerstate = PlayerState.PLAYER_ATTACK;
            yield return new WaitForSeconds(0.5f);
            //
            Vector3 pos = new Vector3();
            if (PlayerControl.Instance.viewdirection == Viewdirection.VIEW_LEFT)
            {
                pos = new Vector3(-3, 5, 0);
            }
            else if (PlayerControl.Instance.viewdirection == Viewdirection.VIEW_RIGHT)
            {
                pos = new Vector3(3, 5, 0);
            }
            Vector2 Size = new Vector2(30, 30);
            Vector2 CharacterFront = transform.position + pos;
            var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
            for (int i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i].gameObject.layer == 10)
                {
                    if (PlayerControl.Instance.viewdirection == Viewdirection.VIEW_LEFT)
                    {
                        if(playermod == PlayerMod.MOD_POWER)
                        {
                            StartCoroutine(Colliders[i].transform.parent.gameObject.GetComponent<Enermy>().KnockbackDamage(-5.0f, 0.0f, true));
                        }
                        else
                        {
                            StartCoroutine(Colliders[i].transform.parent.gameObject.GetComponent<Enermy>().KnockbackDamage(-5.0f, 0.0f, false));
                        }
                    }
                    else if (PlayerControl.Instance.viewdirection == Viewdirection.VIEW_RIGHT)
                    {
                        if (playermod == PlayerMod.MOD_POWER)
                        {
                            StartCoroutine(Colliders[i].transform.parent.gameObject.GetComponent<Enermy>().KnockbackDamage(0.0f, 5.0f, true));
                        }
                        else
                        {
                            StartCoroutine(Colliders[i].transform.parent.gameObject.GetComponent<Enermy>().KnockbackDamage(0.0f, 5.0f, false));
                        }
                    }
                }
                if(Colliders[i].gameObject.tag == "Easybox")
                {
                    if(Colliders[i].GetComponent<EasyDrum>().InvisibleObj == null)
                    {
                        Colliders[i].gameObject.GetComponent<SpriteRenderer>().sprite = Colliders[i].gameObject.GetComponent<EasyDrum>().BrokenSprite;
                        Colliders[i].gameObject.layer = 17;
                    }
                    else
                    {
                        Colliders[i].GetComponent<EasyDrum>().InvisibleObj.SetActive(true);
                        Destroy(Colliders[i].gameObject);
                    }
                }
            }
            yield return new WaitForSeconds(0.2f);
            PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
        }
    }

    public void HpImageReset()
    {
        if(Hp == 1)
        {
            PlayerControl.Instance.HpImage.sprite = PlayerControl.Instance.HpOne;
        }
        else if(Hp == 2)
        {
            PlayerControl.Instance.HpImage.sprite = PlayerControl.Instance.HpTwo;
        }
        else if (Hp == 3)
        {
            PlayerControl.Instance.HpImage.sprite = PlayerControl.Instance.HpThree;
        }
        else if (Hp == 4)
        {
            PlayerControl.Instance.HpImage.sprite = PlayerControl.Instance.HpFour;
        }
        else if (Hp == 5)
        {
            PlayerControl.Instance.HpImage.sprite = PlayerControl.Instance.HpFive;
        }
        else if(Hp <= 0)
        {
            if(PlayerControl.Instance.Die == false)
            {
                PlayerControl.Instance.Die = true;
                MessageBox.Instance.ShowMessageLong("Game Over");
                StartCoroutine(PlayInformation.Instance.Lose());
            }
        }
    }

    public IEnumerator DamagePlayer()
    {
        StopCoroutine(Attack());
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        PlayerControl.Instance.Invincibility = true;
        PlayerControl.Instance.Damaged = true;
        Hp--;
        HpImageReset();
        int CountTime = 0;
        if (Hp > 0)
        {
            while (CountTime < 3)
            {
                if (CountTime % 2 == 0)
                {
                    anicontrol.skeletonAnimation.skeleton.SetColor(Color.white);
                }
                else
                {
                    anicontrol.skeletonAnimation.skeleton.SetColor(Color.clear);
                }
                yield return new WaitForSeconds(0.1f);
                CountTime++;
            }
        }
        anicontrol.skeletonAnimation.skeleton.SetColor(Color.white);
        yield return new WaitForSeconds(0.1f);
        this.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        PlayerControl.Instance.Damaged = false;
        StartCoroutine(Invincibility());
        anicontrol.skeletonAnimation.skeleton.SetColor(Color.white);
    }

    public IEnumerator Invincibility()
    {
        int CountTime = 0;
        while (CountTime < 9)
        {
            if (CountTime % 2 == 0)
            {
                anicontrol.skeletonAnimation.skeleton.SetColor(Color.white);
            }
            else
            {
                anicontrol.skeletonAnimation.skeleton.SetColor(Color.clear);
            }
            yield return new WaitForSeconds(0.1f);
            CountTime++;
        }
        yield return new WaitForSeconds(0.1f);
        PlayerControl.Instance.Invincibility = false;
    }
}
