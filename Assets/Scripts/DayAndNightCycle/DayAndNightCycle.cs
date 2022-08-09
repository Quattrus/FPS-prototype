using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayAndNightCycle : MonoBehaviour
{

    [Header("Initialization Variables")]
    [Range(0.0f, 1.0f)]
    [SerializeField] float time;
    [SerializeField] float fullDayLength;
    [SerializeField] float startTime = 0.4f;
    [SerializeField] Vector3 noon;
    private float timeRate;
    [SerializeField] bool nightTime; //just to set night or day



    [Header("Sun")]
    [SerializeField] Light sun;
    [SerializeField] Gradient sunColor;
    [SerializeField] AnimationCurve sunIntensity;


    [Header("Moon")]
    [SerializeField] Light moon;
    [SerializeField] Gradient moonColor;
    [SerializeField] AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    [SerializeField] AnimationCurve lightingIntensityMultiplier;
    [SerializeField] AnimationCurve reflectionsIntensityMultiplier;

    



    // Start is called before the first frame update
    void Start()
    {
        timeRate = 1.0f / fullDayLength;
        time = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        DayAndNight();
    }

    public void SetDay()
    {
        time = 0.4f;
    }

    public void SetNight()
    {
        time = 7.0f;
    }

    private void DayAndNight()
    {
        //increment time

        time += timeRate * Time.deltaTime;
        if (time >= 1.0f)
        {
            time = 0.0f;
        }
        //light rotation
        sun.transform.eulerAngles = (time - 0.25f) * noon * 4.0f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4.0f;


        //light intensity controls here
        sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonIntensity.Evaluate(time);


        //change colors
        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);

        //enable/disable the sun
        if (sun.intensity == 0 && sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(false);
        }
        else if (sun.intensity > 0 && !sun.gameObject.activeInHierarchy)
        {
            sun.gameObject.SetActive(true);

        }

        //enable/disable the moon
        if (moon.intensity == 0 && moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(false);
        }
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy)
        {
            moon.gameObject.SetActive(true);
        }
        //lighting and reflections intensity
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionsIntensityMultiplier.Evaluate(time);
    }

}
