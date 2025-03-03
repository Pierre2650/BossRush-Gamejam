using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lovers : IControllerFinder
{
    public void chooseController(char t, GameObject boss, List<MonoBehaviour> Controllers)
    {
        switch (t)
        {
            case 'A':
                //boss.AddComponent<Lovers_ATK>();
                //Lovers_ATK temp = boss.GetComponent<Lovers_ATK>();
                //Controllers.Add(temp);

                //temp.enabled = false;
                break;


            case 'B':

                break;


            case 'M':

                break;



        }
    }
}
