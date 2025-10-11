using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Controller : MonoBehaviour
{

    // à enlever 

    [Header("Init")]
    protected Health myHealth;
    protected Animator myAni ;
    protected bool mainHealth = false;

    public GameObject thePlayer;
    public GameObject Grid;
    public GameObject camera;
    public GameObject bossUI;

    public GameObject victory_controller;

    private BoxCollider2D myBxC;
    protected AudioSource hurtSFX = null;

    public List<GameObject> spawnedAttacks = new List<GameObject>();
    


    [Header("Hit")]
    protected Coroutine hitCooldownC = null;

    private void Start()
    {
        myBxC = GetComponent<BoxCollider2D>();
        myHealth = GetComponent<Health>();
        myAni = GetComponent<Animator>();
        hurtSFX = GetComponent<AudioSource>();
        mainHealth = true;
    }



    public void isHit( )
    {

        if (myHealth.isDead && mainHealth) 
        { 
            myAni.SetBool("Dead", true);
            myBxC.enabled = false; 
        }

        if (hitCooldownC == null)
        {
            myAni.SetTrigger("Hit");
            hitCooldownC = StartCoroutine(hitCooldown());

            if (hurtSFX != null)
            {
                float pitch = Random.Range(1.8f, 2.3f);
                hurtSFX.pitch = pitch;
                hurtSFX.Play();
            }

        }
    }

    protected IEnumerator hitCooldown()
    {
        yield return new WaitForSeconds(myHealth.invincibilityTime);

        hitCooldownC = null;

        if (myHealth.isDead)
        {
            if (mainHealth)
            {

                destroySpawnedObjects();
                victory_controller.GetComponent<Round_Victory_Controller>().victory();

            }
            else
            {
                Destroy(myHealth.healthBar.gameObject);
                Destroy(this.gameObject);

            }

        }
    }

    private void destroySpawnedObjects()
    {
        int nbParentChilds = transform.parent.childCount;
        int nbChilds = transform.childCount;

        for (int i = 1; i < nbParentChilds; i++)
        {
            Destroy(transform.parent.GetChild(i).gameObject);
        }

        for (int i = 0; i < nbChilds; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

    }

    public void RestartAnim()
    {
        myAni.SetBool("Dead", false);
        myAni.SetTrigger("Hit");
        
    }
    public void RestartPos()
    {
        this.transform.position = new Vector2(-1.38f, 6.43f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && myBxC.IsTouching(collision.GetComponent<Collider2D>()))
        {

            Health playerHealth = collision.GetComponent<Health>();
            PlayerController playerController = collision.GetComponent<PlayerController>();

            if (!playerHealth.isInvincible  )
            {
                
                playerHealth.takeDamage(5);
                playerController.isHit();
                
                Vector2 knockbackDir = (playerController.transform.position - transform.position).normalized;
                playerController.startKnockBack(knockbackDir.normalized, 0.5f, 20, 0.2f, 0.2f);
            }
        }
    }

}
