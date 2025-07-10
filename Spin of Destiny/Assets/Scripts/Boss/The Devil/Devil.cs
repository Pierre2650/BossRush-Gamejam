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
                //Devil_ATK tempA = boss.GetComponent<Devil_ATK>();
                //tempA.enabled = true;
                boss.AddComponent<Devil_ATK>();
                Devil_ATK tempA = boss.GetComponent<Devil_ATK>();
                Controllers.Add(tempA);

                //temp.enabled = false;
                break;


           // case 'B':

             //   break;


            case 'M':
                //Devil_MAP tempM = boss.GetComponent<Devil_MAP>();
                //tempM.enabled = true;
                boss.AddComponent<Devil_MAP>();
                Devil_MAP tempM = boss.GetComponent<Devil_MAP>();
                Controllers.Add(tempM);

                break;

            default:
                //Devil_MAP tempD = boss.GetComponent<Devil_MAP>();
                //tempD.enabled = true;

                boss.AddComponent<Devil_MAP>();
                Devil_MAP tempD = boss.GetComponent<Devil_MAP>();
                Controllers.Add(tempD);

                break;



        }
    }
}
