using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    public WeaponManager wp;
    public float damage;
    protected Vector2 direction;
    protected float offset;
    protected Transform player;
    protected Vector2 startPos=Vector2.zero;
    protected Animator animator;
    
    public abstract void attack(InputAction.CallbackContext context);

    public virtual void setDir(Vector2 dir){
        direction = dir.normalized;
    }

    public void setPlayer(Transform player){
        this.player = player;
    }

    public void setOffset(float offset){
        this.offset = offset;
    }

    void FixedUpdate()
    {
        transform.position = (Vector2)player.position + new Vector2(direction.x * offset, direction.y * offset);
    }
}
