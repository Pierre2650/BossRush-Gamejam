using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : IControllerFinder
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
                boss.AddComponent<Tower_MAP>();
                Tower_MAP temp = boss.GetComponent<Tower_MAP>();
                Controllers.Add(temp);

               // temp.enabled = false;

                break;



        }
    }
}
