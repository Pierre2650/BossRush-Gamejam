using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class Damage
{
    public static void damageBox(Vector2 center, Vector2 halfExtents, float orientation, int layerMask, float damage){
        Collider2D collider = Physics2D.OverlapBox(center, halfExtents, orientation, layerMask);
        if(collider!= null && collider.tag == "Player"){
            collider.GetComponent<Health>().takeDamage(damage);
        }
    }

    public static void damageCircle(Vector2 center, float radius, int layerMask, float damage){
        Collider2D collider = Physics2D.OverlapCircle(center, radius,layerMask);

        if(collider!= null && collider.tag == "Player"){

            Health playerHealth = collider.GetComponent<Health>();

            if (!playerHealth.isInvincible)
            {
                PlayerController playerController = collider.GetComponent<PlayerController>();


                playerHealth.takeDamage(damage);
                playerController.isHit();
            }
        }
    }
}
