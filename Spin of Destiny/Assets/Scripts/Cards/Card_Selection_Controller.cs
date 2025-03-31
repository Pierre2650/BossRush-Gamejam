using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

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

    [Header("UI")]
    public AnimationCurve curve; 
    public Image obscureImage;

    [Header("Obscuring")]
    private float ObsElapsed = 0f;
    private float ObsDur = 0.3f;
    // Update is called once per frame
    void Update()
    {

        if (cards.Count == numberOfCards)
        {
            //startGame
            animationController.everythingBackInPlace();
            startGame();
        }


        if (Input.GetKeyDown(KeyCode.A))
        {

            obscure();

        }

    }

    public void obscure()
    {
        StartCoroutine(obscuring());
    }

    public void unObscure()
    {
        StartCoroutine(unObscuring());
    }

    private IEnumerator obscuring()
    {
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

    private IEnumerator unObscuring()
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
