using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DownMessageBox : MonoBehaviour
{
    //대화상자를 사용하는 사람의 이름
    public Text Speaker;
    //대화상자 이미지
    public Image BoxImage;

    //변경할 변수
    public float delay;
    public float Skip_delay;
    public int cnt;

    //타이핑효과 변수
    public string[] fulltext;
    public int dialog_cnt;
    string currentText;

    //타이핑확인 변수
    public bool text_exit;
    public bool text_full;
    public bool text_cut;
    public bool speak_break;

    static DownMessageBox instance;
    public static DownMessageBox Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DownMessageBox>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<DownMessageBox>();
                }
            }
            return instance;
        }
    }

    public void starttyping()
    {
        Get_Typing(dialog_cnt, fulltext);
        if (EventControl.Instance.eventstep[EventControl.Instance.eventNum].Bubbletype[cnt] != SpeechbubbleType.NONE)
        {
            Vector3 pos = new Vector3(0, 0, 0);
            SpeechControler.Instance.AddSpeechBubble(Player.Instance.transform, EventControl.Instance.eventstep[EventControl.Instance.eventNum].SpeakBubble[cnt], EventControl.Instance.eventstep[EventControl.Instance.eventNum].Bubbletype[cnt], 3.0f, Color.white, pos);
        }
        PlayerControl.Instance.AttackButton.gameObject.SetActive(false);
        PlayerControl.Instance.SpecialOne.gameObject.SetActive(false);
        PlayerControl.Instance.SpecialTwo.gameObject.SetActive(false);
        PlayerControl.Instance.AttackButton.gameObject.SetActive(false);
        JoystickControler.Instance.gameObject.SetActive(false);
    }


    //모든 텍스트 호출완료시 탈출
    void Update()
    {
        if (text_exit == true)
        {
            gameObject.transform.parent.gameObject.SetActive(false);
        }
        if (Input.GetMouseButtonUp(0) && PlayerControl.Instance.playerstate != PlayerState.PLAYER_PAUSE)
        {
            End_Typing();
        }
    }

    //다음버튼함수
    public void End_Typing()
    {
        //이벤트가 보일때
        if (EventControl.Instance.eventCount < fulltext.Length - 1)
        {
            //text_full = true;
            PlayInformation.Instance.GameActive = false;
            PlayerControl.Instance.AttackButton.gameObject.SetActive(false);
            PlayerControl.Instance.SpecialOne.gameObject.SetActive(false);
            PlayerControl.Instance.SpecialTwo.gameObject.SetActive(false);
            PlayerControl.Instance.AttackButton.gameObject.SetActive(false);
            JoystickControler.Instance.gameObject.SetActive(false);
        }
        //이벤트가 안보일때
        else if(text_full == true)
        {
            //text_full = false;
            text_exit = true;
            StopAllCoroutines();
            PlayInformation.Instance.GameActive = true;
            PlayerControl.Instance.AttackButton.gameObject.SetActive(true);
            PlayerControl.Instance.SpecialOne.gameObject.SetActive(true);
            if (PlayInformation.Instance.ModOpen == true)
            {
                PlayerControl.Instance.SpecialTwo.gameObject.SetActive(true);
            }
            PlayerControl.Instance.AttackButton.gameObject.SetActive(true);
            JoystickControler.Instance.gameObject.SetActive(true);
            if(PlayInformation.Instance.PlayingRoom.roommoveterm == RoomMoveTerms.OBJECT_ACTIVE)
            {
                PlayInformation.Instance.StageClearCheck();
            }
        }

        //다음 텍스트 호출
        if (text_full == true)
        {
            StopAllCoroutines();
            EventControl.Instance.eventCount++;
            SpeechControler.Instance.DestroyAllBehaviour();
            cnt++;
            text_full = false;
            text_cut = false;
            StartCoroutine(ShowText(fulltext));
            if(cnt< fulltext.Length)
            {
                if (EventControl.Instance.eventstep[EventControl.Instance.eventNum].Bubbletype[cnt] != SpeechbubbleType.NONE)
                {
                    BoxImage.gameObject.SetActive(false);
                    Speaker.gameObject.SetActive(false);
                    Vector3 pos = new Vector3(0, 0, 0);
                    SpeechControler.Instance.AddSpeechBubble(Player.Instance.transform, EventControl.Instance.eventstep[EventControl.Instance.eventNum].SpeakBubble[cnt], EventControl.Instance.eventstep[EventControl.Instance.eventNum].Bubbletype[cnt], 3.0f, Color.white, pos);
                }
                else
                {
                    BoxImage.gameObject.SetActive(true);
                    Speaker.gameObject.SetActive(true);
                }
            }
        }
        //텍스트 타이핑 생략
        else
        {
            speak_break = true;
            //text_cut = true;
            if (EventControl.Instance.eventstep[EventControl.Instance.eventNum].Bubbletype[cnt] != SpeechbubbleType.NONE)
            {
                BoxImage.gameObject.SetActive(false);
                Speaker.gameObject.SetActive(false);
                Vector3 pos = new Vector3(0, 0, 0);
                SpeechControler.Instance.AddSpeechBubble(Player.Instance.transform, EventControl.Instance.eventstep[EventControl.Instance.eventNum].SpeakBubble[cnt], EventControl.Instance.eventstep[EventControl.Instance.eventNum].Bubbletype[cnt], 3.0f, Color.white, pos);
            }
            else
            {
                BoxImage.gameObject.SetActive(true);
                Speaker.gameObject.SetActive(true);
            }
        }
    }

    //텍스트 시작호출
    public void Get_Typing(int _dialog_cnt, string[] _fullText)
    {
        //재사용을 위한 변수초기화
        text_exit = false;
        text_full = false;
        text_cut = false;
        cnt = 0;

        //변수 불러오기
        dialog_cnt = _dialog_cnt;
        fulltext = new string[dialog_cnt];
        fulltext = _fullText;

        //타이핑 코루틴시작
        StartCoroutine(ShowText(fulltext));
    }

    public IEnumerator ShowText(string[] _fullText)
    {
        speak_break = false;
        //모든텍스트 종료
        if (cnt >= dialog_cnt && text_full == true)
        {
            StopCoroutine("showText");
        }
        else if(cnt < fulltext.Length)
        {

            Speaker.text = EventControl.Instance.eventstep[EventControl.Instance.eventNum].EventSpeakerman[cnt].ToString();
            //기존문구clear
            currentText = "";
            //타이핑 시작
            for (int i = 0; i < _fullText[cnt].Length; i++)
            {
                if(speak_break == true)
                {
                    currentText = _fullText[cnt].Substring(0, _fullText[cnt].Length);
                    this.GetComponent<Text>().text = currentText;
                    speak_break = false;
                    text_full = true;
                    break;
                }
                //타이핑중도탈출
                if (text_cut == true)
                {
                    break;
                }
                //단어하나씩출력
                currentText = _fullText[cnt].Substring(0, i + 1);
                this.GetComponent<Text>().text = currentText;
                yield return new WaitForSeconds(delay);
            }
            //탈출시 모든 문자출력
            //Debug.Log("Typing 종료");
            //this.GetComponent<Text>().text = _fullText[cnt];
            //yield return new WaitForSeconds(Skip_delay);

            //스킵_지연후 종료
            //Debug.Log("Enter 대기");
            text_full = true;
        }
    }
}
