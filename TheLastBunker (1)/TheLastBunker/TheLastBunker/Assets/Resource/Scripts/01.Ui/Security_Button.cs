using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Security_Button : MonoBehaviour
{
    public int Stage_num;

    public void StartGame()
    {
        PlayInformation.Instance.PlayStageLevel = Stage_num;
        if (Stage_num <= PlayInformation.Instance.MyStageLevel)
        {
            Stage_Controler.Instance.StartGame();
        }
        else if (Stage_num > PlayInformation.Instance.MyStageLevel)
        {
            MessageBox.Instance.ShowMessage("아직 선택할 수 없습니다");
        }
        else
        {
            Stage_Controler.Instance.StartGame();
        }
    }
}
