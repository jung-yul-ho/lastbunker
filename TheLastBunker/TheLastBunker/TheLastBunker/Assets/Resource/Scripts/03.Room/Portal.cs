using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Portal : MonoBehaviour
{
    public List<Room> Controlportal;


    private void OnTriggerEnter(Collider other)
    {
        MoveArea();
    }

    public void MoveArea()
    {

    }
}
