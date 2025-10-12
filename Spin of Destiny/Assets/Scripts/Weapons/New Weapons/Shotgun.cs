using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class ShotGun : Weapon
{
    private AudioSource shotgunSFX;
    [Header("Bullet")]
    public int bulletNumber;
    public float bulletOffset;
    public GameObject bullet;
    public float bulletSpeed;
    public int angleRange;
    public Transform canonEnd;
    public float bulletAcceleration;
    public GameObject spear;

    [Header("Knockback")]
    public float knockBackForce;
    public float slowTime;
    public float accelerationRate;
    public float slowFactor;
    void OnEnable()
    {
        animator = GetComponent<Animator>();
        shotgunSFX= GetComponent<AudioSource>();
        startPos = transform.position;
        
    } 
    
    public override void attack(InputAction.CallbackContext context){
        StartCoroutine("AttackCoroutine");
    }

    public IEnumerator AttackCoroutine(){
        transform.right = direction;

        animator.Play("sotgun_Clip");
        float pitch = Random.Range(0.8f, 1.2f);
        shotgunSFX.pitch = pitch;
        shotgunSFX.Play();
        player.gameObject.GetComponent<PlayerController>().startKnockBack(-direction,slowFactor, knockBackForce, slowTime, accelerationRate);

        fire();
        yield return null;

        float waitingTime = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;//animator.GetCurrentAnimatorStateInfo(0).length;
        float t = 0;

        while(t< waitingTime){

            yield return null;

            t += Time.deltaTime;
        }
        spear.SetActive(true);
        gameObject.SetActive(false);
        wp.canAttack = true;
    }

    void fire(){
        for(int i=-bulletNumber/2;i<=bulletNumber/2;++i){

            if(bulletNumber%2==0 && i ==0)continue;

            GameObject bulletClone = Instantiate(bullet);
            bulletClone.transform.position = canonEnd.position;
            bulletClone.GetComponent<Bullet>().speed = bulletSpeed;
            bulletClone.GetComponent<Bullet>().damage = damage;
            bulletClone.GetComponent<Bullet>().accelerationRate = bulletAcceleration;
            bulletClone.transform.position = bulletClone.transform.position + transform.up * i * bulletOffset;
            bulletClone.transform.right = transform.right;

            int angle = i==0 ? 0 : Random.Range(-angleRange, angleRange + 1);

            bulletClone.transform.rotation *=  Quaternion.Euler(0,0,angle);

            Destroy(bulletClone, 5);
        }
    }
}