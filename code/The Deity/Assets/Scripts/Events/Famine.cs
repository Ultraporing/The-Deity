/*
    Written by Tobias Lenz & Lea Kohl
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
    /// WorldEvent causing all bushes to loose its berries
    /// </summary>
    public class Famine : WorldEvent
    {
        /// <summary>
        /// Constructor of the Famine World Event
        /// </summary>
        /// <param name="eventDoneCallback">Callback when the event is done</param>
        public Famine(WorldEventManager.EventDoneCallback eventDoneCallback) : base(eventDoneCallback)
        {
        }

        /// <summary>
        /// Checks if the prerequisites are met to start the Event
        /// </summary>
        /// <returns>true if prerequisites are met</returns>
        public override bool PrerequisitesMet()
        {
            return PlanetDatalayer.Instance.GetManager<ResourceManager>().GetListForResource(ResourceType.Food).m_ResourceSourceList.Count > 0 && PlanetDatalayer.Instance.GetManager<FoMManager>().m_CurrentFoM >= 60;
        }

        /// <summary>
        /// Strip all berries from all bushes
        /// </summary>
        public override void Start()
        {
            List<ResourceSource> resourceSources = PlanetDatalayer.Instance.GetManager<ResourceManager>().GetListForResource(ResourceType.Food).m_ResourceSourceList;
            while (resourceSources.Count > 0)
            {
                resourceSources.Last().ExtractResource(20);
            }
            End();
        }

        /// <summary>
        /// Update the event
        /// </summary>
        public override void Update()
        {
            
        }
    }
}
