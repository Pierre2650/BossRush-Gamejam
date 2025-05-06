using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool isDead{get;private set;}
    public float maxHealth; 
    private float currentHealth;
    public HealthBar healthBar;

    public float invincibilityTime;
    Coroutine invincibleC = null;

    [HideInInspector] public bool isInvincible{private set;get;}

    void Start(){
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
        isInvincible = false;
    }

    public void takeDamage(float dmg){
        if(invincibleC == null)
        {

            invincibleC = StartCoroutine("InvicibilityCoroutine");
            currentHealth -= dmg;
            healthBar.enQueueRoutine(dmg);



            if(currentHealth <= 0){isDead=true;}

        }
    }

    IEnumerator InvicibilityCoroutine(){
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
        invincibleC = null;
    }

}
