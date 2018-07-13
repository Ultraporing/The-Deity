/*
    Written by Tobias Lenz
 */

using Assets.Scripts.Environment.Planet;
using Assets.Scripts.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unity Wrapper for the Resource source, to be put on objects that are resources
/// </summary>
public class ResourceSourceWrapper : MonoBehaviour {

    public ResourceType ResourceType;
    public int Amount = 10;
    public bool IsInfinite = false;
    public ResourceSource ResourceSource = null;
    float elapsed = 0;

    /// <summary>
    /// Initialize variables, subscribe to the OnResourceEmpty Event
    /// </summary>
    private void Start()
    {
        ResourceSource = new ResourceSource(this, transform.position, ResourceType, Amount, IsInfinite);
        ResourceSource.OnResourceEmpty += OnResourceEmpty;
    }

    /// <summary>
    /// Regrow berries if they are empty
    /// </summary>
    private void Update()
    {
        if (ResourceSource.m_ResourceSourceData.ResourceType == ResourceType.Food && ResourceSource.m_ResourceSourceData.IsEmpty)
        {
            elapsed += Time.deltaTime;

            if (elapsed >= 60)
            {
                ResourceSource.m_ResourceSourceData.Amount = 10;
                transform.Find("Berries").gameObject.SetActive(true);
                PlanetDatalayer.Instance.GetManager<ResourceManager>().RegisterResourceSource(ResourceSource);
                elapsed = 0;
            }            
        }
    }

    /// <summary>
    /// If its Food then deactivate berries, otherwise destroy the Gameobject
    /// </summary>
    private void OnResourceEmpty()
    {
        try
        {
            if (gameObject != null)
            {
                if (ResourceSource.m_ResourceSourceData.ResourceType == ResourceType.Food)
                {
                    transform.Find("Berries").gameObject.SetActive(false);
                }
                else
                    Destroy(gameObject);
            }               
        }
        catch { }
    }
}
