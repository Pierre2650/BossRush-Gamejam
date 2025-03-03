using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector]public float speed;
    
    [HideInInspector]public float damage;
    
    [HideInInspector]public Rigidbody2D rb2d;
    
    [HideInInspector]public float accelerationRate;
    
    [HideInInspector]Animator animator;

    bool isDead = false;

    void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();   
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(!isDead)PlayerController.applyForceToSpeed(speed, transform.right.normalized, rb2d, accelerationRate);
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        Health enemyHealth = other.GetComponent<Health>();
        if(other.tag!="Player" && enemyHealth!=null){
            enemyHealth.takeDamage(damage);
            StartCoroutine("HitCoroutine");
        }
    }

    IEnumerator HitCoroutine(){
        speed = 0;
        accelerationRate = 100;
        rb2d.linearVelocity = Vector2.zero;
        isDead = true;

        animator.Play("bullet_Clip_2");
        yield return null;
        Destroy(gameObject, animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
    }

    
}