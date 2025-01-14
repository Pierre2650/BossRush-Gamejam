using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public GameObject Target;
    public Enemy_Controller controller;

    public SpriteRenderer mySprR;

    public void applyEffect()
    {
        this.transform.position = Target.transform.position;
        //Start Animation
        StartCoroutine(slashAnimation());
        controller.hurt(-2);

    }

    private IEnumerator slashAnimation()
    {
        mySprR.enabled = true;
        yield return new WaitForSeconds(1);
       
        mySprR.enabled = false;

    }

}
