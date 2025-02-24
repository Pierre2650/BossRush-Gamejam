using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : IControllerFinder
{
    public void chooseController(char t, GameObject boss, List<Tarot_Controllers> Controllers)
    {
        switch (t)
        {
            case 'A':
                boss.AddComponent<Tower_ATK>();
                Tower_ATK tempA = boss.GetComponent<Tower_ATK>();
                Controllers.Add(tempA);

                break;


           // case 'B':

            //    break;


            case 'M':
                boss.AddComponent<Tower_MAP>();
                Tower_MAP tempM = boss.GetComponent<Tower_MAP>();
                Controllers.Add(tempM);

               // temp.enabled = false;

                break;

            default:

                boss.AddComponent<Tower_MAP>();
                Tower_MAP temp = boss.GetComponent<Tower_MAP>();
                Controllers.Add(temp);


                break;



        }
    }
}
