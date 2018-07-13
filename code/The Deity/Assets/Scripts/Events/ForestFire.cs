/*
    Written by Tobias Lenz
 */

using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using Assets.Scripts.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Events
{
    /// <summary>
    /// World Event causing 80% of the trees to catch fire
    /// </summary>
    public class ForestFire : WorldEvent
    {
        private float m_PercentileBurningTrees = 0.8f;

        /// <summary>
        /// Constructor of the Forest Fire World Event
        /// </summary>
        /// <param name="eventDoneCallback">Callback when the event is done</param>
        public ForestFire(WorldEventManager.EventDoneCallback eventDoneCallback) : base(eventDoneCallback)
        {         
        }

        /// <summary>
        /// Checks if the prerequisites are met to start the Event
        /// </summary>
        /// <returns>true if prerequisites are met</returns>
        public override bool PrerequisitesMet()
        {
            return PlanetDatalayer.Instance.GetManager<ResourceManager>().GetListForResource(ResourceType.Wood).m_ResourceSourceList.Count > 0 && PlanetDatalayer.Instance.GetManager<FoMManager>().m_CurrentFoM >= 60;
        }

        /// <summary>
        /// Get 80% of the Trees and set them on fire
        /// </summary>
        public override void Start()
        {
            List<ResourceSource> resourceSources = PlanetDatalayer.Instance.GetManager<ResourceManager>().GetListForResource(ResourceType.Wood).m_ResourceSourceList;
            GameObject firePrefab = Transform.FindObjectOfType<FireSpawner>().m_FirePrefab;

            int numberTreesToBurn = (int)Mathf.Ceil(resourceSources.Count * m_PercentileBurningTrees);
            List<int> treeIndex = new List<int>();
            int done = 0;
            while (done < numberTreesToBurn)
            {
                int idx = UnityEngine.Random.Range(0, resourceSources.Count - 1);
                if (!treeIndex.Contains(idx))
                {
                    treeIndex.Add(idx);
                    done++;
                }
            }

            foreach (int i in treeIndex)
                Transform.Instantiate(firePrefab, resourceSources[i].m_Position, Quaternion.identity);

            End();
        }

        /// <summary>
        /// Update the Event
        /// </summary>
        public override void Update()
        {
            
        }
    }
}
