using System.Collections.Generic;
using UnityEngine;

public class Cards_Deck_Controller : MonoBehaviour
{
    public List<GameObject> cards;
   

    public  void spreadCards()
    {
        float i = 0;
        foreach (GameObject c in cards)
        {
            Test_Card_Mix_animation temp = c.GetComponent<Test_Card_Mix_animation>();
            temp.startSpread(i);
            i += 0.05f;
        }

    }
}
