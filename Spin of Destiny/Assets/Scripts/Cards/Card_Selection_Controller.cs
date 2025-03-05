using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Selection_Controller : MonoBehaviour
{
    [Header("Selected Cards Manager")]
    public List<Card_Enum_Type> cards = new List<Card_Enum_Type>();
    public int numberOfCards = 3;

    [Header("Start Game")]
    public GameObject Game;
    public GameObject thisObj;


    [Header("Cards Animation Manager")]
    public Cards_Animations_Controller animationController;

    // Update is called once per frame
    void Update()
    {

        if (cards.Count == numberOfCards)
        {
            //startGame
            animationController.everythingBackInPlace();
            startGame();
        }
        
        
    }

   


    public bool checkConditions(char t)
    {
        bool result = true;
        result = checkIfATKExist(t);

        return result;
    }


    private bool checkIfATKExist(char t)
    {
        foreach (Card_Enum_Type card in cards)
        {
            if (card.type == 'A' && t == 'A')
            {
                return false;
            }
        }

        return true;

    }
             

    private void startGame()
    {
        Game.SetActive(true);
        thisObj.SetActive(false);
    }



}
