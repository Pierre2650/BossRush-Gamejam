using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.U2D.ScriptablePacker;
using static UnityEngine.Rendering.GPUSort;

public class Cards_Animations_Controller : MonoBehaviour
{

    private Card_Selection_Controller cardsSelectionC;

    [Header("Rotating Card")]
    public GameObject rotatingCards;
    private Cards_Rotation_Animation_Controller rtCardsController;

    [Header("Deck of Cards")]
    public GameObject deck;
    private Vector2 deckStartPos;
    private Cards_Deck_Controller deckController;

    public AnimationCurve curve;

    [Header("Move Right")]
    public float goRightDur;
    private float goRightElapsedT = 0;
    public AnimationCurve goRightCurve;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardsSelectionC = GetComponent<Card_Selection_Controller>();
        rtCardsController = rotatingCards.GetComponent<Cards_Rotation_Animation_Controller>();
        deckController = deck.GetComponent<Cards_Deck_Controller>();
        deckStartPos = deck.transform.position;
        
    }


    private void OnEnable()
    {
        if (Card_Selection_Controller.round > 1)
        {
            startAnimations();
        }
    }

 


    public void startAnimations()
    {
        StartCoroutine(moveRight());

    }


    private IEnumerator moveRight()
    {
        float percetageDur;

        Vector2 start = deck.transform.position;
        Vector2 end = new Vector2(-8f, -5.2f);

        

        while (goRightElapsedT < goRightDur)
        {
            percetageDur = goRightElapsedT / goRightDur;

            deck.transform.position = Vector3.Lerp(start, end, goRightCurve.Evaluate(percetageDur));


            goRightElapsedT += Time.deltaTime;

            yield return null;

        }


        goRightElapsedT = 0f;


        yield return new WaitForSeconds(0.8f);

        deckController.spreadCards();

        StartCoroutine(MoveDeckToRotation());
    }

    private IEnumerator MoveDeckToRotation()
    {

        yield return new WaitForSeconds(1.2f);


        while (deckController.cards.Count > 0 )
        {

            deckToRotation();

            yield return new WaitForSeconds(0.13f);

        }

        StartCoroutine(selectPlayableCards(cardsSelectionC.nbCards));
    }

    private void deckToRotation()
    {
        int rand;

        if (deckController.cards.Count > 1) { 
            rand = Random.Range(0, deckController.cards.Count);
        }
        else
        {
            rand = 0;
        }
        
        GameObject card = deckController.cards[rand];
        
        Vector2 pos = rtCardsController.cardAdditionPos;

        rtCardsController.cards.Add(card);
        deckController.cards.Remove(card);

        Single_Card_Animations_Controller tempCard = card.GetComponent<Single_Card_Animations_Controller>();
        tempCard.posToGo = pos;
        tempCard.newParent = rotatingCards;

        tempCard.shrink();
        StartCoroutine(tempCard.moveToPos(pos,false));


    }

    private IEnumerator selectPlayableCards(int nbCards)
    {

        yield return new WaitForSeconds(2f);
        int i = 0;

        if (nbCards == 2)
        {
            i++;
        }

        while (deckController.cards.Count < nbCards)
        {
            //make the pos  random
            cardToTable(deckController.playableCardsPos[i]);

            i++;

            yield return new WaitForSeconds(0.3f);

        }



        StartCoroutine(stopRotation());

    }

    private IEnumerator stopRotation()
    {

        rtCardsController.stopRoation();

        yield return new WaitForSeconds(1f);


        int i = 0;
        List<int> temp = new List<int>();




        while (i < rtCardsController.cards.Count)
        {
            //make the pos  random

            temp.Add(cardsRearrengeEndRotation(temp));

            i++;

           // yield return new WaitForSeconds(0.1f);

        }

        rotatingCards.GetComponent<PickableObj_Mouvement>().enabled = true;


    }


    private int cardsRearrengeEndRotation(List<int> temp)
    {
        int randCard = 0;
        int debug = 0;

        do
        {
            if(debug > 120)
            {
                Debug.Log("boucle infini on cardsRearrengeEndRotation()....  breaking out");
                break;
            }

            randCard = Random.Range(0, rtCardsController.cards.Count);

            debug++;
        } while (temp.Contains(randCard));

        GameObject card = rtCardsController.cards[randCard];

        Single_Card_Animations_Controller tempCard = card.GetComponent<Single_Card_Animations_Controller>();


        Vector2 pos = new Vector2(-0.15f, 5f);
        StartCoroutine(tempCard.moveToPos(pos, false));
        StartCoroutine(tempCard.correctLocalRotation());

        return randCard;


    }
   


    private void cardToTable(Vector2 pos)
    {
        int randCard = Random.Range(0, deckController.cards.Count);

       
        GameObject card = rtCardsController.cards[randCard];

        deckController.cards.Add(card);
        rtCardsController.cards.Remove(card);
        

        Single_Card_Animations_Controller tempCard = card.GetComponent<Single_Card_Animations_Controller>();
        tempCard.posToGo = pos;
        tempCard.newParent = deck;
        
        tempCard.increase();
        StartCoroutine(tempCard.moveToPos(pos,true));


    }




    public void everythingBackInPlace()
    {
        //1. Deck back to start position
        //2. all the cards on Deck Game object
        //3. all the cars size = 1;
        deck.transform.position = deckStartPos;

        foreach(GameObject card in rtCardsController.cards)
        {
            card.transform.parent = deck.transform;
            deckController.cards.Add(card);
        }
        rtCardsController.cards.Clear();


        foreach (GameObject card in deckController.cards)
        {
            card.transform.eulerAngles = Vector3.zero;
            card.transform.position = deckStartPos;
            card.transform.localScale = Vector3.one;
            card.SetActive(true);
        }

        deckController.backInPlace();



    }




}
