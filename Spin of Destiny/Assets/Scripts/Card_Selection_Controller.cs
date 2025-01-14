using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card_Selection_Controller : MonoBehaviour
{
    public List<Enum_Card> cards = new List<Enum_Card>();
    public int numberOfCards = 3;

    //Start Game
    public GameObject Game;
    public GameObject thisObj;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (cards.Count == numberOfCards)
        {
            //startGame
            startGame();
        }
        
        
    }

    public void addCard(Enum_Card card)
    {
        cards.Add(card);
    }
             

    private void startGame()
    {
        Game.SetActive(true);
        thisObj.SetActive(false);
    }



}
