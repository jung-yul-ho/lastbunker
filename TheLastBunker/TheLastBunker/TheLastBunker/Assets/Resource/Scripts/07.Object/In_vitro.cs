using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class In_vitro : Object
{
    public override void GoMyTile()
    {
        Vector2 aa = new Vector2(ParentTile.transform.position.x, ParentTile.transform.position.y + 5);
        gameObject.transform.position = aa;
        TargetVector = aa;
    }
}
