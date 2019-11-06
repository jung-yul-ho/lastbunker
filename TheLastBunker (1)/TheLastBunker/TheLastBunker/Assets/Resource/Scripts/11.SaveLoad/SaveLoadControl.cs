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
        SaveData mysavedata = new SaveData(PlayInformation.Instance.MyStageLevel, PlayInformation.Instance.ModOpen);

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
            PlayInformation.Instance.MyStageLevel = int.Parse(myjsondata["SecurityLevel"].ToString());
            PlayInformation.Instance.ModOpen = bool.Parse(myjsondata["Mod_open"].ToString());
        }
        if(PlayInformation.Instance.MyStageLevel <= 0)
        {
            PlayInformation.Instance.MyStageLevel = 1;
        }
        Stage_Controler.Instance.SettingStage();
    }

    private void OnApplicationQuit()
    {
        Save();
    }
}
