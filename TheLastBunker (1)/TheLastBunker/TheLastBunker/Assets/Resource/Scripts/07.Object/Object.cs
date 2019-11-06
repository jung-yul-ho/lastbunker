using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object : MonoBehaviour
{
    public Room ControlerRoom;
    public Tile ParentTile;
    public Vector2 TargetVector;
    public bool CanActive;
    public GameObject blindObj;

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    public virtual void Update()
    {
        if(tag != "Player")
        {
            transform.position = Vector3.Lerp(transform.position, TargetVector, Time.deltaTime * 5.0f);
        }
    }

    public virtual void Active()
    {

    }

    public virtual void GoMyTile()
    {
        this.gameObject.transform.position = ParentTile.transform.position;
        TargetVector = ParentTile.transform.position;
    }

    public void MoveObj(DIRECTION direction)
    {
        if (direction == DIRECTION.EAST)
        {
            Vector2 aa = new Vector2(transform.position.x + 10.0f, transform.position.y);
            if (CheckCanMoveObj(aa) == true)
            {
                TargetVector = aa;
            }
        }
        else if(direction == DIRECTION.WEST)
        {
            Vector2 aa = new Vector2(transform.position.x - 10.0f, transform.position.y);
            if (CheckCanMoveObj(aa) == true)
            {
                TargetVector = aa;
            }
        }
        else if (direction == DIRECTION.SOUTH)
        {
            Vector2 aa = new Vector2(transform.position.x, transform.position.y - 10.0f);
            if (CheckCanMoveObj(aa) == true)
            {
                TargetVector = aa;
            }

        }
        else if (direction == DIRECTION.NORTH)
        {
            Vector2 aa = new Vector2(transform.position.x, transform.position.y + 10.0f);
            if (CheckCanMoveObj(aa) == true)
            {
                TargetVector = aa;
            }
        }
    }

    public bool CheckCanMoveObj(Vector2 pos)
    {
        Vector2 Size = new Vector2(5, 5);
        var Colliders = Physics2D.OverlapBoxAll(pos, Size, 0.1f);
        bool NoTouch = true; ;
        for (int i = 0; i < Colliders.Length; i++)
        {
            if(Colliders[i].gameObject.tag == "Secretrabber")
            {
                blindObj = Colliders[i].gameObject;
                Colliders[i].gameObject.GetComponent<SecretSwitch>().ActiveObj();
                NoTouch = false;
                blindObj = Colliders[i].gameObject;
                //blindObj.gameObject.SetActive(false);
            }
            else if (Colliders[i].gameObject.tag == "FirstKit" || Colliders[i].gameObject.tag == "Canmove_obj")
            {
                blindObj = Colliders[i].gameObject;
                blindObj.gameObject.SetActive(false);
                NoTouch = false;
            }
            else if(Colliders[i].gameObject.tag == "Wall" || Colliders[i].gameObject.tag == "box")
            {
                NoTouch = false;
                return false;
            }
            else
            {

            }
        }

        //숨겨진 물건이 있을경우 드러내자
        if (blindObj != null && NoTouch == true)
        {
            blindObj.SetActive(true);
            if(blindObj.tag == "Canmove_obj")
            {
                StartCoroutine(blindObj.GetComponent<ElectricTrap>().ActiveTrap());
            }
        }
        return true;
    }

    public virtual void ActiveObj()
    {

    }

    public void SecretSwitchCehck()
    {
        for (int i = 0; i < PlayInformation.Instance.PlayingRoom.secretswitch.Count; i++)
        {
            if (PlayInformation.Instance.PlayingRoom.secretswitch[i].gameObject.activeSelf == false || PlayInformation.Instance.PlayingRoom.secretswitch[i].gameObject == this.gameObject)
            {
                PlayInformation.Instance.PlayingRoom.secretswitchdown[i] = true;
            }
            else
            {
                PlayInformation.Instance.PlayingRoom.secretswitchdown[i] = false;
            }
        }
    }
}
