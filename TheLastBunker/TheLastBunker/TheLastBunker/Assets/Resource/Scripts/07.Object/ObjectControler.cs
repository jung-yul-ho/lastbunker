using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectControler : MonoBehaviour
{

    static ObjectControler instance;
    public static ObjectControler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ObjectControler>();
                if (instance == null)
                {
                    GameObject container = new GameObject("오류발생");
                    instance = container.AddComponent<ObjectControler>();
                }
            }
            return instance;
        }
    }
    
    public void DoorCreate(int x, int z, int MapX, int MapZ, DIRECTION direction)
    {

    }
}
