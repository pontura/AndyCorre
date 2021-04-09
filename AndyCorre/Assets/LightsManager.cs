using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsManager : MonoBehaviour
{
    public Light light;
    public float maxLight = 2;
    public float minLightValue = 0.3f;
    public float speed = 2;
    float lightValue;

    void Start()
    {
        lightValue = maxLight;
    }
    public void SetStatus(float value)
    {
        lightValue = value * maxLight;        
    }
    private void Update()
    {
        if (lightValue < minLightValue)
            lightValue = minLightValue;
        light.intensity = Mathf.Lerp(light.intensity, lightValue, speed * Time.deltaTime);
    }
}
