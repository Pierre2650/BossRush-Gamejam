using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Devil : IControllerFinder
{
    public void chooseController(char t, GameObject boss, List<Tarot_Controllers> Controllers)
    {
        switch (t)
        {
            case 'A':
                boss.AddComponent<Devil_ATK>();
                Devil_ATK tempA = boss.GetComponent<Devil_ATK>();
                Controllers.Add(tempA);

                //temp.enabled = false;
                break;


           // case 'B':

             //   break;


            case 'M':
                boss.AddComponent<Devil_MAP>();
                Devil_MAP tempM = boss.GetComponent<Devil_MAP>();
                Controllers.Add(tempM);

                break;

            default:
                boss.AddComponent<Devil_MAP>();
                Devil_MAP tempD = boss.GetComponent<Devil_MAP>();
                Controllers.Add(tempD);

                break;



        }
    }
}
