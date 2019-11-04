using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Firstaid : Object
{
    //public override void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Player.Instance.Hp++;
    //        Player.Instance.HpImageReset();
    //        Destroy(this.gameObject);
    //    }
    //}

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
    }
}
