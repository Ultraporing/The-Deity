using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Resources;

/*
    3rd party, Modified by Tobias Lenz
    https://unity3d.com/learn/tutorials/topics/graphics/realtime-global-illumination-daynight-cycle
 */

/// <summary>
/// Controls the Day night cycle by manipulating the skybox and directional light (sun)
/// </summary>
public class AutoIntensity : MonoBehaviour
{

    public Gradient nightDayColor;

    public float maxIntensity = 3f;
    public float minIntensity = 0f;
    public float minPoint = -0.2f;

    public float maxAmbient = 1f;
    public float minAmbient = 0f;
    public float minAmbientPoint = -0.2f;


    public Gradient nightDayFogColor;
    public AnimationCurve fogDensityCurve;
    public float fogScale = 1f;

    public float dayAtmosphereThickness = 0.4f;
    public float nightAtmosphereThickness = 0.87f;

    public Vector3 dayRotateSpeed;
    public Vector3 nightRotateSpeed;
    public float dayLengthInSec = 300;
    public Dictionary<ResourceType, int> dictionary = new Dictionary<ResourceType, int>();

    float skySpeed = 1;

    Light sun;
    Skybox sky;
    Material skyMat;

    /// <summary>
    /// Initialize variables
    /// </summary>
    void Start()
    {
        sun = GetComponent<Light>();
        skyMat = RenderSettings.skybox;
    }

    /// <summary>
    /// Update the rotation and intensity of the sun (directional light) based on the elapsed time
    /// </summary>
    void Update()
    {

        float tRange = 1 - minPoint;
        float dot = Mathf.Clamp01((Vector3.Dot(sun.transform.forward, Vector3.down) - minPoint) / tRange);
        float i = ((maxIntensity - minIntensity) * dot) + minIntensity;

        sun.intensity = i;

        tRange = 1 - minAmbientPoint;
        dot = Mathf.Clamp01((Vector3.Dot(sun.transform.forward, Vector3.down) - minAmbientPoint) / tRange);
        i = ((maxAmbient - minAmbient) * dot) + minAmbient;
        RenderSettings.ambientIntensity = i;

        sun.color = nightDayColor.Evaluate(dot);
        RenderSettings.ambientLight = sun.color;

        RenderSettings.fogColor = nightDayFogColor.Evaluate(dot);
        RenderSettings.fogDensity = fogDensityCurve.Evaluate(dot) * fogScale;

        i = ((dayAtmosphereThickness - nightAtmosphereThickness) * dot) + nightAtmosphereThickness;
        skyMat.SetFloat("_AtmosphereThickness", i);

        if (dot > 0)
        {
            transform.Rotate(dayRotateSpeed * (60 / dayLengthInSec) * Time.deltaTime * skySpeed);
        }
        else
        {
            transform.Rotate(nightRotateSpeed * (60 / dayLengthInSec) * Time.deltaTime * skySpeed);
        }  
    }
}