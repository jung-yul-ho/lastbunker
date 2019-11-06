using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundCam : MonoBehaviour
{
    public GameObject TargetView;
    static FollowCam instance;
    public static FollowCam Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FollowCam>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<FollowCam>();
                }
            }
            return instance;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (PlayerControl.Instance.playerstate == PlayerState.PLAYER_VIEWAREA)
        //{
        //    Vector2 aa = new Vector3(TargetView.transform.position.x, TargetView.transform.position.y, -10);
        //    transform.position = Vector3.Lerp(transform.position, aa, Time.deltaTime * 1.5f);
        //}
        //else if (PlayerControl.Instance.playerstate != PlayerState.PLAYER_DIE)
        //{
        //    transform.position = Player.Instance.transform.position + new Vector3(0, 0, -150);
        //}
    }

    public void SettingCamera()
    {
        transform.position = new Vector3(90 + (PlayInformation.Instance.PlayingRoom.x * 200), 85, -70 + (PlayInformation.Instance.PlayingRoom.y * 100));
        transform.eulerAngles = new Vector3(80, 0, 0);
        AreaViewCam.Instance.SettingCamera();
    }
}
