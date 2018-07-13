/*
    Written by Tobias Lenz
 */

using Assets.Scripts.AI.Creature.Villager;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Constructions
{
    /// <summary>
    /// Villager House that can have residents
    /// </summary>
    public class House : Building
    {
        public static int TotalVillagerCapacity = 0;

        protected int m_MaxResidents = 2;
        protected List<VillagerAI> m_Residents = new List<VillagerAI>();

        public int NumResidents { get { return m_Residents.Count; } }
        public List<VillagerAI> Residents { get { return m_Residents; } }

        public int Index;

        /// <summary>
        /// Register Building with the BuildingManager and add the capacity to the total capacity
        /// </summary>
        protected override void RegisterBuilding()
        {
            base.RegisterBuilding();
            TotalVillagerCapacity += m_MaxResidents;
        }

        /// <summary>
        /// Deregister Building with the BuildingManager and subtract the capacity from the total capacity
        /// </summary>
        protected override void DeregisterBuilding()
        {
            base.DeregisterBuilding();
            TotalVillagerCapacity -= m_MaxResidents;
        }

        /// <summary>
        /// Try to add a Villager to the house as Resident
        /// </summary>
        /// <param name="villager">The Villager</param>
        /// <returns>true if successful</returns>
        public bool AddResident(VillagerAI villager)
        {
            if (m_Residents.Count < m_MaxResidents)
            {
                m_Residents.Add(villager);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Remove this Villager from the House
        /// </summary>
        /// <param name="villager">The Villager</param>
        /// <returns>true if successful</returns>
        public bool RemoveResident(VillagerAI villager)
        {
            return m_Residents.Remove(villager);
        }
    }
}