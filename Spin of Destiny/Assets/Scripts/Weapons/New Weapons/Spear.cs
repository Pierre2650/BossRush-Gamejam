using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Spear : Weapon
{
    
    public Collider2D box; 
    [Header("animation")]
    /*public SpriteRenderer renderer;
    public Sprite[] frames = new Sprite[6];
    public float[] animationTimes = new float[6]; 
    public float[] animationOffsets = new float[6];*/
    bool isAttacking = false;
    bool holding = false;
    PlayerInputActions inputActions;
    public Sprite spriteIdle;

    void OnEnable(){
        box.enabled = false;
        isAttacking = false;
        GetComponent<SpriteRenderer>().sprite = spriteIdle;
    }

    void Start()
    {
        /*renderer = GetComponent<SpriteRenderer>();*/
        box = GetComponent<BoxCollider2D>();
        box.enabled = false;
        animator = GetComponent<Animator>();
        startPos = transform.position;
        inputActions = GameObject.Find("Player").GetComponent<PlayerController>().playerInputActions;
        
    } 

    void Update(){ 
        if(inputActions.Player.Attack.ReadValue<float>()>0 && !isAttacking && wp.canAttack){
            StartCoroutine("AttackCoroutine");
        }
    }

    public override void setDir(Vector2 dir){
        base.setDir(dir);
        transform.up = dir;
    }
    public override void attack(InputAction.CallbackContext context){
        /*if(context.performed){
            holding = true;
            box.enabled = true;
            animator.Play("spear_Clip");
            animator.SetBool("isAttacking", true);
        }
        if(context.canceled){
            holding = false;
            print(context+"");
            box.enabled = false;
            animator.SetBool("isAttacking", false);
        }*/
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        Health enemyHealth = other.GetComponent<Health>();
        if(other.tag!="Player"&& enemyHealth!=null){
            enemyHealth.takeDamage(damage);
        }
    }

    public IEnumerator AttackCoroutine(){
        isAttacking = true;
        box.enabled = true;
        animator.Play("spear_Clip");
        while(animator.GetCurrentAnimatorStateInfo(0).IsName("spear_Clip"))
        {
            yield return null;
        }
        box.enabled = false;
        isAttacking = false;
        
        GetComponent<SpriteRenderer>().sprite = spriteIdle;
    }

    /*public IEnumerator SpearAnimCoroutine(){
        box.enabled = true;
        isAttacking = true;
        Vector3 startPosition = transform.localPosition;
        
        for(int i = 0; i < frames.Length; i++){
            renderer.sprite = frames[i];
            float elapsedTime = 0;
            Vector3 position = transform.localPosition;
           
            Vector3 endPoint = transform.localPosition+(Vector3)this.direction *animationOffsets[i];
            Debug.Log(direction);
            while(elapsedTime <= animationTimes[i]){
                transform.up = direction;
                float t = elapsedTime / animationTimes[i];
                transform.localPosition = Vector3.Lerp(position, endPoint, t );
                yield return null;
                elapsedTime += Time.deltaTime;
            }
            transform.localPosition = transform.localPosition+(Vector3)this.direction *animationOffsets[i];
        }
        transform.localPosition = startPosition;
        box.enabled = false;
        isAttacking = false;
    }*/
    


}