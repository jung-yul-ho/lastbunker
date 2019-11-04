using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stage_Button : MonoBehaviour
{
    public int Stage_num;
    public Image LockImg;

    public void StartGame()
    {
        Stage_Controler.Instance.StageLevel = Stage_num;
        if(Stage_Controler.Instance.SecurityLevel < PlayInformation.Instance.SecurityLevel)
        {
            Stage_Controler.Instance.StartGame();
        }
        else if(Stage_Controler.Instance.StageLevel > PlayInformation.Instance.RoomLevel)
        {
            MessageBox.Instance.ShowMessage("아직 선택할 수 없습니다");
        }
        else
        {
            Stage_Controler.Instance.StartGame();
        }
    }
}
