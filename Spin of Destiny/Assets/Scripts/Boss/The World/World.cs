using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : IControllerFinder
{
    public void chooseController(char t, GameObject boss, List<MonoBehaviour> Controllers)
    {
        switch (t)
        {
            case 'A':
                World_ATK tempA = boss.GetComponent<World_ATK>();
                tempA.enabled = true;
                Controllers.Add(tempA);

                break;


            //case 'B':

             //   break;


            case 'M':

                World_MAP tempM = boss.GetComponent<World_MAP>();
                tempM.enabled = true;
                Controllers.Add(tempM);

                //temp.enabled = false;

                break;

            default:
                World_MAP tempD = boss.GetComponent<World_MAP>();
                tempD.enabled = true;
                Controllers.Add(tempD);

                break;



        }
    }
}
