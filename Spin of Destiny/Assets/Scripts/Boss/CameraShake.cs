using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public GameObject camera;
    private Transform cameraTransform;

    [Header("Shake Coroutine")]
    Coroutine dirShakeC = null;

    private float shakeElapsedT = 0f;
    public float shakeDur = 0.15f;

    private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);


    private void Start()
    {
        cameraTransform = camera.transform;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            //longCameraShake(0.5f);


        }
    }

    public void randomCameraShake()
    {
        StartCoroutine(randomShake());
        
    }

    public void longCameraShake(float duration)
    {
        StartCoroutine(longShake(duration));
    }


    private IEnumerator randomShake()
    {
        Vector3 start = cameraTransform.position;

        Vector2 random = Random.insideUnitCircle;
        random *= 0.6f; // distance
        Vector3 randomDir = new Vector3(random.x, random.y, start.z);

        Vector3 randomLastDir;

        float percentageDur = 0;

        while (shakeElapsedT < shakeDur / 3)
        {

            percentageDur = shakeElapsedT / (shakeDur / 3);

            cameraTransform.position = Vector3.Lerp(start, randomDir, curve.Evaluate(percentageDur));

            shakeElapsedT += Time.deltaTime;
            yield return null;

        }
       
        shakeElapsedT = 0;

        randomLastDir = randomDir;

        random = Random.insideUnitCircle;
        random *= 0.6f;
        randomDir = new Vector3(random.x, random.y, start.z);


        while (shakeElapsedT < shakeDur / 3)
        {

            percentageDur = shakeElapsedT / (shakeDur / 3);

            
            cameraTransform.position = Vector3.Lerp(randomLastDir, randomDir, curve.Evaluate(percentageDur));



            shakeElapsedT += Time.deltaTime;
            yield return null;

        }
       

        shakeElapsedT = 0;



        while (shakeElapsedT < shakeDur / 3)
        {

            percentageDur = shakeElapsedT / (shakeDur / 3);

            
            cameraTransform.position = Vector3.Lerp(randomDir, start, curve.Evaluate(percentageDur));

            shakeElapsedT += Time.deltaTime;
            yield return null;

        }

        cameraTransform.position = start;
        shakeElapsedT = 0;

    }

    private IEnumerator longShake(float dur)
    {
        float count = 0;
        bool temp = false;

        while (count < dur )
        {
            StartCoroutine(randomShake());
            yield return new WaitForSeconds(shakeDur);
            count+= shakeDur;

        }
    }

    public void directionalShake(char dir)
    {
        if (dirShakeC == null) { 
            dirShakeC = StartCoroutine(dirShake(dir));
        }

    }

    private IEnumerator dirShake(char dir )
    {
        float distance = 0.6f;
        Vector3 start = cameraTransform.position;
        Vector3 end = Vector3.zero;
        switch (dir)
        {
            case 'U':
                end = new Vector3(start.x, distance, start.z);
                break;
            case 'R':
                end = new Vector3(distance, start.y, start.z);
                break;
            case 'D':
                end = new Vector3(start.x, distance, start.z);
                break;
            case 'L':
                end = new Vector3(distance, start.y, start.z);
                break;
        }

        float percentageDur = 0;

        while (shakeElapsedT < shakeDur / 2)
        {

            percentageDur = shakeElapsedT / (shakeDur / 2);

            cameraTransform.position = Vector3.Lerp(start, end, curve.Evaluate(percentageDur));

            shakeElapsedT += Time.deltaTime;
            yield return null;

        }

        shakeElapsedT = 0;

        while (shakeElapsedT < shakeDur / 2)
        {

            percentageDur = shakeElapsedT / (shakeDur / 2);


            cameraTransform.position = Vector3.Lerp(end, start, curve.Evaluate(percentageDur));



            shakeElapsedT += Time.deltaTime;
            yield return null;

        }

        cameraTransform.position = start;

        shakeElapsedT = 0;

        dirShakeC = null;

    }

}
