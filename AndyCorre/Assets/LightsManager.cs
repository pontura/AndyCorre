using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsManager : MonoBehaviour
{
    public Light light;
    public float maxLight = 2;
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
        light.intensity = Mathf.Lerp(light.intensity, lightValue, speed * Time.deltaTime);
    }
}
