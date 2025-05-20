using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{

    // à enlever 

    [Header("Init")]
    protected Health myHealth;
    protected Animator myAni ;

    public GameObject thePlayer;
    public GameObject Grid;
    public GameObject camera;
    public GameObject bossUI;


    [Header("Hit")]
    protected Coroutine hitCooldownC = null;

    private void Start()
    {
        myHealth = GetComponent<Health>();
        myAni = GetComponent<Animator>();
    }



    public void isHit( )
    {
        
        if (hitCooldownC == null)
        {
            myAni.SetTrigger("Hit");
            hitCooldownC = StartCoroutine(hitCooldown());
        }
    }

    protected IEnumerator hitCooldown()
    {
        yield return new WaitForSeconds(myHealth.invincibilityTime);

        hitCooldownC = null;

        if (myHealth.isDead)
        {
            Destroy(myHealth.healthBar.gameObject);
            Destroy(this.gameObject);
        }
    }
    public void reStartPos()
    {
        this.transform.position = new Vector2(-2.46f, 6.67f);
    }
    
}
