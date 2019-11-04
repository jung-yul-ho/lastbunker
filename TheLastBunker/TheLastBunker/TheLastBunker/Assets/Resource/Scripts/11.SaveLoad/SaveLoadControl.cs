using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;

public class SaveLoadControl : MonoBehaviour
{
    static SaveLoadControl instance;
    public static SaveLoadControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SaveLoadControl>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<SaveLoadControl>();
                }
            }
            return instance;
        }
    }

    public void Start()
    {
        Load();
    }

    public void Save()
    {
        SaveData mysavedata = new SaveData(PlayInformation.Instance.SecurityLevel, PlayInformation.Instance.RoomLevel, PlayInformation.Instance.ModOpen);

        JsonData savedata = JsonMapper.ToJson(mysavedata);

        //컴퓨터용 저장경로
        //File.WriteAllText(Application.dataPath + "/Resource/Savedata/11.json", savedata.ToString());

        //저장경로
        File.WriteAllText(Application.persistentDataPath + "/11.json", savedata.ToString());
    }

    //플레이어의 인벤토리및 장비, 게임정보를 불러오는 코드
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/11.json"))
        {
            string mysavedata = File.ReadAllText(Application.persistentDataPath + "/11.json");
            JsonData myjsondata = JsonMapper.ToObject(mysavedata);
            PlayInformation.Instance.SecurityLevel = int.Parse(myjsondata["SecurityLevel"].ToString());
            PlayInformation.Instance.RoomLevel = int.Parse(myjsondata["RoomLevel"].ToString());
            PlayInformation.Instance.ModOpen = bool.Parse(myjsondata["Mod_open"].ToString());
        }
        if(PlayInformation.Instance.SecurityLevel <= 0)
        {
            PlayInformation.Instance.SecurityLevel = 1;
        }
        if(PlayInformation.Instance.RoomLevel <= 0)
        {
            PlayInformation.Instance.RoomLevel = 1;
        }
        if (Stage_Controler.Instance != null)
        {
            Stage_Controler.Instance.SecuritySetting();
        }
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
