using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public bool isDead{get;private set;}
    public float maxHealth; 
    private float currentHealth;
    public HealthBar healthBar;
    public float invincibilityTime;

    [HideInInspector] public bool isInvincible{private set;get;}

    void Start(){
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    public void takeDamage(float dmg){
        if(!isInvincible){
            StartCoroutine("InvicibilityCoroutine");
            currentHealth -= dmg;
            healthBar.enQueueRoutine(dmg);
            if(currentHealth < 0){isDead=true;}
        }
    }

    IEnumerator InvicibilityCoroutine(){
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

}
