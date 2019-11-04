using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Security_Button : MonoBehaviour
{
    public int Security_num;

    public void Show()
    {
        if(Security_num > PlayInformation.Instance.SecurityLevel)
        {
            MessageBox.Instance.ShowMessageForTitle("아직 선택할수 없습니다");
        }
        else
        {
            Stage_Controler.Instance.SelectSecurity(Security_num);
        }
    }


}
