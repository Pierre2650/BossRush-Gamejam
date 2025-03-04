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
    [Header("Rotating Card")]
    public GameObject rotatingCards;
    private Test_Card_Rotation_Animations rtCardsController;

    [Header("Deck of Cards")]
    public GameObject deck;
    private Vector2 deckStartPos;
    private Cards_Deck_Controller deckController;

    public AnimationCurve curve;

    [Header("Playable Cards")]
    public Vector2[] playableCardsPos;
    public List<GameObject> playableCards;

    [Header("Move Right")]
    public float goRightDur;
    private float goRightElapsedT = 0;
    public AnimationCurve goRightCurve;

    [Header("Move to new Pos")]
    public float changePosDur;
    private float changePosElapsedT = 0;





    private List<GameObject> test = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        rtCardsController = rotatingCards.GetComponent<Test_Card_Rotation_Animations>();
        deckController = deck.GetComponent<Cards_Deck_Controller>();

        deckStartPos = deck.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {

            StartCoroutine(moveRight());
        }

        if (Input.GetKeyDown(KeyCode.X))
        {

            StartCoroutine(selectPlayableCards(3));
        }

        

        if (Input.GetKeyDown(KeyCode.C))
        {
            foreach(GameObject g in test)
            {
                Debug.Log("Transform. Euler angles Z = " + g.transform.eulerAngles.z);

            }
            
        }
    }



    private IEnumerator moveRight()
    {
        float percetageDur;

        Vector2 start = deck.transform.position;
        Vector2 end = new Vector2(-8f, -4f);

        

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

            yield return new WaitForSeconds(0.14f);

        }

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
        
        Vector2 pos = rtCardsController.circlePoints[21];

        rtCardsController.cards.Add(card);
        deckController.cards.Remove(card);

        Test_Card_Mix_animation tempCard = card.GetComponent<Test_Card_Mix_animation>();
        tempCard.posToGo = pos;
        tempCard.newParent = rotatingCards;

        tempCard.shrink();
        StartCoroutine(tempCard.moveToPos(pos,false));


    }

    private IEnumerator selectPlayableCards(int nbCards)
    {

        //yield return new WaitForSeconds(1.2f);
        int i = 0;

        
        int randIndex = Random.Range(0, 3);

        while (deckController.cards.Count < nbCards)
        {
            //make the pos  random
            cardToTable(playableCardsPos[i]);

            i++;

            yield return new WaitForSeconds(0.3f);

        }

    }


    private void cardToTable(Vector2 pos)
    {
        int randCard = Random.Range(0, deckController.cards.Count);
        int randExistingCard = Random.Range(0, playableCards.Count);


        GameObject playable = playableCards[randExistingCard];
        playable.transform.position = pos;
        playableCards.Remove(playable);

        GameObject card = rtCardsController.cards[randCard];

        deckController.cards.Add(card);
        test.Add(card);
        rtCardsController.cards.Remove(card);
        

        Test_Card_Mix_animation tempCard = card.GetComponent<Test_Card_Mix_animation>();
        tempCard.posToGo = pos;
        tempCard.newParent = deck;
        
        tempCard.increase();
        StartCoroutine(tempCard.moveToPos(pos,true));


    }




}
