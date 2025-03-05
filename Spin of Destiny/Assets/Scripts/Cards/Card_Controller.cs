using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Card_Controller : MonoBehaviour
{
    [Header("InitComponents")]
    private SpriteRenderer spriteComponent;


    [Header("CardDefine")]
    public Enum_Card value;
    private char type;

    private Card_Enum_Type valueTypeTuple  = new Card_Enum_Type();


    public Card_Selection_Controller selectionController;

    [Header("Sprites")]
    public Sprite[] sprites;

    [Header("Animations Parameters")]
    private bool onHover = false;
    public bool clickable = false;
    private bool onObject = false;
    private Coroutine currentAnim;
    public AnimationCurve curve;

    [Header("Hover Animations")]
    private float onHoverElapsedT = 0;
    private float onHoverDuration = 0.16f;

    [Header("flip Animations")]
    private float flipAnimationElapsed = 0;
    private float flipAnimationD = 0.1f;

    [Header("Click Animations")]
    private float onClickElapsed = 0;
    private float onClickAnimationD = 0.1f;

  

    private void Awake()
    {
        spriteComponent = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        //clickable = true;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0) && onObject && clickable)
        {

            StartCoroutine(onClick());

            selectType();
           
            valueTypeTuple.value = value;
            valueTypeTuple.type = type;

            //selectionController.cards.Add(temp);

            clickable = false;

        }


     
    }


    private void selectType()
    {
        int temp = 0;

        do
        {
            generateType();
            temp++;

            if (temp > 50)
            {
                Debug.Log("SelectType loop force break");
                break;
            }

        }while (!selectionController.checkConditions(type));

        Debug.Log("Card Type = " + type);


        if (selectionController.cards.Count == selectionController.numberOfCards - 1 && selectionController.checkConditions('A'))
        {
            type = 'A';
            Debug.Log("Card Type changed to A " + type);
        }
    }


    private void generateType()
    {
        int rand = Random.Range(1, 4);

        switch (rand)
        {
            case 1:

                type = 'A';

                break;

            case 2:
                type = 'B';

                break;
            
            case 3:
                type = 'M';
                break;
        }
    }

    void OnMouseOver()
    {
        onObject = true;

        if (!onHover && currentAnim == null)
        {
            currentAnim = StartCoroutine(onHoverAnimation());
            onHover = true;
        }
    }

    void OnMouseExit()
    {
        onObject = false;

        if (onHover)
        {
            if (currentAnim != null)
            {
                StopCoroutine(currentAnim);
                currentAnim = null;
            }
            currentAnim = StartCoroutine(offHoverAnimation());
            onHover = false;
        }
    }
 

    private IEnumerator onClick()
    {

        float percentageDur = 0;

        Color start = new Color(1f, 1f, 1f); // White
        Color end = new Color(144f / 255f, 144f / 255f, 144f / 255f); // Gray

        while (onClickElapsed < onClickAnimationD)
        {

            percentageDur = onClickElapsed / onClickAnimationD;

            spriteComponent.color = Color.Lerp(start, end, curve.Evaluate(percentageDur));

            onClickElapsed += Time.deltaTime;
            yield return null;

        }
        onClickElapsed = 0;

        while (onClickElapsed < onClickAnimationD)
        {

            percentageDur = onClickElapsed / onClickAnimationD;

            spriteComponent.color = Color.Lerp(end, start, curve.Evaluate(percentageDur));

            onClickElapsed += Time.deltaTime;
            yield return null;

        }
        onClickElapsed = 0;

        StartCoroutine(flipCard());
    }


    private IEnumerator onHoverAnimation()
    {


        float percentageDur = 0;

        Vector2 start = new Vector2(0.95f, 0.95f);
        Vector2 end = new Vector2(1.2f, 1.2f);


        while (onHoverElapsedT < onHoverDuration)
        {

            percentageDur = onHoverElapsedT / onHoverDuration;

            transform.localScale = Vector2.Lerp(start, end, curve.Evaluate(percentageDur));

            onHoverElapsedT += Time.deltaTime;
            yield return null;

        }
        onHoverElapsedT = 0;

        currentAnim = null;


    }

    private IEnumerator offHoverAnimation()
    {

        float percentageDur = 0;

        Vector2 start = transform.localScale;
        Vector2 end = new Vector2(0.95f, 0.95f);


        while (onHoverElapsedT < onHoverDuration)
        {

            percentageDur = onHoverElapsedT / onHoverDuration;

            transform.localScale = Vector2.Lerp(start, end, curve.Evaluate(percentageDur));

            onHoverElapsedT += Time.deltaTime;
            yield return null;

        }

        onHoverElapsedT = 0;

        currentAnim = null;


    }

   


    private IEnumerator flipCard()
    {
        float percentageDur = 0;

        Quaternion start = Quaternion.Euler(0, 0, 0);
        Quaternion end = Quaternion.Euler(0, -90, 0);


        while (flipAnimationElapsed < flipAnimationD)
        {

            percentageDur = flipAnimationElapsed / flipAnimationD;

            transform.rotation = Quaternion.Lerp(start, end, curve.Evaluate(percentageDur));

            flipAnimationElapsed += Time.deltaTime;
            yield return null;

        }

        flipAnimationElapsed = 0;

        //change image
        spriteComponent.sprite = sprites[1];

        while (flipAnimationElapsed < flipAnimationD)
        {

            percentageDur = flipAnimationElapsed / flipAnimationD;

            transform.rotation = Quaternion.Lerp(end, start, curve.Evaluate(percentageDur));

            flipAnimationElapsed += Time.deltaTime;
            yield return null;

        }




        flipAnimationElapsed = 0;

        selectionController.cards.Add(valueTypeTuple);

    }

    


}