using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricTrap : Object
{
    public bool Loop;
    public ParticleSystem ElectriclightOne;
    public ParticleSystem ElectriclightTwo;
    public void Start()
    {
        StartCoroutine(ActiveTrap());
    }
    public IEnumerator ActiveTrap()
    {
        ElectriclightOne.Play();
        ElectriclightTwo.Play();
        if (Loop == true)
        {

        }
        else
        {
            while (true)
            {
                ElectriclightOne.Play();
                ElectriclightTwo.Play();
                yield return new WaitForSeconds(3.0f);
                ElectriclightOne.Stop();
                ElectriclightTwo.Stop();
                yield return new WaitForSeconds(3.0f);
            }
        }
    }
}
