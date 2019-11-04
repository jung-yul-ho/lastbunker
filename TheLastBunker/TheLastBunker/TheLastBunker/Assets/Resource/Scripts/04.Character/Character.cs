using Spine.Unity;
using Spine.Collections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public int Hp;
    public Room MyRoom;

    //캐릭터(플레이어)가 방을 이동하는가 여부
    public bool EscapeRoom;

    //연결된 애니메이션 컨트롤러
    public AniControl anicontrol;
    public ViewingAngle viewangle;

    public virtual void Start()
    {

    }

    public virtual void Update()
    {
        if (!EscapeRoom)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
        else
        {
            EscapeRoom = false;
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        }
    }

    public virtual IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.5f);
        //
        Vector3 pos = new Vector3();
        if (viewangle.m_viewRotateZ < 0)
        {
            pos = new Vector3(-3, 5, 0);
        }
        else
        {
            pos = new Vector3(3, 5, 0);
        }
        Vector2 Size = new Vector2(30, 30);
        Vector2 CharacterFront = transform.position + pos;
        var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
        for (int i = 0; i < Colliders.Length; i++)
        {
            if (Colliders[i].gameObject.layer == 9 && PlayerControl.Instance.Invincibility == false)
            {
                StartCoroutine(Colliders[i].transform.gameObject.GetComponent<Player>().DamagePlayer());
            }
        }
        yield return new WaitForSeconds(0.2f);
        yield return new WaitForSeconds(2.0f);
    }

    public bool LerpSupportUp()
    {
        if (PlayInformation.Instance.UsingDoor == false)
        {
            Vector3 pos = new Vector3(0, 8, 0);
            Vector2 Size = new Vector2(4, 1);
            Vector2 CharacterFront = Player.Instance.transform.position + pos;
            var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
            for (int i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i].gameObject.layer == 12 || Colliders[i].gameObject.layer == 13 || Colliders[i].gameObject.layer == 18)
                {
                    if (Colliders[i].gameObject.tag != "door")
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    public bool LerpSupportDown()
    {
        if (PlayInformation.Instance.UsingDoor == false)
        {
            Vector3 pos = new Vector3(0, -1, 0);
            Vector2 Size = new Vector2(4, 1);
            Vector2 CharacterFront = Player.Instance.transform.position + pos;
            var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
            for (int i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i].gameObject.layer == 12 || Colliders[i].gameObject.layer == 13 || Colliders[i].gameObject.layer == 18)
                {
                    if (Colliders[i].gameObject.tag != "door")
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    public bool LerpSupportLeft()
    {
        if (PlayInformation.Instance.UsingDoor == false)
        {
            Vector3 pos = new Vector3(-3, 5, 0);
            Vector2 Size = new Vector2(3, 6);
            Vector2 CharacterFront = Player.Instance.transform.position + pos;
            var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
            for (int i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i].gameObject.layer == 12 || Colliders[i].gameObject.layer == 13 || Colliders[i].gameObject.layer == 18)
                {
                    if (Colliders[i].gameObject.tag != "door")
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
    public bool LerpSupportRight()
    {
        if (PlayInformation.Instance.UsingDoor == false)
        {
            Vector3 pos = new Vector3(3, 5, 0);
            Vector2 Size = new Vector2(3, 6);
            Vector2 CharacterFront = Player.Instance.transform.position + pos;
            var Colliders = Physics2D.OverlapBoxAll(CharacterFront, Size, 0.1f);
            for (int i = 0; i < Colliders.Length; i++)
            {
                if (Colliders[i].gameObject.layer == 12 || Colliders[i].gameObject.layer == 13 || Colliders[i].gameObject.layer == 18)
                {
                    if (Colliders[i].gameObject.tag != "door")
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}
