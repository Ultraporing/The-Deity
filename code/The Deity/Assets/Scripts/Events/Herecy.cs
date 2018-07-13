/*
    Written by Tobias Lenz
 */

using Assets.Scripts.AI.Creature.Villager;
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
    /// Causes 20% of the Villagers to become Heretics
    /// </summary>
    public class Herecy : WorldEvent
    {
        private float m_HereticPercentile = 0.2f;

        /// <summary>
        /// Constructor of the Herecy World Event
        /// </summary>
        /// <param name="eventDoneCallback">Callback when the event is done</param>
        public Herecy(WorldEventManager.EventDoneCallback eventDoneCallback) : base(eventDoneCallback)
        {
        }

        /// <summary>
        /// Checks if the prerequisites are met to start the Event
        /// </summary>
        /// <returns>true if prerequisites are met</returns>
        public override bool PrerequisitesMet()
        {
            return PlanetDatalayer.Instance.GetManager<VillagerManager>().NumVillagers > 5 && PlanetDatalayer.Instance.GetManager<FoMManager>().m_CurrentFoM >= 60;
        }

        /// <summary>
        /// Sets the Heretic flag on 20% of the villagers
        /// </summary>
        public override void Start()
        {
            int numHeretics = Mathf.CeilToInt(PlanetDatalayer.Instance.GetManager<VillagerManager>().NumVillagers * m_HereticPercentile);

            List<int> villagerIndex = new List<int>();
            int done = 0;
            while (done < numHeretics)
            {
                int idx = UnityEngine.Random.Range(0, PlanetDatalayer.Instance.GetManager<VillagerManager>().NumVillagers - 1);
                if (!villagerIndex.Contains(idx))
                {
                    villagerIndex.Add(idx);
                    done++;
                }                  
            }

            foreach (int i in villagerIndex)
            {
                PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList[i].GetVillagerStats().IsHeretic = true;
            }
        }

        /// <summary>
        /// Ends the event when no villagers are heretics anymore
        /// </summary>
        public override void Update()
        {
            if (PlanetDatalayer.Instance.GetManager<VillagerManager>().GetAllHeretics().Count == 0)
                End();
        }
        
    }
}
