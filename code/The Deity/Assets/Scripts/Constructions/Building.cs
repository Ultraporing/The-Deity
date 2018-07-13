/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.Environment.Planet;

namespace Assets.Scripts.Constructions
{
    /// <summary>
    /// Building baseclass
    /// </summary>
    public abstract class Building : MonoBehaviour
    {
        /// <summary>
        /// Call the RegisterBuilding Method
        /// </summary>
        private void Awake()
        {
            RegisterBuilding();
        }

        /// <summary>
        /// Call the DeregisterBuilding Method
        /// </summary>
        private void OnDestroy()
        {
            DeregisterBuilding();
        }

        /// <summary>
        /// Register the Building with the Manager
        /// </summary>
        protected virtual void RegisterBuilding()
        {
            PlanetDatalayer.Instance.GetManager<BuildingManager>().RegisterBuilding(this);
        }

        /// <summary>
        /// Deregister the Building with the Manager
        /// </summary>
        protected virtual void DeregisterBuilding()
        {
            PlanetDatalayer.Instance.GetManager<BuildingManager>().DeregisterBuilding(this);
        }
    }
}