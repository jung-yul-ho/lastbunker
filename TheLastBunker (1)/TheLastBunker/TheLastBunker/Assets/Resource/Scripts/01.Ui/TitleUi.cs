using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleUi : MonoBehaviour
{
    bool active;
    bool Click;
    public Image TarImg;
    public Sprite newimage;
    public Text thelast;
    public Text L;
    public Text bunker;
    float color;
    float colorsupport;
    public Text ClickButton;
    public void Start()
    {
        Click = false;
        color = 0.0f;
        active = true;
        Screen.SetResolution(1920, 1080, true);
        StartCoroutine(BrightImgae());

    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && active == true && Click == false)
        {
            Click = true;
            StopAllCoroutines();
            StartCoroutine(GoStageMenu());
        }
    }

    public IEnumerator BrightImgae()
    {
        while(color < 1)
        {
            Color aa = new Color(color, color, color);
            thelast.color = aa;
            L.color = new Color(color, 0, 0);
            bunker.color = aa;
            TarImg.color = aa;
            color += 0.035f;
            yield return new WaitForSeconds(0.1f);
            if (color >= 1.0f)
            {
                active = true;
            }
        }
        StartCoroutine(ColorChangeClickButton());
        yield return new WaitForSeconds(2.0f);
    }

    public IEnumerator ColorChangeClickButton()
    {
        ClickButton.gameObject.SetActive(true);
        bool white = true;
        float whitecolor = 0;
        while(true)
        {
            if(white == true)
            {
                whitecolor += 0.1f;
                if (whitecolor >= 1.0f)
                {
                    white = false;
                }
            }
            else
            {
                whitecolor -= 0.1f;
                if(whitecolor <= 0.0f)
                {
                    white = true;
                }
            }
            ClickButton.color = new Color(255, 255, 255, whitecolor);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public IEnumerator GoStageMenu()
    {
        ClickButton.gameObject.SetActive(false);
        TarImg.sprite = newimage;
        L.color = new Color(0, 0, 0);
        thelast.color = new Color(0, 0, 0);
        bunker.color = new Color(0, 0, 0);
        TarImg.color = new Color(0, 0, 0);
        yield return new WaitForSeconds(1.0f);
        TarImg.color = new Color(1, 1, 1);
        L.color = new Color(1, 0, 0);
        thelast.color = new Color(1, 1, 1);
        bunker.color = new Color(1, 1, 1);
        yield return new WaitForSeconds(1.0f);
        DontDestroyOnLoad(Audio.Instance.gameObject);
        //스테이지 시작
        SceneManager.LoadScene("Stage_Scene");
        SceneManager.LoadScene("Data_Scene", LoadSceneMode.Additive);
    }
}
