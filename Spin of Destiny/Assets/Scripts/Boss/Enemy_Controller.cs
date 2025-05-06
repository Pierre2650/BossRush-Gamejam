using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{

    // à enlever 

    [Header("Init")]
    private Health myHealth;
    private Animator myAni;

    public GameObject thePlayer;
    public GameObject Grid;
    public GameObject camera;


    [Header("Hit")]
    private Coroutine hitCooldownC = null;

    private void Start()
    {
        myHealth = GetComponent<Health>();
        myAni = GetComponent<Animator>();
    }



    public void isHit()
    {
        
        if (hitCooldownC == null)
        {
            myAni.SetTrigger("Hit");
            hitCooldownC = StartCoroutine(hitCooldown());
        }
    }

    private IEnumerator hitCooldown()
    {
        yield return new WaitForSeconds(myHealth.invincibilityTime);

        hitCooldownC = null;

    }
    public void reStartPos()
    {
        this.transform.position = new Vector2(-2.46f, 6.67f);
    }
    
}
