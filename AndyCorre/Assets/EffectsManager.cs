using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class EffectsManager : MonoBehaviour
{
    public PostProcessVolume volume;

    void Awake()
    {
        volume = GetComponent<PostProcessVolume>();
    }

    public void ChangeAberration(float value)
    {
        ChromaticAberration ca;
        volume.profile.TryGetSettings(out ca);
        ca.intensity.value = value;
    }
}
