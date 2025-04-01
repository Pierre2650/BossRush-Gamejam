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

    void Update()
    {
        while(routines.Count > 0){
            runRoutine();
        }
    }
    
    public void setMaxHealth(float health)  {
        routines = new Queue<IEnumerator>();
        isRunning = false;
        mainSlider.maxValue = health;
        mainSlider.value = health;
    }

    public void enQueueRoutine(float dmg) {
        if(!isRunning){
            StartCoroutine(looseHealthRoutine(dmg));
        }
        else{
            routines.Enqueue(looseHealthRoutine(dmg));
        }
    }

    public void runRoutine() {
        if(!isRunning){
            StartCoroutine(routines.Dequeue());
        }
    }


    public IEnumerator looseHealthRoutine(float healthLost){
        isRunning = true;
        float t = 0;
        float currentHealh = mainSlider.value;
        float newHealth = mainSlider.value-healthLost;

        while(t<animationTime){
            float currentLoose = t*healthLost/animationTime;
            mainSlider.value = currentHealh-currentLoose;
            t+=Time.deltaTime;
            yield return null;
        }
        isRunning = false;

        /*yield return new WaitForSeconds(showTime);

        t=0;
        while(t<animationTime){
            float currentLoose = t*healthLost/animationTime;
            subSlider.value = currentHealh-currentLoose;
            t+=Time.deltaTime;
            yield return null;
        }*/
    }
}