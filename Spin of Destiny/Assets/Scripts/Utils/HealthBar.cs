using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider mainSlider;
    //public Slider subSlider;
    public float animationTime;
    //public float showTime;

    Queue<IEnumerator> routines;
    bool isRunning;

    Coroutine currentRoutine = null;

    void Update()
    {
        int temp = 0;

        if (routines.Count > 0)
        {
            if (currentRoutine == null)
            {
                runRoutine();
            }
        }

       
    }
    
    public void setMaxHealth(float health)  {
        routines = new Queue<IEnumerator>();
 
        mainSlider.maxValue = health;
        mainSlider.value = health;
    }

    public void enQueueRoutine(float dmg) {
 
        if(currentRoutine == null)
        {
            currentRoutine =  StartCoroutine(looseHealthRoutine(dmg));
        }
        else{
            routines.Enqueue(looseHealthRoutine(dmg));
        }
    }

    public void runRoutine() {
        
        currentRoutine = StartCoroutine(routines.Dequeue());
        
    }


    public IEnumerator looseHealthRoutine(float healthLost){
        float t = 0;
        float currentHealh = mainSlider.value;
        float newHealth = mainSlider.value-healthLost;

        int temp = 0;

        while(t<animationTime){

            if (temp > 500)
            {
                Debug.Log("While loop treshhold Reached on COROUTINE, breaking cycle");
                Debug.Log("Local var t = "+t);
                break;

            }


            float currentLoose = t*healthLost/animationTime;
            mainSlider.value = currentHealh-currentLoose;
            t+=Time.deltaTime;

            temp++;
            yield return null;
        }

        currentRoutine = null;

    }
}