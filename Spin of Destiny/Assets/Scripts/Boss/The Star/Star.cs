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
                //Star_ATK tempA = boss.GetComponent<Star_ATK>();
                //tempA.enabled = true;

                boss.AddComponent<Star_ATK>();
                Star_ATK tempA = boss.GetComponent<Star_ATK>();
                Controllers.Add(tempA);

                break;


           // case 'B':

            //    break;


            case 'M':
                //Star_MAP tempM = boss.GetComponent<Star_MAP>();
                //tempM.enabled = true;

                boss.AddComponent<Star_MAP>();
                Star_MAP tempM = boss.GetComponent<Star_MAP>();
                Controllers.Add(tempM);

                break;

            default:
                //Star_MAP tempD = boss.GetComponent<Star_MAP>();
                //tempD.enabled = true;

                boss.AddComponent<Star_MAP>();
                Star_MAP tempD = boss.GetComponent<Star_MAP>();
                Controllers.Add(tempD);

                break;



        }
    }
}
