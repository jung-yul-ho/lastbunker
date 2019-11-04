using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectViewCam : MonoBehaviour
{
    public GameObject TargetView;
    static EffectViewCam instance;
    public static EffectViewCam Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EffectViewCam>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<EffectViewCam>();
                }
            }
            return instance;
        }
    }

    private void Update()
    {
        if (PlayerControl.Instance.playerstate == PlayerState.PLAYER_VIEWAREA)
        {
            transform.position = Vector3.Lerp(transform.position, TargetView.transform.position, Time.deltaTime * 1.5f);
        }
        else if (PlayerControl.Instance.playerstate != PlayerState.PLAYER_DIE)
        {
            transform.position = Player.Instance.transform.position + new Vector3(0, 0, -150);
        }
    }

    public void SettingCamera()
    {
        transform.position = new Vector3(90 + (PlayInformation.Instance.PlayingRoom.x * 200), 85, -70 + (PlayInformation.Instance.PlayingRoom.y * 100));
        transform.eulerAngles = new Vector3(80, 0, 0);
        AreaViewCam.Instance.SettingCamera();
    }
}
