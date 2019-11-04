using Spine.Unity;
using Spine.Collections;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAniControl : AniControl
{
    public List<SpineSet> spineset;

    public string SpineSetName;

    [SpineAnimation]
    public string idleAnimation;

    [SpineAnimation]
    public string attackAnimation;

    [SpineAnimation]
    public string moveAnimation;

    [SpineAnimation]
    public string hitAnimation;

    [SpineAnimation]
    public string dieAnimation;

    [SpineAnimation]
    public string dashAnimation;

    [SpineAnimation]
    public string PutdownAnimation;

    [SpineAnimation]
    public string HoldAnimation;

    [SpineAnimation]
    public string WalkStumpAnimation;

    public SpineSkin skin;
    [SpineSkin]
    public string SpineSkin;

    static PlayerAniControl instance;
    public static PlayerAniControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerAniControl>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<PlayerAniControl>();
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.OnRebuild += Apply;
    }

    void Apply(SkeletonRenderer skeletonRenderer)
    {
        //StartCoroutine("Blink");
    }

    void Update()
    {
        if (PlayerControl.Instance.Die == true)
        {
            StopAllCoroutines();
            AnimationDie(1);
            skeletonAnimation.loop = false;
        }
        else if(PlayerControl.Instance.playerstate == PlayerState.PLAYER_LIFTING)
        {
            if (JoystickControler.Instance.moveflag == true)
            {
                if (JoystickControler.Instance.JoyVec.x > 0)
                {
                    AnimationMoveLifting(-1);
                }
                else
                {
                    AnimationMoveLifting(1);
                }
            }
            //else
            //{
            //    skeletonAnimation.AnimationName = idleAnimation;
            //}
        }
        else
        {
            if (PlayerControl.Instance.Dash == true)
            {
                if (JoystickControler.Instance.JoyVec.x > 0)
                {
                    AnimationDash(-1);
                }
                else
                {
                    AnimationDash(1);
                }
            }
            else if (PlayerControl.Instance.Damaged == true)
            {
                skeletonAnimation.AnimationName = hitAnimation;
            }
            else if (PlayerControl.Instance.playerstate == PlayerState.PLAYER_ATTACK)
            {
                if (JoystickControler.Instance.JoyVec.x > 0)
                {
                    AnimationAttack(-1);
                }
                else
                {
                    AnimationAttack(1);
                }
            }
            else if (JoystickControler.Instance.moveflag == true)
            {
                if (JoystickControler.Instance.JoyVec.x > 0)
                {
                    AnimationMove(-1);
                }
                else
                {
                    AnimationMove(1);
                }
            }
            else if (PlayInformation.Instance.playstate == PlayState.PLAY_STATE_ROOMMOVE_EAST)
            {
                AnimationMove(-1);
            }
            else if (PlayInformation.Instance.playstate == PlayState.PLAY_STATE_ROOMMOVE_WEST)
            {
                AnimationMove(1);
            }
            else if (PlayerControl.Instance.playerstate != PlayerState.PLAYER_LIFTING)
            {
                skeletonAnimation.AnimationName = idleAnimation;
            }
        }
    }

    public void AnimationMove(int scale)
    {
        skeletonAnimation.AnimationName = moveAnimation;
        skeletonAnimation.Skeleton.ScaleX = scale;
    }

    public void AnimationAttack(int scale)
    {
        skeletonAnimation.AnimationName = attackAnimation;
        skeletonAnimation.Skeleton.ScaleX = scale;
    }

    public void AnimationDie(int scale)
    {
        skeletonAnimation.AnimationName = dieAnimation;
        skeletonAnimation.Skeleton.ScaleX = scale;
    }

    public void AnimationDash(int scale)
    {
        skeletonAnimation.AnimationName = dashAnimation;
        skeletonAnimation.Skeleton.ScaleX = scale;
    }

    public void AnimationLifting(int scale)
    {
        skeletonAnimation.loop = false;
        skeletonAnimation.AnimationName = HoldAnimation;
        skeletonAnimation.Skeleton.ScaleX = scale;
    }

    public void AnimationMoveLifting(int scale)
    {
        skeletonAnimation.loop = true;
        skeletonAnimation.AnimationName = WalkStumpAnimation;
        skeletonAnimation.Skeleton.ScaleX = scale;
    }

    public void StopLifting()
    {
        skeletonAnimation.loop = true;
    }

    public void SpineSelect(string spinename)
    {
        for(int i = 0; i< spineset.Count; i++)
        {
            if(spineset[i].SpineSetName == spinename)
            {
                SpineSetName = spineset[i].SpineSetName;
                idleAnimation = spineset[i].idleAnimation;
                attackAnimation = spineset[i].attackAnimation;
                moveAnimation = spineset[i].moveAnimation;
                hitAnimation = spineset[i].hitAnimation;
                dieAnimation = spineset[i].dieAnimation;
                dashAnimation = spineset[i].dashAnimation;
            }
        }
    }
}
