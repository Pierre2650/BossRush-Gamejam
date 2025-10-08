using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Card_Selection_Controller : MonoBehaviour
{
    [Header("Selected Cards Manager")]
    public List<Card_Enum_Type> cards;
    public static int round = 1;
    public int nbCards = 1;

    [Header("Start Game")]
    public GameObject Game;
    public GameObject thisObj;


    [Header("Cards Animation Manager")]
    public Cards_Animations_Controller animationController;

    [Header("Cards Deck Controller")]
    public Cards_Deck_Controller deckController;

    [Header("UI")]
    public AnimationCurve curve; 
    public Image obscureImage;

    public GameObject StartUI;
    public GameObject roundCounter;

    [Header("Obscuring")]
    private float ObsElapsed = 0f;
    private float ObsDur = 0.3f;

 

    private void OnEnable()
    {
        roundCounter.transform.GetComponent<TMP_Text>().text = round.ToString();

        if (round == 1)
        {
            nbCards = 1;
        }
        else if (round == 2)
        {
            nbCards = 2;
        }
        else
        {
            nbCards = 3;
        }

        cards = new List<Card_Enum_Type>();

    }

    // Update is called once per frame
    void Update()
    {

        if (cards.Count == nbCards)
        {
            //startGame
            animationController.everythingBackInPlace();
            startGame();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            round = 1;
            SceneManager.LoadScene("SampleScene");
        }

    }

    public void obscure(GameObject toSkip)
    {
        StartCoroutine(obscuring(toSkip));
    }

    public void unObscure(GameObject toSkip)
    {
        StartCoroutine(unObscuring(toSkip));
    }

    private IEnumerator obscuring(GameObject toSkip)
    {
        deckController.changeClickableState(false, toSkip);
        float percentageDur = 0;


        Color start = new Color(0f, 0f, 0f,0f); // black
        Color end = new Color(0f, 0f, 0f, 210/255f); // obscure

        while (ObsElapsed < ObsDur)
        {

            percentageDur = ObsElapsed / ObsDur;

            obscureImage.color = Color.Lerp(start, end, curve.Evaluate(percentageDur));

            ObsElapsed += Time.deltaTime;
            yield return null;

        }
        ObsElapsed = 0;

    }

    private IEnumerator unObscuring(GameObject toSkip)
    {
        float percentageDur = 0;

        Color start = new Color(0f, 0f, 0f, 0f); // black
        Color end = new Color(0f, 0f, 0f, 210 / 255f); // obscure

        while (ObsElapsed < ObsDur)
        {

            percentageDur = ObsElapsed / ObsDur;

            obscureImage.color = Color.Lerp(end, start, curve.Evaluate(percentageDur));

            ObsElapsed += Time.deltaTime;
            yield return null;

        }
        ObsElapsed = 0;

        deckController.changeClickableState(true, toSkip);
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
             
    public void startAnimation()
    {

        animationController.startAnimations();
        StartUI.SetActive(false);

    }
    private void startGame()
    {
        round++;

        Game.SetActive(true);
        thisObj.SetActive(false);
    }



}
