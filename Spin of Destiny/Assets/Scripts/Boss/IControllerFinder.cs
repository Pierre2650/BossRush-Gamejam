using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllerFinder
{
    void chooseController(char t, GameObject boss, List<Tarot_Controllers> Controllers);

}
