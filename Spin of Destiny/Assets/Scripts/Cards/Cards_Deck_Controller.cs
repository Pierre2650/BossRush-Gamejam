using System.Collections.Generic;
using UnityEngine;

public class Cards_Deck_Controller : MonoBehaviour
{

    public List<GameObject> cards;

    [Header("Playable Cards")]
    public Vector2[] playableCardsPos;
    public List<GameObject> playableCards;

    private int[] PlayableCardsSelected = { -1, -1, -1 };

    private int indexPlayableCardsSelected = 0;
    private int indexPos;


    private void OnEnable()
    {
        if (Card_Selection_Controller.round == 2)
        {
            indexPos = 1;
        }
        else
        {
            indexPos = 0;
        }

    }

    public void spreadCards()
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
            randPlayablesCard = Random.Range(0, 5);

        } while (playableCardAlreadyChoosen(randPlayablesCard, PlayableCardsSelected));

        PlayableCardsSelected[indexPlayableCardsSelected] = randPlayablesCard;
        


        GameObject playable = playableCards[randPlayablesCard];
        playable.transform.position = playableCardsPos[indexPos];
        playable.GetComponent<Playable_Card_Controller>().setPos = playableCardsPos[indexPos];
        playable.GetComponent<Playable_Card_Controller>().clickable = true;
        Debug.Log("Choosen Card: " + playable.GetComponent<Playable_Card_Controller>().value.ToString());
        indexPlayableCardsSelected++;
        indexPos++;


    }


    public void changeClickableState(bool state)
    {
        foreach(GameObject card in playableCards)
        {
            Playable_Card_Controller temp = card.GetComponent<Playable_Card_Controller>();
            temp.clickable = state;
        }
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

    public void backInPlace()
    {
        PlayableCardsSelected[0] = -1;
        PlayableCardsSelected[1] = -1;
        PlayableCardsSelected[2] = -1;

        indexPlayableCardsSelected = 0;

        foreach (GameObject card in playableCards)
        {
            card.GetComponent<Playable_Card_Controller>().resetPlayableCard();
        }
    }
}
