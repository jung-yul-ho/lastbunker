using Spine.Unity;
using Spine.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum MonsterName
{
    MONSTER_MACHINE_DOG,
    MONSTER_CYBOG,
    MONSTER_CAMERA,
    MONSTER_SB300
}

public enum MonsterAttackType
{
    Near,
    Remote
}
public enum EnermyState
{
    ENERMY_IDLE,
    ENERMY_KNOCKBACK,
    ENERMY_MOVE,
    ENERMY_ATTACK,
    ENERMY_DIE
}

public class Enermy : Character
{
    //캐릭터의 상태
    public EnermyState enermystate;
    public MonsterName monstername;

    //캐릭터의 공격 타입
    public MonsterAttackType attacktype;

    //캐릭터의 위치정보
    public Transform monsterTr;
    public Transform playerTr;
    public float Targeting;
    public float TraceDist;
    public float attackDist;
    //플레이어 추격 타임(추격대상을 잃더라도 잠시동안은 추적)
    public float TraceTime;
    public bool FindPlayer;
    //속도
    public float velocity;
    //추적가속도
    public float accelaration;
    //넉백방향
    public Vector3 NuckBackDirection;
    //넉백가속도
    public float NuckBackAccelaration;
    //추적대상위치
    public Vector3 direction;
    //사망여부
    public bool Die;
    //공격여부(딜레이)
    public bool attack;

    public GameObject Laser;

    //플레이어가 방을 이동했을때 위치정보 갱신을 위해 만들어놈
    public Tile ParentTile;

    public override void Start()
    {
        viewangle = GetComponent<ViewingAngle>();
        Die = false;
        anicontrol = GetComponent<EnermyAniControl>();
        playerTr = Player.Instance.gameObject.transform;
        monsterTr = gameObject.GetComponent<Transform>();
        viewangle.mycharacter = this;
    }

    public void OnParticleCollision(GameObject other)
    {
        StartCoroutine(KnockbackDamage(0.1f, 0.1f, true));
    }

    public override void Update()
    {
        //if(FindPlayer == true)
        //{
        //    Debug.Log((Player.Instance.transform.position - transform.position).normalized);
        //}
        if (Hp <= 0 && Die == false)
        {
            enermystate = EnermyState.ENERMY_DIE;
            StartCoroutine(anicontrol.DieStart());
            Die = true;
            //RoomStateChangeDie();
        }
        else if (PlayerControl.Instance.playerstate != PlayerState.PLAYER_DIE && PlayerControl.Instance.Damaged == false && PlayInformation.Instance.GameActive == true)
        {
            Targeting = Vector3.Distance(monsterTr.transform.position, playerTr.transform.position);
            if (Hp <= 0)
            {
                if (Laser != null)
                {
                    Laser.SetActive(false);
                }
                enermystate = EnermyState.ENERMY_DIE;
            }
            else if (enermystate == EnermyState.ENERMY_KNOCKBACK)
            {
                if (Laser != null)
                {
                    Laser.SetActive(false);
                }
                transform.Translate(NuckBackDirection * NuckBackAccelaration * Time.deltaTime);
                NuckBackAccelaration -= 1.0f;
            }
            else if (TraceTime > 0)
            {
                if (Targeting <= attackDist)
                {
                    enermystate = EnermyState.ENERMY_ATTACK;
                    if (Laser != null)
                    {
                        Laser.SetActive(true);
                        if (attack == false && Laser.gameObject.activeSelf == true)
                        {
                            Vector2 aa = new Vector2(Player.Instance.transform.position.x, Player.Instance.transform.position.y + 5);
                            Laser.gameObject.transform.LookAt(aa);
                            StartCoroutine(LaserAttack());
                        }
                    }
                    else
                    {
                        StartCoroutine(MelleAttack());
                    }
                }
                else if (Targeting <= TraceDist)
                {
                    //if (Laser != null)
                    //{
                    //    Laser.SetActive(false);
                    //}
                    enermystate = EnermyState.ENERMY_MOVE;
                    if(monstername != MonsterName.MONSTER_CAMERA)
                    {
                        Trace();
                    }
                }
            }
            else
            {
                //if (Laser != null)
                //{
                //    Laser.SetActive(false);
                //}
                enermystate = EnermyState.ENERMY_IDLE;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" &&PlayerControl.Instance.Damaged == false)
        {
            Player.Instance.NuckBackAccelaration = 20.0f;
            Player.Instance.NuckBackDirection = (Player.Instance.transform.position - transform.position).normalized;
            PlayerControl.Instance.Damaged = true;
            StartCoroutine(Player.Instance.DamagePlayer());
        }
    }

    public IEnumerator MelleAttack()
    {
        TraceTime = 10.0f;
        bool attack = false;
        Vector3 pos = new Vector3();
        Vector2 Size = new Vector2(20, 20);
        if (viewangle.m_viewRotateZ < 0)
        {
            pos = new Vector3(-3, 5, 0);
        }
        else
        {
            pos = new Vector3(3, 5, 0);
        }
        Vector2 CharacterFront = transform.position + pos;
        var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject.layer == 9)
            {
                attack = true;
            }
        }
        //CharacterData.Instance.CheckSupport.transform.parent = transform;
        //CharacterData.Instance.CheckSupport.transform.position = CharacterFront;
        //CharacterData.Instance.CheckSupport.GetComponent<BoxCollider2D>().size = Size;
        if (attack == true)
        {
            StartCoroutine(Attack());
        }
        yield return new WaitForSeconds(2.0f);
    }

