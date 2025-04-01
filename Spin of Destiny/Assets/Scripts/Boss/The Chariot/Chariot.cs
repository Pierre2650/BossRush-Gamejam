using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chariot : IControllerFinder
{
    public void chooseController(char t , GameObject boss, List<MonoBehaviour> Controllers)
    {
        switch (t)
        {
            case 'A':
                Chariot_ATK tempA = boss.GetComponent<Chariot_ATK>();
                tempA.enabled = true;
                Controllers.Add(tempA);

                //temp.enabled = false;
                break;

            // case 'B':

            //   break;

            case 'M' :
                Chariot_MAP tempM = boss.GetComponent<Chariot_MAP>();
                tempM.enabled = true;
                Controllers.Add(tempM);
                break;


            default :
                Chariot_MAP tempD = boss.GetComponent<Chariot_MAP>();
                tempD.enabled = true;
                Controllers.Add(tempD);

                break;



        }
    }
}
