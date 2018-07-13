/*
    Written by Tobias Lenz
 */

using Assets.Scripts.AI.Creature.Villager;
using Assets.Scripts.Constructions;
using Assets.Scripts.Environment.Planet;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Creatures.Villager
{
    /// <summary>
    /// Contains References to all Villagers
    /// </summary>
    public class VillagerManager : IManager
    {
        public List<VillagerAI> m_VillagerList = new List<VillagerAI>();
        public int NumVillagers { get { return m_VillagerList.Count; } }

        /// <summary>
        /// Register a Villager
        /// </summary>
        /// <param name="villager">The Villager</param>
        public void RegisterVillager(VillagerAI villager)
        {
            m_VillagerList.Add(villager);
        }
        
        /// <summary>
        /// Deregister a Villager
        /// </summary>
        /// <param name="villager">The Villager</param>
        public void DeregisterVillager(VillagerAI villager)
        {
            m_VillagerList.Remove(villager);
        }

        /// <summary>
        /// Gets all Heretics which are residing in houses on a perticular bonfire
        /// </summary>
        /// <param name="bonfire">The Bonfire to check</param>
        /// <returns>List of Heretics, otherwise empty list</returns>
        public List<VillagerAI> GetResidingHereticsFromBonfire(Bonfire bonfire)
        {
            List<VillagerAI> villagers = new List<VillagerAI>();
            if (bonfire != null)
            {
                if (PlanetDatalayer.Instance.GetManager<BuildingManager>().GetHouseListFromBonfire(bonfire) != null)
                {
                    foreach (House h in PlanetDatalayer.Instance.GetManager<BuildingManager>().GetHouseListFromBonfire(bonfire))
                    {
                        villagers.AddRange(h.Residents);
                    }
                }
            }
            return villagers.Where((v) => v.GetVillagerStats().IsHeretic).ToList();
        }

        /// <summary>
        /// Get All Heretics that exist
        /// </summary>
        /// <returns>List of Heretics, otherwise empty list</returns>
        public List<VillagerAI> GetAllHeretics()
        {
            return m_VillagerList.Where((v) => v.GetVillagerStats().IsHeretic).ToList();
        }

        public int GetVillagerIndex(VillagerAI villager)
        {
            for (int i = 0; i < m_VillagerList.Count; i++)
                if (m_VillagerList[i] == villager)
                    return i;

            return -1;
        }

        /// <summary>
        /// Update the Manager and the Goals
        /// </summary>
        public void Update()
        {
            if(NumVillagers >= 10 && PlanetDatalayer.Instance.GetManager<GoalManager>().m_EnoughVillagers == false && PlanetDatalayer.Instance.GetManager<GoalManager>().m_CycleNumber == 4)
            {
                PlanetDatalayer.Instance.GetManager<GoalManager>().m_EnoughVillagers = true;
                PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
            }
        }
    }
}