    public IEnumerator LaserAttack()
    {
        attack = true;
        Laser.GetComponent<ParticleSystem>().Stop();
        yield return new WaitForSeconds(0.3f);
        TraceTime = 10.0f;
        Laser.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(0.86f);
        Laser.GetComponent<ParticleSystem>().Stop();
        attack = false;
    }

    public void RoomStateChangeDie()
    {
        if(MyRoom.roommoveterm == RoomMoveTerms.MONSTER_DESTROY)
        {
            bool check = true;
            for(int i = 0; i< MyRoom.enermies.Count; i++)
            {
                if(MyRoom.enermies[i] != null)
                {
                    if(MyRoom.enermies[i].Die == false)
                    {
                        check = false;
                        break;
                    }
                }
            }
            if(check == true)
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
                PlayInformation.Instance.StageClearCheck();
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
        PlayInformation.Instance.GameActive = true;
        PlayerControl.Instance.playerstate = PlayerState.PLAYER_IDLE;
    }

    //플레이어를 추적하는 코드
    public void Trace()
    {
        if(PlayInformation.Instance.playstate == PlayState.PLAY_STATE_PLAY)
        {
            enermystate = EnermyState.ENERMY_MOVE;
            velocity = (velocity + accelaration * Time.deltaTime);
            direction = (playerTr.position - transform.position).normalized;

            transform.position = new Vector3(transform.position.x + (direction.x * velocity), transform.position.y + (direction.y * velocity), transform.position.z);
        }
        else
        {
            enermystate = EnermyState.ENERMY_IDLE;
        }
    }

    public void GoMyTile()
    {
        this.gameObject.transform.position = ParentTile.transform.position;
    }

    //중형
    //public IEnumerator KnockbackDamage(float LeftPower, float RightPower)
    //{
    //    NuckBackAccelaration = 150.0f;
    //    enermystate = EnermyState.ENERMY_KNOCKBACK;
    //    NuckBackDirection = (transform.position - Player.Instance.transform.position ).normalized; ;
    //    yield return new WaitForSeconds(0.2f);
    //    enermystate = EnermyState.ENERMY_IDLE;
    //    Hp--;
    //}

    public IEnumerator KnockbackDamage(float LeftPower, float RightPower, bool Damage)
    {
        StartCoroutine(ColorChange());
        NuckBackAccelaration = 100.0f;
        enermystate = EnermyState.ENERMY_KNOCKBACK;
        NuckBackDirection = (transform.position - Player.Instance.transform.position).normalized;
        yield return new WaitForSeconds(0.2f);
        enermystate = EnermyState.ENERMY_IDLE;
        if(Damage == true)
        {
            Hp--;
        }
    }

    public IEnumerator ColorChange()
    {
        int CountTime = 0;
        while (CountTime < 7)
        {
            if (CountTime % 2 == 0)
            {
                anicontrol.skeletonAnimation.skeleton.SetColor(Color.white);
            }
            else
            {
                anicontrol.skeletonAnimation.skeleton.SetColor(Color.red);
            }
            yield return new WaitForSeconds(0.1f);
            CountTime++;
        }
    }
}