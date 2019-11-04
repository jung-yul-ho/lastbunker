using UnityEngine;
using System.Collections;
using Spine.Unity;

public class EnermyAniControl : AniControl
{
    Enermy MyRealEnermy;


    [SpineAnimation]
    public string idleAnimation;

    [SpineAnimation]
    public string attackAnimation;

    [SpineAnimation]
    public string moveAnimation;

    [SpineAnimation]
    public string dieAnimation;

    [SpineSlot]
    public string eyesSlot;

    [SpineAttachment(currentSkinOnly: true, slotField: "eyesSlot")]
    public string eyesOpenAttachment;

    [SpineAttachment(currentSkinOnly: true, slotField: "eyesSlot")]
    public string blinkAttachment;

    [Range(0, 0.2f)]
    public float blinkDuration = 0.05f;

    public float moveSpeed = 3;

    void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        skeletonAnimation.OnRebuild += Apply;
        MyRealEnermy = GetComponent<Enermy>();
    }

    void Apply(SkeletonRenderer skeletonRenderer)
    {
        StartCoroutine("Blink");
    }

    void Update()
    {
        if (MyRealEnermy.enermystate == EnermyState.ENERMY_DIE)
        {
            if (Player.Instance.transform.position.x > this.gameObject.transform.position.x)
            {
                AnimationDie(-1);
                MyRealEnermy.viewangle.m_viewRotateZ = +90.0f;
            }
            else
            {
                AnimationDie(1);
                MyRealEnermy.viewangle.m_viewRotateZ = -90.0f;
            }
        }
        else if (MyRealEnermy.enermystate == EnermyState.ENERMY_ATTACK)
        {
            if (Player.Instance.transform.position.x > this.gameObject.transform.position.x)
            {
                AnimationAttack(-1);
                MyRealEnermy.viewangle.m_viewRotateZ = +90.0f;
            }
            else
            {
                AnimationAttack(1);
                MyRealEnermy.viewangle.m_viewRotateZ = -90.0f;
            }
        }
        else if (MyRealEnermy.enermystate == EnermyState.ENERMY_MOVE)
        {
            if (Player.Instance.transform.position.x > this.gameObject.transform.position.x)
            {
                AnimationMove(-1);
                MyRealEnermy.viewangle.m_viewRotateZ = +90.0f;
            }
            else
            {
                AnimationMove(1);
                MyRealEnermy.viewangle.m_viewRotateZ = -90.0f;
            }
        }

        else
        {
            skeletonAnimation.AnimationName = idleAnimation;
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

    IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(0.25f, 3f));
            skeletonAnimation.Skeleton.SetAttachment(eyesSlot, blinkAttachment);
            yield return new WaitForSeconds(blinkDuration);
            skeletonAnimation.Skeleton.SetAttachment(eyesSlot, eyesOpenAttachment);
        }
    }
}
