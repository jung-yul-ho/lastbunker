using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectData : MonoBehaviour
{
    public Clue clue;
    public Clue paperclue;
    public Door LeftDoor;
    public Door RightDoor;
    public Door DownDoor;
    public Door UpDoor;
    public Drum drum;
    public EasyDrum easydrum;
    public Rabber rabber;
    public LaserControler LaserControler;
    public Laser laser_up;
    public Laser laser_down;
    public In_vitro in_vitro;
    public Wall NormalWall;
    public Wall StageTwoNormalWall;
    public Wall UpperWall;
    public Firstaid firstaid;
    public SecretSwitch secretswitch;
    public ElectricTrap electrictrap;
    public ElectricTrap Loopelectrictrap;
    public ClearRabber clearrabber;
    public ObjectPrinter objectprinter;
    public EasyDoor easydoor;

    static ObjectData instance;
    public static ObjectData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectData>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<ObjectData>();
                }
            }
            return instance;
        }
    }
}
