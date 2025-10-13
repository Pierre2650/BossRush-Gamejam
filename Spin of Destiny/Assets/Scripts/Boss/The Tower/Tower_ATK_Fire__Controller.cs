using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower_ATK_Fire_Controller : MonoBehaviour
{

    private AudioSource fireSFX;
    [Header("Spawn Anim")]
    private float animElapsed = 0f;
    private float animDur = 0.2f;
    private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [Header("Duration")]
    private float toDestroyElapsed = 0f;
    private float toDestroyDur = 3f;

    [Header("Damage")]
    public float damage;
    public float frequency = 0.2f;
    public float damageRadius=1;
    private float timer;
    

    // Start is called before the first frame update
    void Start()
    {
        fireSFX = GetComponent<AudioSource>();

        float pitch = Random.Range(0.6f, 1.2f);
        fireSFX.pitch = pitch;
        fireSFX.Play();
        StartCoroutine(spawnAnim());


    }


    private void Update()
    {
        if(Time.time - timer >= frequency){
            Damage.damageCircle(transform.position, damageRadius, LayerMask.GetMask("Player"), damage);
        }

        toDestroyElapsed += Time.deltaTime;
        if (toDestroyElapsed > toDestroyDur)
        {
            Destroy(gameObject);

        }

    }


    private IEnumerator spawnAnim()
    {

        Vector2 start = new Vector2( transform.localPosition.x, transform.localPosition.y + 1f);
        Vector2 end = transform.localPosition;

        float percentageDur = 0f;

        while (animElapsed < animDur)
        {

            percentageDur = animElapsed / animDur;

            transform.localPosition = Vector2.Lerp(start, end, curve.Evaluate(percentageDur));

            animElapsed += Time.deltaTime;
            yield return null;

        }
        animElapsed = 0;
      

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, damageRadius);       
    }
}
