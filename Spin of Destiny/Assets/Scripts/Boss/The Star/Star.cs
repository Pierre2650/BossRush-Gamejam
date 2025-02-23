using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : IControllerFinder
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

                boss.AddComponent<Star_MAP>();
                Star_MAP temp = boss.GetComponent<Star_MAP>();
                Controllers.Add(temp);

                break;



        }
    }
}
