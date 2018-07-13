/*
    Written by Tobias Lenz
 */

using Assets.Scripts.AI;
using Assets.Scripts.Constructions;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Topography;
using Assets.Scripts.Events;
using Assets.Scripts.Helper;
using Assets.Scripts.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Environment.Planet
{
    /// <summary>
    /// Singleton implementation and Highest layer of Manager control
    /// </summary>
    public class PlanetDatalayer
    {
        #region Singleton Implementation
        private static PlanetDatalayer instance;

        public static PlanetDatalayer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PlanetDatalayer();
                }
                return instance;
            }
        }
        #endregion

        private readonly IManager[] m_Managers =
        {
            new WaterManager(),
            new BiomeManager(),
            new ResourceManager(),
            new SurfaceMaterialManager(),
            new BuildingManager(),
            new VillagerManager(),
            new WorldEventManager(),
            new FoMManager(),
            new GoalManager()
        };

        /// <summary>
        /// Gets a requested Manager by Type T
        /// </summary>
        /// <typeparam name="T">Type of the Manager</typeparam>
        /// <returns>Found Manager otherwise null</returns>
        public T GetManager<T>() where T : class, IManager
        {
            foreach (IManager manager in m_Managers)
            {
                if (manager is T)
                    return (T)manager;
            }

            return null;
        }

        /// <summary>
        /// Updates all Managers
        /// </summary>
        public void Update()
        {
            foreach (IManager manager in m_Managers)
                manager.Update();
        }
    }
}
