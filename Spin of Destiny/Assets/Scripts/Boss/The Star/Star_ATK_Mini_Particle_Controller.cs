using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star_ATK_Mini_Particles_Controller : MonoBehaviour
{
    private ParticleSystem myPrtSys;

    private void OnEnable()
    {
        transform.position = new Vector3(transform.position.x , transform.position.y, -10);
        
    }


    public void setRotation(Vector3 dir)
    {
        transform.up = (dir - transform.position).normalized;
        transform.eulerAngles = new Vector3(0, 0f, transform.eulerAngles.z + 180);
    }

}
