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


    public Card_Selection_Controller selectionController;

    public bool clickable = false;

    [Header("animations")]
    private bool onObject = false;
    //Curve for type f mouvement
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private AnimationCurve curveForStart;

    //handle sprites by searching them
    [Header("Animations")]
    //public float delay = 0;
    //private float startAnimationElapsed = 0;
    //private float startAnimationD = 0.2f;

    private float onHoverElapsedT = 0;
    private float onHoverDuration = 0.16f;

    private float flipAnimationElapsed = 0;
    private float flipAnimationD = 0.3f;

    private float onClickElapsed = 0;
    private float onClickAnimationD = 0.1f;

    private bool onHover = false;
    private Coroutine currentAnim;

    public Vector2 endPos;

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
           
            Card_Enum_Type temp = new Card_Enum_Type();
            temp.value = value;
            temp.type = type;

            selectionController.cards.Add(temp);

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

        //vote_scrpt.choiceMade(value);
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

    /*
    private IEnumerator startAnimation()
    {
        yield return new WaitForSeconds(delay);

        float percentageDur = 0;

        Vector2 start = transform.localPosition;



        while (startAnimationElapsed < startAnimationD)
        {

            percentageDur = startAnimationElapsed / startAnimationD;

            transform.localPosition = Vector2.Lerp(start, endPos, curveForStart.Evaluate(percentageDur));

            startAnimationElapsed += Time.deltaTime;
            yield return null;

        }

        startAnimationElapsed = 0;

        StartCoroutine(flipCard());



    }
    */


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
        //spriteComponent.sprite = valueCard[1];

        while (flipAnimationElapsed < flipAnimationD)
        {

            percentageDur = flipAnimationElapsed / flipAnimationD;

            transform.rotation = Quaternion.Lerp(end, start, curve.Evaluate(percentageDur));

            flipAnimationElapsed += Time.deltaTime;
            yield return null;

        }




        flipAnimationElapsed = 0;

        clickable = true;

    }

    //private IEnumerator endAnimation() { 
    //}


}