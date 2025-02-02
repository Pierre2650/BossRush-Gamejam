using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chariot : IControllerFinder
{
    public void chooseController(char t , GameObject boss, List<Tarot_Controllers> Controllers)
    {
        switch (t)
        {
            case 'A':
                boss.AddComponent<Chariot_ATK>();
                Chariot_ATK temp = boss.GetComponent<Chariot_ATK>();
                Controllers.Add(temp);

                //temp.enabled = false;
                break;


            case 'B':
               
                break;


            case 'M':
                
                break;



        }
    }
}
