using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFallingStar : MonoBehaviour
{
    
    public GameObject star;
    private Falling_Star_Controller star_Controller;
    private Vector2 starSpawnPos;
    private AudioSource mySFX;
    public float damage_zone_radius;
    public float damage;


    

    // Start is called before the first frame update
    void Start()
    {
        star_Controller = star.GetComponent<Falling_Star_Controller>();
        mySFX = GetComponent<AudioSource>();
        starSpawnPos = new Vector2(transform.position.x, 12 );

        star.transform.position = starSpawnPos;
        spawnStar();

    }

    // Update is called once per frame
    void Update()
    {

        if (star_Controller.fallEnd)
        {
            Damage.damageCircle(transform.position, damage_zone_radius, LayerMask.GetMask("Player"), damage);
            Destroy(this.gameObject);
            
        }
    }

    private void spawnStar()
    {
        float pitch = Random.Range(0.8f, 2.2f);
        mySFX.pitch = pitch;
        mySFX.Play();

        star_Controller.makeItFall();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, damage_zone_radius);
    }
}
