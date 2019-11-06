using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stage_Controler : MonoBehaviour
{
    public Image tarimg;
    public Button CloseButton;
    public Vector3 MiddleVector;
    public List<Security_Button> securities;
    static Stage_Controler instance;
    public Sprite ChapterOnSprite;
    public Sprite ChapterOffSprite;
    public Sprite OnLock;
    public Sprite OffLock;
    public GameObject GameExitPannel;

    public static Stage_Controler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Stage_Controler>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<Stage_Controler>();
                }
            }
            return instance;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitFirstStep();
        }
    }

    public void ExitFirstStep()
    {
        GameExitPannel.SetActive(true);
    }

    public void ExitSecondStep()
    {
        SaveLoadControl.Instance.Save();
        Application.Quit();
    }

    public void BackToStage()
    {
        GameExitPannel.SetActive(false);
    }

    public void SettingStage()
    {
        for(int i = 0; i< 6; i++)
        {
            if(PlayInformation.Instance.MyStageLevel - 1 >= i)
            {
                securities[i].GetComponent<Image>().sprite = ChapterOnSprite;
            }
            else
            {
                securities[i].GetComponent<Image>().sprite = ChapterOffSprite;
            }
        }
    }

    public void StartGame()
    {
        PlayInformation.Instance.playstate = PlayState.PLAY_STATE_PLAY;
        DontDestroyOnLoad(RoomData.Instance);
        DontDestroyOnLoad(ObjectData.Instance);
        DontDestroyOnLoad(CharacterData.Instance);
        DontDestroyOnLoad(PlayInformation.Instance);
        SceneManager.LoadScene("UI_Scene");
        //SceneManager.LoadScene("Data_Scene", LoadSceneMode.Additive);
        SceneManager.LoadScene("Game_Scene", LoadSceneMode.Additive);
    }

    public void LevelReset()
    {
        PlayInformation.Instance.MyStageLevel = 1;
        PlayInformation.Instance.ModOpen = false;
        SettingStage();
    }
}
