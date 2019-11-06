using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    static Audio instance;
    public static Audio Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Audio>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<Audio>();
                }
            }
            return instance;
        }
    }
}
