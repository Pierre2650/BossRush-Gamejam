using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : IControllerFinder
{
    public void chooseController(char t, GameObject boss, List<MonoBehaviour> Controllers)
    {
        switch (t)
        {
            case 'A':
                Devil_ATK tempA = boss.GetComponent<Devil_ATK>();
                tempA.enabled = true;
                Controllers.Add(tempA);

                //temp.enabled = false;
                break;


           // case 'B':

             //   break;


            case 'M':
                Devil_MAP tempM = boss.GetComponent<Devil_MAP>();
                tempM.enabled = true;
                Controllers.Add(tempM);

                break;

            default:
                Devil_MAP tempD = boss.GetComponent<Devil_MAP>();
                tempD.enabled = true;
                Controllers.Add(tempD);

                break;



        }
    }
}
