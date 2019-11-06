using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RoomControler : MonoBehaviour
{
    public List<Room> InGameArea;
    int RoomCount = 0;
    Color black = new Color(0, 0, 0);
    Color white = new Color(1, 1, 1);
    Color red = new Color(1, 0, 0);
    Color green = new Color(0, 1, 0);
    Color blue = new Color(0, 0, 1);
    Color yellow = new Color(1, 1, 0);
    Color pupple = new Color(1, 0, 1);
    Color mint = new Color(0, 1, 1);
    
    static RoomControler instance;
    public static RoomControler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RoomControler>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<RoomControler>();
                }
            }
            return instance;
        }
    }

    public void Start()
    {
        StageCreate();
    }

    //스테이지를 창조하는 명령
    public void StageCreate()
    {
        for(int i = 0; i< RoomData.Instance.RoomInformation.Count; i++)
        {
            if(RoomData.Instance.RoomInformation[i].StageNum == PlayInformation.Instance.PlayStageLevel)
            {
                CreateRoom(RoomData.Instance.RoomInformation[i]);
                if (RoomData.Instance.RoomInformation[i].RoomNum == 1)
                {
                    PlayInformation.Instance.ResetRooms();
                }
            }
        }
        PlayInformation.Instance.PlayingRoom.eventcheck();
    }
    
    //룸을 창조하는 명령
    public void CreateRoom(RoomInformation roominformation)
    {
        GameObject newarea = Instantiate(RoomData.Instance.RoomPrefab.gameObject);
        newarea.GetComponent<Room>().AreaTiles = new List<Tile>();
        InGameArea.Add(newarea.GetComponent<Room>());
        newarea.transform.parent = gameObject.transform;
        newarea.SetActive(true);
        newarea.GetComponent<Room>().x = roominformation.RoomNumX;
        newarea.GetComponent<Room>().y = roominformation.RoomNumY;
        TransferMyAreas(newarea.GetComponent<Room>());
        Color[] RoomInformation = roominformation.RoomTiles.GetPixels();
        Color[] DoorInformation = roominformation.DoorTiles.GetPixels();
        Color[] EnermyInformation = roominformation.EnermyRespown.GetPixels();
        Color[] ObjectInformation = roominformation.ObjectPosition.GetPixels();
        Color[] wallinformation = roominformation.WallTiels.GetPixels();
        Color[] SpecialObjinformation = roominformation.specialObjPosition.GetPixels();
        Color[] Trapinformation = roominformation.Trap.GetPixels();
        //룸클리어 조건
        newarea.GetComponent<Room>().roommoveterm = roominformation.roommoveterm;
        if(newarea.GetComponent<Room>().roommoveterm == RoomMoveTerms.FREE)
        {
            newarea.GetComponent<Room>().CanMoveRoom = true;
        }
        else
        {
            newarea.GetComponent<Room>().CanMoveRoom = false;
        }
        int p = 0;
        //기초 타일들을 만드는 코드
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                if(RoomInformation[p] == white)
                {
                    Tile NewTile = Instantiate(RoomData.Instance.TileList[0]);
                    NewTile.gameObject.SetActive(true);
                    NewTile.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                    NewTile.gameObject.transform.parent = newarea.transform;
                    InGameArea[RoomCount].AreaTiles.Add(NewTile);
                }
                else if(RoomInformation[p] == black)
                {
                    Tile NewTile = Instantiate(RoomData.Instance.TileList[1]);
                    NewTile.gameObject.SetActive(true);
                    NewTile.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                    NewTile.gameObject.transform.parent = newarea.transform;
                    InGameArea[RoomCount].AreaTiles.Add(NewTile);
                }
                p++;
            }
        }

        p = 0;
        //벽을 창조하는 코드
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                if (wallinformation[p] == green)
                {
                    In_vitro in_vitro = Instantiate(ObjectData.Instance.in_vitro);
                    in_vitro.gameObject.SetActive(true);
                    in_vitro.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100) + 5, 0);
                    in_vitro.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(in_vitro);
                    in_vitro.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    in_vitro.ControlerRoom = newarea.GetComponent<Room>();
                    in_vitro.TargetVector = in_vitro.transform.position;
                }
                else if(wallinformation[p] == black)
                {
                    if(roominformation.StageNum == 1)
                    {
                        Wall wall = Instantiate(ObjectData.Instance.NormalWall);
                        wall.gameObject.SetActive(true);
                        wall.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                        wall.gameObject.transform.parent = newarea.transform;
                        newarea.GetComponent<Room>().objs.Add(wall);
                        wall.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                        wall.ControlerRoom = newarea.GetComponent<Room>();
                        wall.TargetVector = wall.transform.position;
                    }
                    else
                    {
                        Wall wall = Instantiate(ObjectData.Instance.StageTwoNormalWall);
                        wall.gameObject.SetActive(true);
                        wall.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                        wall.gameObject.transform.parent = newarea.transform;
                        newarea.GetComponent<Room>().objs.Add(wall);
                        wall.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                        wall.ControlerRoom = newarea.GetComponent<Room>();
                        wall.TargetVector = wall.transform.position;
                    }

                }
                else if (wallinformation[p] == red)
                {
                    Wall wall = Instantiate(ObjectData.Instance.UpperWall);
                    wall.gameObject.SetActive(true);
                    wall.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                    wall.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(wall);
                    wall.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    wall.ControlerRoom = newarea.GetComponent<Room>();
                    wall.TargetVector = wall.transform.position;
                }
                p++;
            }
        }

        p = 0;
        //문을 창조하는 코드
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                if (DoorInformation[p] == red)
                {
                    if (x == 0)
                    {
                        if(InGameArea[RoomCount].LeftDoor != null)
                        {
                            Destroy(InGameArea[RoomCount].LeftDoor.gameObject);
                        }
                        Door NewDoor = Instantiate(ObjectData.Instance.LeftDoor);
                        NewDoor.doortype = Doortype.Left;
                        NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.LeftdoorCloseSprite;
                        NewDoor.gameObject.SetActive(true);
                        NewDoor.transform.position = new Vector3(roominformation.RoomNumX * 200, 40 + (roominformation.RoomNumY * 100), 0);
                        //NewDoor.transform.localScale = new Vector3(10, 20);
                        NewDoor.gameObject.transform.parent = newarea.transform;
                        NewDoor.ControlerRoom = newarea.GetComponent<Room>();
                        InGameArea[RoomCount].LeftDoor = NewDoor;
                        NewDoor.TargetVector = NewDoor.transform.position;
                        if(newarea.GetComponent<Room>().roommoveterm == RoomMoveTerms.FREE)
                        {
                            NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.LeftdoorOpenSprite;
                            NewDoor.CanUsingThis = true;
                        }
                        if(newarea.GetComponent<Room>().w != null)
                        {
                            if (newarea.GetComponent<Room>().w.RightDoor.CanUsingThis == true)
                            {
                                NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.LeftdoorOpenSprite;
                                NewDoor.CanUsingThis = true;
                            }
                        }
                    }
                    else if (x == 19)
                    {
                        if (InGameArea[RoomCount].RightDoor != null)
                        {
                            Destroy(InGameArea[RoomCount].RightDoor.gameObject);
                        }
                        Door NewDoor = Instantiate(ObjectData.Instance.RightDoor);
                        NewDoor.doortype = Doortype.Right;
                        NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.RightdoorCloseSprite;
                        NewDoor.gameObject.SetActive(true);
                        NewDoor.transform.position = new Vector3(190 + (roominformation.RoomNumX * 200), 40 + (roominformation.RoomNumY * 100), 0);
                        //NewDoor.transform.localScale = new Vector3(10, 20);
                        NewDoor.gameObject.transform.parent = newarea.transform;
                        NewDoor.ControlerRoom = newarea.GetComponent<Room>();
                        InGameArea[RoomCount].RightDoor = NewDoor;
                        NewDoor.TargetVector = NewDoor.transform.position;
                        if (newarea.GetComponent<Room>().roommoveterm == RoomMoveTerms.FREE)
                        {
                            NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.RightdoorOpenSprite;
                            NewDoor.CanUsingThis = true;
                        }
                        if (newarea.GetComponent<Room>().e != null)
                        {
                            if (newarea.GetComponent<Room>().e.LeftDoor.CanUsingThis == true)
                            {
                                NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.RightdoorOpenSprite;
                                NewDoor.CanUsingThis = true;
                            }
                        }
                    }
                    else if(y == 0)
                    {
                        if (InGameArea[RoomCount].DownDoor != null)
                        {
                            Destroy(InGameArea[RoomCount].DownDoor.gameObject);
                        }
                        Door NewDoor = Instantiate(ObjectData.Instance.DownDoor);
                        NewDoor.doortype = Doortype.Down;
                        NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.DowndoorClosesSprite;
                        NewDoor.gameObject.SetActive(true);
                        NewDoor.transform.position = new Vector3(95 + (roominformation.RoomNumX * 200), (roominformation.RoomNumY * 100), 0);
                        NewDoor.transform.localScale = new Vector3(40, 10);
                        NewDoor.gameObject.transform.parent = newarea.transform;
                        NewDoor.ControlerRoom = newarea.GetComponent<Room>();
                        InGameArea[RoomCount].DownDoor = NewDoor;
                        NewDoor.TargetVector = NewDoor.transform.position;
                        if (newarea.GetComponent<Room>().roommoveterm == RoomMoveTerms.FREE)
                        {
                            NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.DowndoorOpenSprite;
                            NewDoor.CanUsingThis = true;
                        }
                        if (newarea.GetComponent<Room>().s != null)
                        {
                            if (newarea.GetComponent<Room>().s.UpDoor.CanUsingThis == true)
                            {
                                NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.DowndoorOpenSprite;
                                NewDoor.CanUsingThis = true;
                            }
                        }
                    }
                    else if(y == 9)
                    {
                        if (InGameArea[RoomCount].UpDoor != null)
                        {
                            Destroy(InGameArea[RoomCount].UpDoor.gameObject);
                        }
                        Door NewDoor = Instantiate(ObjectData.Instance.UpDoor);
                        NewDoor.doortype = Doortype.Up;
                        NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.UpdoorClseSprite;
                        NewDoor.gameObject.SetActive(true);
                        NewDoor.transform.position = new Vector3(95 + (roominformation.RoomNumX * 200), 90 + (roominformation.RoomNumY * 100), 0);
                        NewDoor.transform.localScale = new Vector3(40, 10);
                        NewDoor.gameObject.transform.parent = newarea.transform;
                        NewDoor.ControlerRoom = newarea.GetComponent<Room>();
                        InGameArea[RoomCount].UpDoor = NewDoor;
                        NewDoor.TargetVector = NewDoor.transform.position;
                        if (newarea.GetComponent<Room>().roommoveterm == RoomMoveTerms.FREE)
                        {
                            NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.UpdoorOpenSprite;
                            NewDoor.CanUsingThis = true;
                        }
                        if (newarea.GetComponent<Room>().n != null)
                        {
                            if (newarea.GetComponent<Room>().n.DownDoor.CanUsingThis == true)
                            {
                                NewDoor.GetComponent<SpriteRenderer>().sprite = PlayerControl.Instance.UpdoorOpenSprite;
                                NewDoor.CanUsingThis = true;
                            }
                        }
                    }
                }
                else if(DoorInformation[p] == blue)
                {
                    EasyDoor newdoor = Instantiate(ObjectData.Instance.easydoor);
                    newdoor.EventNum = roominformation.DownDoorEventNum;
                    newdoor.gameObject.SetActive(true);
                    newdoor.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                    newdoor.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(newdoor);
                    newdoor.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    newdoor.ControlerRoom = newarea.GetComponent<Room>();
                    newdoor.TargetVector = newdoor.transform.position;
                }
                p++;
            }
        }

        p = 0;
        //물체오브젝트를 창조하는 코드
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                //상자창조코드
                if (ObjectInformation[p] == blue)
                {
                    Drum newdrum = Instantiate(ObjectData.Instance.drum);
                    newdrum.gameObject.SetActive(true);
                    newdrum.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                    newdrum.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(newdrum);
                    newdrum.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    newdrum.ControlerRoom = newarea.GetComponent<Room>();
                    newdrum.TargetVector = newdrum.transform.position;
                    //상자와 함정이 같이있다면 겹쳐쓰겠다.!!!
                    if (Trapinformation[p] == pupple)
                    {
                        ElectricTrap electrictrap = Instantiate(ObjectData.Instance.electrictrap);
                        newdrum.blindObj = electrictrap.gameObject;
                        electrictrap.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                        electrictrap.gameObject.transform.parent = newarea.transform;
                        newarea.GetComponent<Room>().objs.Add(electrictrap);
                        electrictrap.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                        electrictrap.ControlerRoom = newarea.GetComponent<Room>();
                        electrictrap.TargetVector = electrictrap.transform.position;
                    }
                    //전류 지속도 마찬가지다!!
                    else if (Trapinformation[p] == yellow)
                    {
                        ElectricTrap electrictrap = Instantiate(ObjectData.Instance.Loopelectrictrap);
                        newdrum.blindObj = electrictrap.gameObject;
                        electrictrap.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                        electrictrap.gameObject.transform.parent = newarea.transform;
                        newarea.GetComponent<Room>().objs.Add(electrictrap);
                        electrictrap.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                        electrictrap.ControlerRoom = newarea.GetComponent<Room>();
                        electrictrap.TargetVector = electrictrap.transform.position;
                    }
                }
                else if(ObjectInformation[p] == green)
                {
                    EasyDrum newdrum = Instantiate(ObjectData.Instance.easydrum);
                    newdrum.gameObject.SetActive(true);
                    newdrum.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                    newdrum.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(newdrum);
                    newdrum.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    newdrum.ControlerRoom = newarea.GetComponent<Room>();
                    newdrum.TargetVector = newdrum.transform.position;
                    //만일 당기는 레버가 같은 타일에 존재한다면 예가 부서지면 당기는 타일이 창조될것이다.
                    if (SpecialObjinformation[p] == pupple)
                    {
                        ClearRabber clearrabber = Instantiate(ObjectData.Instance.clearrabber);
                        clearrabber.gameObject.SetActive(false);
                        clearrabber.CanActive = false;
                        clearrabber.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                        clearrabber.gameObject.transform.parent = newarea.transform;
                        newarea.GetComponent<Room>().objs.Add(clearrabber);
                        clearrabber.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                        clearrabber.ControlerRoom = newarea.GetComponent<Room>();
                        clearrabber.TargetVector = clearrabber.transform.position;
                        newdrum.InvisibleObj = clearrabber.gameObject;
                        newarea.GetComponent<Room>().clearrabeers.Add(clearrabber);
                    }
                    else if(SpecialObjinformation[p] == blue)
                    {
                        SecretSwitch secretswitch = Instantiate(ObjectData.Instance.secretswitch);
                        secretswitch.gameObject.SetActive(false);
                        secretswitch.CanActive = false;
                        secretswitch.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                        secretswitch.gameObject.transform.parent = newarea.transform;
                        newarea.GetComponent<Room>().objs.Add(secretswitch);
                        secretswitch.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                        secretswitch.ControlerRoom = newarea.GetComponent<Room>();
                        secretswitch.TargetVector = secretswitch.transform.position;
                        newdrum.InvisibleObj = secretswitch.gameObject;
                        newarea.GetComponent<Room>().secretswitch.Add(secretswitch);
                        newarea.GetComponent<Room>().secretswitchdown.Add(false);
                    }
                }
                //단서창조코드
                else if(ObjectInformation[p] == yellow)
                {
                    Clue newclue = Instantiate(ObjectData.Instance.clue);
                    newclue.gameObject.SetActive(true);
                    newclue.EventNum = roominformation.ClueEventNum;
                    newclue.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                    newclue.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(newclue);
                    newclue.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    newclue.ControlerRoom = newarea.GetComponent<Room>();
                    newclue.TargetVector = newclue.transform.position;
                }
                else if (ObjectInformation[p] == pupple)
                {
                    Clue newclue = Instantiate(ObjectData.Instance.paperclue);
                    newclue.gameObject.SetActive(true);
                    newclue.EventNum = roominformation.PaperClueEventNum;
                    newclue.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                    newclue.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(newclue);
                    newclue.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    newclue.ControlerRoom = newarea.GetComponent<Room>();
                    newclue.TargetVector = newclue.transform.position;
                }
                p++;
            }
        }
        p = 0;

        //적을 창조하는 코드
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                if (EnermyInformation[p] == green)
                {
                    EnermyCreate(newarea, 0 , roominformation.RoomNumX, roominformation.RoomNumY, x, y);
                }
                else if(EnermyInformation[p] == blue)
                {
                    EnermyCreate(newarea, 1, roominformation.RoomNumX, roominformation.RoomNumY, x, y);
                }
                else if (EnermyInformation[p] == yellow)
                {
                    EnermyCreate(newarea, 2, roominformation.RoomNumX, roominformation.RoomNumY, x, y);
                }
                else if (EnermyInformation[p] == pupple)
                {
                    EnermyCreate(newarea, 3, roominformation.RoomNumX, roominformation.RoomNumY, x, y);
                }
                else if(EnermyInformation[p] == mint)
                {
                    PlayerCreate(newarea, roominformation.RoomNumX, roominformation.RoomNumY, x, y);
                }
                p++;
            }
        }

        p = 0;
        //장치를 창조하는 코드
        int rabbercount = 0;
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                if (SpecialObjinformation[p] == green)
                {
                    Rabber rabber = Instantiate(ObjectData.Instance.rabber);
                    rabber.LaserNumber = roominformation.LaserRabberNum[rabbercount];
                    rabbercount++;
                    rabber.gameObject.SetActive(true);
                    rabber.LaserActive = true;
                    rabber.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                    rabber.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(rabber);
                    rabber.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    rabber.ControlerRoom = newarea.GetComponent<Room>();
                    rabber.TargetVector = rabber.transform.position;
                }
                else if (SpecialObjinformation[p] == yellow)
                {
                    Firstaid firstaid = Instantiate(ObjectData.Instance.firstaid);
                    firstaid.gameObject.SetActive(true);
                    firstaid.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                    firstaid.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(firstaid);
                    firstaid.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    firstaid.ControlerRoom = newarea.GetComponent<Room>();
                    firstaid.TargetVector = firstaid.transform.position;
                }
                else if (SpecialObjinformation[p] == blue)
                {
                    if (ObjectInformation[p] != green)
                    {
                        SecretSwitch secretswitch = Instantiate(ObjectData.Instance.secretswitch);
                        secretswitch.gameObject.SetActive(true);
                        secretswitch.CanActive = false;
                        secretswitch.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                        secretswitch.gameObject.transform.parent = newarea.transform;
                        newarea.GetComponent<Room>().objs.Add(secretswitch);
                        secretswitch.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                        secretswitch.ControlerRoom = newarea.GetComponent<Room>();
                        secretswitch.TargetVector = secretswitch.transform.position;
                        newarea.GetComponent<Room>().secretswitch.Add(secretswitch);
                        newarea.GetComponent<Room>().secretswitchdown.Add(false);
                    }
                }
                //당기는 레버 창조코드
                else if(SpecialObjinformation[p] == pupple)
                {
                    //만일부서지기 쉬운 상자가 같은 타일에 존재할경우 안만들겠다!!!
                    if (ObjectInformation[p] != green)
                    {
                        ClearRabber clearrabber = Instantiate(ObjectData.Instance.clearrabber);
                        clearrabber.gameObject.SetActive(true);
                        clearrabber.CanActive = false;
                        clearrabber.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                        clearrabber.gameObject.transform.parent = newarea.transform;
                        newarea.GetComponent<Room>().objs.Add(clearrabber);
                        clearrabber.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                        clearrabber.ControlerRoom = newarea.GetComponent<Room>();
                        clearrabber.TargetVector = clearrabber.transform.position;
                        newarea.GetComponent<Room>().clearrabeers.Add(clearrabber);
                    }
                }
                else if(SpecialObjinformation[p] == red)
                {
                    ObjectPrinter objecprinter = Instantiate(ObjectData.Instance.objectprinter);
                    objecprinter.gameObject.SetActive(true);
                    objecprinter.EventNum = roominformation.ObjPrintEventNum;
                    objecprinter.CanActive = false;
                    objecprinter.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                    objecprinter.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(objecprinter);
                    objecprinter.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    objecprinter.ControlerRoom = newarea.GetComponent<Room>();
                    objecprinter.TargetVector = objecprinter.transform.position;
                }
                p++;
            }
        }
        rabbercount = 0;
        p = 0;
        //함정을 창조하는 코드
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 20; x++)
            {
                //레이저 창조코드
                if (Trapinformation[p] == mint)
                {
                    Laser laser = Instantiate(ObjectData.Instance.laser_down);
                    laser.lasertype = LaserType.LASER_UP;
                    laser.gameObject.SetActive(true);
                    laser.transform.position = new Vector2((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100) - 6);
                    laser.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(laser);
                    laser.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    laser.ControlerRoom = newarea.GetComponent<Room>();
                    laser.TargetVector = laser.transform.position;
                }
                else if(Trapinformation[p] == black)
                {
                    Laser laser = Instantiate(ObjectData.Instance.laser_up);
                    laser.lasertype = LaserType.LASER_DOWN;
                    laser.gameObject.SetActive(true);
                    laser.transform.position = new Vector2((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100) - 2);
                    laser.gameObject.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().objs.Add(laser);
                    laser.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                    laser.ControlerRoom = newarea.GetComponent<Room>();
                    laser.TargetVector = laser.transform.position;

                    //레이저 컨트롤러 창조코드
                    LaserControler laserControler = Instantiate(ObjectData.Instance.LaserControler);
                    laserControler.ControlerNumber = roominformation.LaserControlerNum[rabbercount];
                    rabbercount++;
                    laserControler.gameObject.SetActive(true);
                    laserControler.transform.parent = newarea.transform;
                    newarea.GetComponent<Room>().LaserControler.Add(laserControler);

                    laserControler.EndLaser = laser;
                    for(int i = 0; i< newarea.GetComponent<Room>().objs.Count; i++)
                    {
                        if(newarea.GetComponent<Room>().objs[i].tag == "Laser")
                        {
                            if(newarea.GetComponent<Room>().objs[i].TargetVector.x == laser.TargetVector.x && newarea.GetComponent<Room>().objs[i].TargetVector.y != laser.TargetVector.y)
                            {
                                laserControler.StartLaser = newarea.GetComponent<Room>().objs[i].GetComponent<Laser>();
                            }
                        }
                    }
                }
                else if (Trapinformation[p] == pupple)
                {
                    //전류함정과 상자가 같은위치에 있을경우 상자밑에 숨기겠다!!!
                    if (ObjectInformation[p] == blue)
                    {

                    }
                    else
                    {
                        ElectricTrap electrictrap = Instantiate(ObjectData.Instance.electrictrap);
                        electrictrap.gameObject.SetActive(true);
                        electrictrap.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                        electrictrap.gameObject.transform.parent = newarea.transform;
                        newarea.GetComponent<Room>().objs.Add(electrictrap);
                        electrictrap.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                        electrictrap.ControlerRoom = newarea.GetComponent<Room>();
                        electrictrap.TargetVector = electrictrap.transform.position;
                    }
                }
                else if(Trapinformation[p] == yellow)
                {
                    //전류함정과 상자가 같은위치에 있을경우 상자밑에 숨기겠다!!!
                    if (ObjectInformation[p] == blue)
                    {

                    }
                    else
                    {
                        ElectricTrap electrictrap = Instantiate(ObjectData.Instance.Loopelectrictrap);
                        electrictrap.gameObject.SetActive(true);
                        electrictrap.transform.position = new Vector3((x * 10) + (roominformation.RoomNumX * 200), (y * 10) + (roominformation.RoomNumY * 100), 0);
                        electrictrap.gameObject.transform.parent = newarea.transform;
                        newarea.GetComponent<Room>().objs.Add(electrictrap);
                        electrictrap.ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
                        electrictrap.ControlerRoom = newarea.GetComponent<Room>();
                        electrictrap.TargetVector = electrictrap.transform.position;
                    }
                }
                p++;
            }
        }
        PlayInformation.Instance.PlayingRoom = InGameArea[0];
        //if (roominformation.RoomNumX == 0 && roominformation.RoomNumY == 0)
        //{
        //    PlayerCreate(newarea, roominformation.RoomNumX, roominformation.RoomNumY, 4, 4);
        //}
        if (InGameArea[0] != newarea)
        {
            newarea.gameObject.SetActive(false);
        }

        //룸이미지를 생성 및 위치
        GameObject aa = Instantiate(roominformation.RoomImage);
        newarea.GetComponent<Room>().myImage = aa;
        aa.transform.position = new Vector3(95 + (roominformation.RoomNumX * 200), 45 + (roominformation.RoomNumY * 100), 0);
        aa.SetActive(true);
        aa.transform.parent = newarea.transform;

        //고유이벤트 유무 확인
        newarea.GetComponent<Room>().Event = roominformation.eventcontrol;

        RoomCount++;
    }

    //룸을 연결시켜주는 코드
    public void TransferMyAreas(Room thisroom)
    {
        for(int i = 0; i< InGameArea.Count; i++)
        {
            if(InGameArea[i] != thisroom)
            {
                if(thisroom.x - 1 == InGameArea[i].x && thisroom.y == InGameArea[i].y)
                {
                    thisroom.w = InGameArea[i];
                    InGameArea[i].e = thisroom;
                }
                if (thisroom.x + 1 == InGameArea[i].x && thisroom.y == InGameArea[i].y)
                {
                    thisroom.e = InGameArea[i];
                    InGameArea[i].w = thisroom;
                }
                if (thisroom.x == InGameArea[i].x && thisroom.y - 1 == InGameArea[i].y)
                {
                    thisroom.s = InGameArea[i];
                    InGameArea[i].n = thisroom;
                }
                if (thisroom.x == InGameArea[i].x && thisroom.y + 1 == InGameArea[i].y)
                {
                    thisroom.n = InGameArea[i];
                    InGameArea[i].s = thisroom;
                }
            }
        }
    }

    //플레이어를 창조시키는 명령
    public void PlayerCreate(GameObject newarea, int RoomNumX, int RoomNumY, int x, int y)
    {
        GameObject NewPlayer = Instantiate(CharacterData.Instance.PlayerObj);
        NewPlayer.SetActive(true);
        NewPlayer.transform.position = new Vector3((x * 10) + (RoomNumX * 200), (y * 10) + (RoomNumY * 100), 0);
        NewPlayer.transform.parent = newarea.transform;
        NewPlayer.GetComponent<Player>().MyRoom = newarea.GetComponent<Room>();
    }
    
    //몬스터를 창조시키는 명령
    public void EnermyCreate(GameObject newarea, int CharNum, int RoomNumX, int RoomNumY, int x, int y)
    {
        GameObject NewEnermy = Instantiate(CharacterData.Instance.EnermyObj[CharNum]);
        NewEnermy.SetActive(true);
        NewEnermy.transform.position = new Vector3((x * 10) + (RoomNumX * 200), (y * 10) + (RoomNumY * 100), 0);
        NewEnermy.transform.parent = newarea.transform;
        NewEnermy.GetComponent<Enermy>().MyRoom = newarea.GetComponent<Room>();
        NewEnermy.GetComponent<Enermy>().ParentTile = newarea.GetComponent<Room>().AreaTiles[x + (y * 20)];
        newarea.GetComponent<Room>().enermies.Add(NewEnermy.GetComponent<Enermy>());
    }
}
