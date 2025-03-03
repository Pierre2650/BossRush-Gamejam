using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : IControllerFinder
{
    public void chooseController(char t, GameObject boss, List<MonoBehaviour> Controllers)
    {
        switch (t)
        {
            case 'A':
                boss.AddComponent<Star_ATK>();
                Star_ATK tempA = boss.GetComponent<Star_ATK>();
                Controllers.Add(tempA);

                break;


           // case 'B':

            //    break;


            case 'M':

                boss.AddComponent<Star_MAP>();
                Star_MAP tempM = boss.GetComponent<Star_MAP>();
                Controllers.Add(tempM);

                break;

            default:
                boss.AddComponent<Star_MAP>();
                Star_MAP tempD = boss.GetComponent<Star_MAP>();
                Controllers.Add(tempD);

                break;



        }
    }
}
