using Spine.Unity;
using Spine.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[System.Serializable]
public class SpineSet
{
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
    public string throwAnimation;
}

public class AniControl : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;

    public GameObject DieEffect;

    private void Start()
    {

    }

    void Update()
    {

    }

    public IEnumerator DieStart()
    {
        DieEffect.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        if (DieEffect != null)
        {
            DieEffect.SetActive(true);
        }
        Destroy(gameObject);
    }
}
