using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : IControllerFinder
{
    public void chooseController(char t, GameObject boss, List<Tarot_Controllers> Controllers)
    {
        switch (t)
        {
            case 'A':
                boss.AddComponent<World_ATK>();
                World_ATK tempA = boss.GetComponent<World_ATK>();
                Controllers.Add(tempA);

                break;


            //case 'B':

             //   break;


            case 'M':

                boss.AddComponent<World_MAP>();
                World_MAP tempM = boss.GetComponent<World_MAP>();
                Controllers.Add(tempM);

                //temp.enabled = false;

                break;

            default:
                boss.AddComponent<World_MAP>();
                World_MAP tempD = boss.GetComponent<World_MAP>();
                Controllers.Add(tempD);

                break;



        }
    }
}
