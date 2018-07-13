/*
    Written by Tobias Lenz
 */

using Assets.Scripts.Helper;
using Assets.Scripts.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Environment.Planet.Water
{
    /// <summary>
    /// Water Class to attach to the water plane to create the watermap
    /// </summary>
    [RequireComponent(typeof(ResourceSource))]
    public class Water : MonoBehaviour
    {
        public Terrain m_Terrain;

        /// <summary>
        /// Creates the Watermap
        /// </summary>
        private void Start()
        {
            if (!PlanetDatalayer.Instance.GetManager<WaterManager>().CreateWaterMap(m_Terrain, this))
            {
                Debug.LogError("Failed to create watermap");
            }
        }
    }
}
