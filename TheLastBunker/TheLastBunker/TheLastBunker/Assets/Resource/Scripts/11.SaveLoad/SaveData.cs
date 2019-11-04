using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class SaveData
{
    public int SecurityLevel;
    public int RoomLevel;
    public bool Mod_open;

    public SaveData(int securitylevel, int roomlevel, bool Mod_open)
    {
        this.SecurityLevel = securitylevel;
        this.RoomLevel = roomlevel;
        this.Mod_open = Mod_open;
    }

    public SaveData()
    {

    }

    //public int killcount;

    //public SaveData(int killcount)
    //{
    //    this.killcount = 1111;
    //    //killcount = this.killcount;
    //}
}