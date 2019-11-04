using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EventActiveType
{
    MONSTER_KILL,
    ROOM_ENTER,
    GET_ITEM
}

[System.Serializable]
public class EventStep
{
    public string[] EventSpeakerman;
    public string[] EventSpeak;
    public string[] SpeakBubble;
    public List<int> Targetx;
    public List<int> Targety;
    public EventActiveType eventactivetype;
    public List<SpeechbubbleType> Bubbletype;
    
}

public class EventControl : MonoBehaviour
{
    public int eventNum;
    public int eventCount;
    public List<EventStep> eventstep;
    public GameObject dowmmessagebox;

    static EventControl instance;
    public static EventControl Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EventControl>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<EventControl>();
                }
            }
            return instance;
        }
    }

    public void StartEvent(int EventNum)
    {
        eventNum = EventNum;
        dowmmessagebox.SetActive(true);
        Player.Instance.PlayerTargetTransform = Player.Instance.transform.position;
        PlayInformation.Instance.GameActive = false;
        StopAllCoroutines();
        eventCount = 0;
        DownMessageBox.Instance.GetComponent<Text>().text = "";
        DownMessageBox.Instance.cnt = 0;
        DownMessageBox.Instance.fulltext = eventstep[eventNum].EventSpeak;
        DownMessageBox.Instance.dialog_cnt = eventstep[eventNum].EventSpeak.Length;
        DownMessageBox.Instance.starttyping();
    }
}
