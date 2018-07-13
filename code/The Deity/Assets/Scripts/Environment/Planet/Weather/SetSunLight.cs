/*
    3rd-Party Modified by Tobias Lenz
    https://unity3d.com/learn/tutorials/topics/graphics/realtime-global-illumination-daynight-cycle
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// Changes water texture and stars rotation based on time of day
/// </summary>
public class SetSunLight : MonoBehaviour
{
    Material sky;

    public Renderer water;

    public Transform stars;

    /// <summary>
    /// Initialize variables
    /// </summary>
    void Start()
    {
        sky = RenderSettings.skybox;
    }

    /// <summary>
    /// Update stars rotation and water material based on time of day
    /// </summary>
    void Update()
    {      
        stars.transform.rotation = transform.rotation;
        water.material.mainTextureOffset = new Vector2(Time.time / 100, 0);
        water.material.SetTextureOffset("_DetailAlbedoMap", new Vector2(0, Time.time / 80));
    }
}