using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth; 
    private float currentHealth;
    public HealthBar healthBar;

    void Start(){
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }
    public void takeDamage(float dmg){
        currentHealth -= dmg;
        StartCoroutine(healthBar.looseHealthRoutine(dmg));
        Debug.Log("" + currentHealth);
    }

}
