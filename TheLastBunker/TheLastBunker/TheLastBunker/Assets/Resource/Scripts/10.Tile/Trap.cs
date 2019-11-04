using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : Tile
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("!!!");
            PlayerControl.Instance.playerstate = PlayerState.PLAYER_DIE;
            PlayerControl.Instance.dietype = DieType.DIE_DOWN;
        }
    }


}
