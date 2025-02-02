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
                
                break;


            case 'B':

                break;


            case 'M':

                boss.AddComponent<World_MAP>();
                World_MAP temp = boss.GetComponent<World_MAP>();
                Controllers.Add(temp);

                //temp.enabled = false;

                break;



        }
    }
}
