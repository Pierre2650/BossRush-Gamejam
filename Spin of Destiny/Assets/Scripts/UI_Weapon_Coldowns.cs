using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Weapon_Coldowns : MonoBehaviour
{
    [Header("Spear")]
    public Image spear;
    public Sprite[] spFrames;
    private int currentSpFrame = 0;

    [Header("Axe")]
    public Image axe;
    public Sprite[] axeFrames;
    private int currentAxeFrame = 0;


    [Header("ShotGun")]
    public Image shotGun;
    public Sprite[] shGFrames;
    private int currentShGFrame = 0;

    private Coroutine dashAnimationCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public void axeOnColdown(float coldownT)
    {
        coldownT = coldownT / axeFrames.Length;
        StartCoroutine(axeColdownAnimation(axe, coldownT));
    }

    private IEnumerator axeColdownAnimation(Image UiImage, float dur)
    {
        
        do
        {

            

            UiImage.sprite = axeFrames[currentAxeFrame];
            currentAxeFrame++;

            yield return new WaitForSeconds(dur);


        } while (currentAxeFrame <= 5);
        

        currentAxeFrame = 0;
        UiImage.sprite = axeFrames[currentAxeFrame];

    }

    public void shotGunOnColdown(float coldownT)
    {
        coldownT = coldownT / shGFrames.Length;
        StartCoroutine(shotGunColdownAnimation(shotGun, coldownT));
    }

    private IEnumerator shotGunColdownAnimation(Image UiImage , float dur)
    {
        bool end = true;
        yield return new WaitForSeconds(dur);


        if (currentShGFrame > 5)
        {

            currentShGFrame = 0;
            end = false;

        }

        UiImage.sprite = shGFrames[currentShGFrame];
        currentShGFrame++;



        if (end)
        {
            StartCoroutine(shotGunColdownAnimation(UiImage, dur));
        }


    }
}
