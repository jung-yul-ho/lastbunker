using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoomInformation
{
    public int StageNum;
    public int RoomNum;
    public int RoomNumX;
    public int RoomNumY;
    public GameObject RoomImage;
    public Texture2D RoomTiles;
    public Texture2D DoorTiles;
    public Texture2D EnermyRespown;
    public Texture2D specialObjPosition;
    public Texture2D ObjectPosition;
    public Texture2D WallTiels;
    public Texture2D Trap;
    public RoomMoveTerms roommoveterm;
    public int eventcontrol;
    public int ClueEventNum;
    public int PaperClueEventNum;
    public int ObjPrintEventNum;
    public int DownDoorEventNum;
    public List<int> LaserRabberNum;
    public List<int> LaserControlerNum;
}

public class RoomData : MonoBehaviour
{
    public GameObject PortalPrefab;
    public GameObject RoomPrefab;
    public List<RoomInformation> RoomInformation;

    public List<Tile> TileList;

    static RoomData instance;
    public static RoomData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RoomData>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<RoomData>();
                }
            }
            return instance;
        }
    }
}
