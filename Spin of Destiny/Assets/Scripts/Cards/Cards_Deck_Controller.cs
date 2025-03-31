using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Cards_Deck_Controller : MonoBehaviour
{
    public List<GameObject> cards;

    [Header("Playable Cards")]
    public Vector2[] playableCardsPos;
    public List<GameObject> playableCards;

    private int[] PlayableCardsSelected = { 0, 0, 0 };
    private int indexPlayableCardsSelected = 0;


    public  void spreadCards()
    {
        float i = 0;
        foreach (GameObject c in cards)
        {
            Single_Card_Animations_Controller temp = c.GetComponent<Single_Card_Animations_Controller>();
            temp.startSpread(i);
            i += 0.05f;
        }

    }



    public void switchToPlayableCards()
    {
       
        int randPlayablesCard;
        do
        {
            randPlayablesCard = Random.Range(0, 6);

        } while (playableCardAlreadyChoosen(randPlayablesCard, PlayableCardsSelected));

        PlayableCardsSelected[indexPlayableCardsSelected] = randPlayablesCard;
        


        GameObject playable = playableCards[randPlayablesCard];
        playable.transform.position = playableCardsPos[indexPlayableCardsSelected];
        playable.GetComponent<Card_Controller>().setPos = playableCardsPos[indexPlayableCardsSelected];

        indexPlayableCardsSelected++;


    }


    

    private bool playableCardAlreadyChoosen(int a, int[] temp)
    {
        for (int i = 0; i < temp.Length; i++)
        {
            if (a == temp[i])
            {
                return true;
            }
        }

        return false;


    }
}
