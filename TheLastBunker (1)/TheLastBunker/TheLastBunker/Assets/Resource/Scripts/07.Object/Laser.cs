using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LaserType
{
    LASER_UP,
    LASER_DOWN
}

public class Laser : Object
{
    public LaserType lasertype;
    public override void GoMyTile()
    {
        if(lasertype == LaserType.LASER_UP)
        {
            Vector2 aa = new Vector2(ParentTile.transform.position.x, ParentTile.transform.position.y - 6);
            this.gameObject.transform.position = aa;
            TargetVector = aa;
        }
        else
        {
            Vector2 aa = new Vector2(ParentTile.transform.position.x, ParentTile.transform.position.y - 2);
            this.gameObject.transform.position = aa;
            TargetVector = aa;
        }
    }
}
