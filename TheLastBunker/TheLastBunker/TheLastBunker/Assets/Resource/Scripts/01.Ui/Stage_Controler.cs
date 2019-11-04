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
    public List<Stage_Button> Rooms;
    public int SecurityLevel;
    public int StageLevel;
    static Stage_Controler instance;
    public Sprite ChapterOnSprite;
    public Sprite ChapterOffSprite;
    public Sprite OnLock;
    public Sprite OffLock;

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


    IEnumerator MoveButton(GameObject Target ,Vector2 endPos, float duration)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        Vector2 startPos = Target.transform.localPosition;

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            Target.transform.localPosition = Vector2.Lerp(startPos, endPos, elapsed / duration);

            yield return waitForEndOfFrame;
        }

        Target.transform.localPosition = endPos;
    }

    public void SelectSecurity(int SecurityNum)
    {
        SecurityLevel = SecurityNum;
        for(int i = 0; i<5; i++)
        {
            if (securities[i].Security_num != SecurityNum)
            {
                securities[i].gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(MoveButton(securities[i].gameObject, new Vector3(0, 500, 0), 1.0f));
                securities[i].GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);
            }
            Rooms[i].gameObject.SetActive(true);
        }
        CloseButton.gameObject.SetActive(true);
        RoomSetting();
    }

    public void StartGame()
    {
        PlayInformation.Instance.playstate = PlayState.PLAY_STATE_PLAY;
        DontDestroyOnLoad(RoomData.Instance);
        DontDestroyOnLoad(ObjectData.Instance);
        DontDestroyOnLoad(CharacterData.Instance);
        DontDestroyOnLoad(PlayInformation.Instance);
        PlayInformation.Instance.StageNum = StageLevel;
        SceneManager.LoadScene("UI_Scene");
        //SceneManager.LoadScene("Data_Scene", LoadSceneMode.Additive);
        SceneManager.LoadScene("Game_Scene", LoadSceneMode.Additive);
    }

    public void SecuritySetting()
    {
        for(int i = 0; i< 5; i++)
        {
            if(PlayInformation.Instance.SecurityLevel - 1 < i)
            {
                securities[i].GetComponent<Image>().sprite = ChapterOffSprite;
            }
        }
    }

    public void RoomSetting()
    {
        for (int i = 0; i < 5; i++)
        {
            if (PlayInformation.Instance.RoomLevel - 1 < i)
            {
                Rooms[i].GetComponent<Image>().sprite = ChapterOffSprite;
                Rooms[i].LockImg.sprite = OffLock;
            }
            if(PlayInformation.Instance.SecurityLevel > SecurityLevel)
            {
                Rooms[i].GetComponent<Image>().sprite = ChapterOnSprite;
            }
        }
    }

    public void CloseSelectStage()
    {
        CloseButton.gameObject.SetActive(false);
        for (int i = 0; i < 5; i++)
        {
            if(i == SecurityLevel - 1)
            {
                StartCoroutine(MoveButton(securities[i].gameObject, new Vector3(-760 + (i * 380) , 500, 0), 1.0f));
                //securities[i].GetComponent<RectTransform>().sizeDelta = new Vector2(300, 300);
            }
            securities[i].gameObject.SetActive(true);
            Rooms[i].gameObject.SetActive(false);
        }
        SecurityLevel = 0;
    }

    public void LevelReset()
    {
        PlayInformation.Instance.RoomLevel = 1;
        PlayInformation.Instance.SecurityLevel = 1;
        PlayInformation.Instance.ModOpen = false;
        SecuritySetting();
        RoomSetting();
    }
}
