using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Falling_Star_Controller : MonoBehaviour
{

    private float fallElapsedT = 0;
    private float fallDuration = 1f;
    private AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f),  // Start key with flat tangent
                                                      new Keyframe(1f, 1f, 2f, 0f)   // End key with steep tangent
                                                       );

    private Vector2 fallZone;
    public bool fallEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //StartCoroutine(Fall());

        }
        
    }

    public void makeItFall()
    {
        StartCoroutine(Fall());

    }

    private IEnumerator Fall()
    {

        float percentageDur = 0;

        Vector2 start = transform.position;
        Vector2 end = transform.parent.position;


        while (fallElapsedT < fallDuration)
        {

            percentageDur = fallElapsedT / fallDuration;

            transform.position = Vector2.Lerp(start, end, curve.Evaluate(percentageDur));

            fallElapsedT += Time.deltaTime;
            yield return null;

        }
        fallElapsedT = 0;

        fallEnd = true;

    }
}
