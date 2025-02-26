using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider mainSlider;
    public Slider subSlider;
    public float animationTime;
    public float showTime;
    
    public void setMaxHealth(float health)  {
        mainSlider.maxValue = health;
    }


    IEnumerator looseHealthRoutine(float healthLost){
        float t = 0;
        float currentHealh = mainSlider.value;
        float newHealth = mainSlider.value-healthLost;

        while(t<animationTime){
            float currentLoose = t*healthLost/animationTime;
            mainSlider.value = currentHealh-currentLoose;
            t+=Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(showTime);

        t=0;
        while(t<animationTime){
            float currentLoose = t*healthLost/animationTime;
            subSlider.value = currentHealh-currentLoose;
            t+=Time.deltaTime;
            yield return null;
        }
    }
}