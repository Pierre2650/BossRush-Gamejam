using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Axe : Weapon
{
    
    public Collider2D box; 
     public GameObject spear;
    private AudioSource axeSFX;
    void OnEnable()
    {
        box = GetComponent<Collider2D>();
        axeSFX = GetComponent<AudioSource>();
        box.enabled = false;
        animator = GetComponent<Animator>();
        startPos = transform.position;
        
    } 
    
    public override void attack(InputAction.CallbackContext context){
        StartCoroutine("AttackCoroutine");
    }

    public IEnumerator AttackCoroutine(){
        transform.right = direction;	
        box.enabled = true;
        float pitch = Random.Range(0.8f, 1.2f);
        axeSFX.pitch = pitch;
        axeSFX.Play();
        animator.Play("axe_Clip");
        yield return null;
        print(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        float waitingTime = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;//animator.GetCurrentAnimatorStateInfo(0).length;
        float t = 0;
        while(t< waitingTime){
            yield return null;
            t+=Time.deltaTime;
        }
        box.enabled = false;
        spear.SetActive(true);
        gameObject.SetActive(false);
        wp.canAttack = true;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Health enemyHealth = other.GetComponent<Health>();
        Enemy_Controller enemyController = other.GetComponent<Enemy_Controller>();
        if (other.tag!="Player"&& enemyHealth!=null){
            enemyHealth.takeDamage(damage);

            if (enemyController != null)
            {
                enemyController.isHit();
            }
        }
    }
}