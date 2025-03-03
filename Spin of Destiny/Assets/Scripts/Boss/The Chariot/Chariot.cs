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
                boss.AddComponent<Chariot_ATK>();
                Chariot_ATK tempA = boss.GetComponent<Chariot_ATK>();
                Controllers.Add(tempA);

                //temp.enabled = false;
                break;

            // case 'B':

            //   break;

            case 'M' :
                boss.AddComponent<Chariot_MAP>();
                Chariot_MAP tempM = boss.GetComponent<Chariot_MAP>();
                Controllers.Add(tempM);
                break;


            default :
                boss.AddComponent<Chariot_MAP>();
                Chariot_MAP tempD = boss.GetComponent<Chariot_MAP>();
                Controllers.Add(tempD);

                break;



        }
    }
}
