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
                Devil_ATK temp = boss.GetComponent<Devil_ATK>();
                Controllers.Add(temp);

                //temp.enabled = false;
                break;


            case 'B':

                break;


            case 'M':

                break;



        }
    }
}
