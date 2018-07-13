/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Constructions
{
    /// <summary>
    /// Contains references to all built buildings
    /// </summary>
    public class BuildingManager : IManager
    {
        public List<Building> m_BuildingList = new List<Building>();

        /// <summary>
        /// Register a Building
        /// </summary>
        /// <param name="building">Building to Register</param>
        public void RegisterBuilding(Building building)
        {
            m_BuildingList.Add(building);
        }

        /// <summary>
        /// Deregister a Building
        /// </summary>
        /// <param name="building">Building to Deregister</param>
        public void DeregisterBuilding(Building building)
        {
            m_BuildingList.Remove(building);
        }

        /// <summary>
        /// Gets a list of Buildings with the type T 
        /// </summary>
        /// <typeparam name="T">Type of the Building</typeparam>
        /// <returns>List of buildings of type T, else empty list</returns>
        public List<T> GetBuildingsOfType<T>() where T : Building
        {
            List<T> outList = new List<T>();
            foreach (Building b in m_BuildingList)
            {
                if (b is T)
                {
                    outList.Add((T)b);
                }
            }

            return outList;
        }

        /// <summary>
        /// Gets all Houses from this perticular Bonfire
        /// </summary>
        /// <param name="bonfire">Bonfire to get the Houses from</param>
        /// <returns>List of Houses, else empty list</returns>
        public List<House> GetHouseListFromBonfire(Bonfire bonfire)
        {
            GameObject[] gameStuffs= bonfire.GetComponent<HouseBuilding>().m_BuiltHouses.Where(x=> x!= null).ToArray();
            List<House> houses = new List<House>();
            foreach (GameObject go in gameStuffs)
            {
                if (go.GetComponent<House>() != null)
                {
                    houses.Add(go.GetComponent<House>());
                }
            }

            return houses;
        }

        /// <summary>
        /// Update the Manager
        /// </summary>
        public void Update()
        {
            
        }
    }
}