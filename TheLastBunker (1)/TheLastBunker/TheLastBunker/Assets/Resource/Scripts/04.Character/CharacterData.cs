using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public GameObject PlayerObj;
    //체크 서포트를 위해 만듬 나중에 삭제할거임
    //public GameObject CheckSupport;
    public List<GameObject> EnermyObj;

    static CharacterData instance;
    public static CharacterData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharacterData>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<CharacterData>();
                }
            }
            return instance;
        }
    }
}
