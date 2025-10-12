using UnityEngine;

public class Tower_MAP_Push : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            PlayerController playerController = other.GetComponent<PlayerController>();

              
            Vector2 knockbackDir = (playerController.transform.position - transform.position).normalized;
            playerController.startKnockBack(knockbackDir.normalized, 0.5f, 40 , 0.2f, 0.2f);
            
        }
    }
}
