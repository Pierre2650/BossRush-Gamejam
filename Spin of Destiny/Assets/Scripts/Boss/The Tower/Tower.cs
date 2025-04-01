using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : IControllerFinder
{
    public void chooseController(char t, GameObject boss, List<MonoBehaviour> Controllers)
    {
        switch (t)
        {
            case 'A':
                Tower_ATK tempA = boss.GetComponent<Tower_ATK>();
                tempA.enabled = true;
                Controllers.Add(tempA);

                break;


           // case 'B':

            //    break;


            case 'M':
                Tower_MAP tempM = boss.GetComponent<Tower_MAP>();
                tempM.enabled = true;
                Controllers.Add(tempM);

               // temp.enabled = false;

                break;

            default:

                Tower_MAP temp = boss.GetComponent<Tower_MAP>();
                temp.enabled = true;
                Controllers.Add(temp);


                break;



        }
    }
}
