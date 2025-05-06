using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{

    public GameObject gameOver;

    [Header("Obscuring")]
    public Image obscureImage;
    public AnimationCurve curve;
    private float ObsElapsed = 0f;
    private float ObsDur = 0.3f;

  

    public void gameOverUI()
    {
        gameOver.SetActive(true);
        StartCoroutine(obscuring());
    }


    private IEnumerator obscuring()
    {
        float percentageDur = 0;


        Color start = new Color(0f, 0f, 0f, 0f); // black
        Color end = new Color(0f, 0f, 0f, 210 / 255f); // obscure

        while (ObsElapsed < ObsDur)
        {

            percentageDur = ObsElapsed / ObsDur;

            obscureImage.color = Color.Lerp(start, end, curve.Evaluate(percentageDur));

            ObsElapsed += Time.deltaTime;
            yield return null;

        }
        ObsElapsed = 0;

    }

}
