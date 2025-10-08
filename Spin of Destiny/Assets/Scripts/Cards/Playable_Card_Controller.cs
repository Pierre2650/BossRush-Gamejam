using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class Playable_Card_Controller : MonoBehaviour
{
    [Header("InitComponents")]
    private SpriteRenderer spriteComponent;
    private Image imageComponent;

    public Vector2 setPos;


    [Header("CardDefine")]
    public Enum_Card value;
    private char type;

    private Card_Enum_Type valueTypeTuple  = new Card_Enum_Type();


    public Card_Selection_Controller selectionController;

    [Header("Sprites")]
    public Sprite[] sprites;

    [Header("Text")]
    public GameObject child;

    [Header("Animations Parameters")]
    private bool onHover = false;
    public bool clickable = false;
    private bool onObject = false;
    private Coroutine currentAnim;
    public AnimationCurve curve;

    [Header("Hover Animations")]
    private Vector2 hoverStart;
    private Vector2 hoverEnd;
    private float onHoverElapsedT = 0;
    private float onHoverDuration = 0.16f;

    [Header("flip Animations")]
    private float flipAnimationElapsed = 0;
    private float flipAnimationD = 0.1f;

    [Header("Click Animations")]
    private float onClickElapsed = 0;
    private float onClickAnimationD = 0.1f;


    [Header("Zoom Animations")]
    private float zoomElapsed = 0;
    private float zoomAnimationD = 0.3f;


    private void OnEnable()
    {
        clickable = false;
        onObject = false;
    }
    private void Awake()
    {
        spriteComponent = GetComponent<SpriteRenderer>();
        imageComponent = GetComponent<Image>();

        hoverStart = transform.localScale;
        hoverEnd = new  Vector2(transform.localScale.x + 0.25f, transform.localScale.y + 0.25f);
        //hoverEnd = new Vector2(transform.localScale.x + 13f, transform.localScale.y + 13f);
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


        if (selectionController.cards.Count == selectionController.nbCards - 1 && selectionController.checkConditions('A'))
        {
            type = 'A';
            Debug.Log("Card Type changed to A = " + type);
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

        if (!onHover && currentAnim == null && clickable)
        {
            currentAnim = StartCoroutine(onHoverAnimation());
            onHover = true;
        }
    }

    void OnMouseExit()
    {
        onObject = false;

        if (onHover && clickable)
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
            //imageComponent.color = Color.Lerp(start, end, curve.Evaluate(percentageDur));

            onClickElapsed += Time.deltaTime;
            yield return null;

        }
        onClickElapsed = 0;

        while (onClickElapsed < onClickAnimationD)
        {

            percentageDur = onClickElapsed / onClickAnimationD;

            spriteComponent.color = Color.Lerp(end, start, curve.Evaluate(percentageDur));
            //imageComponent.color = Color.Lerp(end, start, curve.Evaluate(percentageDur));

            onClickElapsed += Time.deltaTime;
            yield return null;

        }
        onClickElapsed = 0;


        selectionController.obscure(this.gameObject);
        StartCoroutine(flipCard());
    }


    private IEnumerator onHoverAnimation()
    {


        float percentageDur = 0;

        while (onHoverElapsedT < onHoverDuration)
        {

            percentageDur = onHoverElapsedT / onHoverDuration;

            transform.localScale = Vector2.Lerp(hoverStart, hoverEnd, curve.Evaluate(percentageDur));

            onHoverElapsedT += Time.deltaTime;
            yield return null;

        }
        onHoverElapsedT = 0;

        currentAnim = null;


    }

    private IEnumerator offHoverAnimation()
    {

        float percentageDur = 0;


        while (onHoverElapsedT < onHoverDuration)
        {

            percentageDur = onHoverElapsedT / onHoverDuration;

            transform.localScale = Vector2.Lerp(hoverEnd, hoverStart, curve.Evaluate(percentageDur));

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
        //imageComponent.sprite = sprites[1];

        while (flipAnimationElapsed < flipAnimationD)
        {

            percentageDur = flipAnimationElapsed / flipAnimationD;

            transform.rotation = Quaternion.Lerp(end, start, curve.Evaluate(percentageDur));

            flipAnimationElapsed += Time.deltaTime;
            yield return null;

        }




        flipAnimationElapsed = 0;

        child.SetActive(true);

        StartCoroutine(zoom_CenterCard());
        

    }


    private IEnumerator zoom_CenterCard()
    {
        spriteComponent.sortingOrder = 2;
        float percentageDur = 0;

        Vector2 start = transform.localScale;
        Vector2 end = new Vector2(3.5f, 3.5f);

        Vector2 startPos = transform.position;
        Vector2 endPos = new Vector2(-0.25f, 0);




        while (zoomElapsed < zoomAnimationD)
        {

            percentageDur = zoomElapsed / zoomAnimationD;

            transform.localScale = Vector2.Lerp(start, end, curve.Evaluate(percentageDur));
            transform.position = Vector2.Lerp(startPos, endPos, curve.Evaluate(percentageDur));

            zoomElapsed += Time.deltaTime;
            yield return null;

        }

        zoomElapsed = 0;

        yield return new WaitForSeconds(2f);
        selectionController.unObscure(this.gameObject);
        StartCoroutine(zoomAwaycard());
        

    }

    private IEnumerator zoomAwaycard()
    {
        float percentageDur = 0;

        Vector2 start = transform.localScale;
        Vector2 end = new Vector2(1, 1);

        Vector2 startPos = transform.position;
        Vector2 endPos = setPos;



        while (zoomElapsed < zoomAnimationD)
        {

            percentageDur = zoomElapsed / zoomAnimationD;

            transform.localScale = Vector2.Lerp(start, end, curve.Evaluate(percentageDur));
            transform.position = Vector2.Lerp(startPos, endPos, curve.Evaluate(percentageDur));


            zoomElapsed += Time.deltaTime;
            yield return null;

        }

        zoomElapsed = 0;

        spriteComponent.sortingOrder = 1;


        selectionController.cards.Add(valueTypeTuple);

   

    }


    public void resetPlayableCard()
    {

        spriteComponent.sprite = sprites[0];
        this.transform.position = new Vector3(21, 0, 0);
        child.SetActive(false);

    }

}