using Assets.Scripts.Environment.Planet;
using Assets.Scripts.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BiomeIndicator : MonoBehaviour {

    //Lea Kohl
    //Helps the player find the were bushes and trees spawn
    public Sprite[] m_CloudImages;
    //0 = cross, 1 = bush, 2 = trees
    public Image m_CurrentImage;

	void Update () {
        int x = (int)gameObject.transform.position.x;
        int z = (int)gameObject.transform.position.z;
        //incase an Array Index Out of Bounds exception is thrown whe nthe player goes over the biome borders
        try
        {
            if (PlanetDatalayer.Instance.GetManager<BiomeManager>().BiomeMap[x, z] == Biomes.BushLand)
            {
                m_CurrentImage.sprite = m_CloudImages[1];
            }
            else if (PlanetDatalayer.Instance.GetManager<BiomeManager>().BiomeMap[x, z] == Biomes.Forest)
            {
                m_CurrentImage.sprite = m_CloudImages[2];
            }
            else
            {
                m_CurrentImage.sprite = m_CloudImages[0];
            }
        }
        catch (Exception e)
        {
            m_CurrentImage.sprite = m_CloudImages[0];
        }
        
	}
}
