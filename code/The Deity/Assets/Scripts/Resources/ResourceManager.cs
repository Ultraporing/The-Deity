/*
    Written by Tobias Lenz
 */

using Assets.Scripts.AI.Creature.Villager;
using Assets.Scripts.Environment.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Resources
{
    /// <summary>
    /// Holds a full list of all resources on the Map
    /// </summary>
    public class ResourceManager : IManager
    {
        /// <summary>
        /// Resource Type and List of Resource Sources of this type
        /// </summary>
        public class ResourceSourceList
        {
            public ResourceType m_ResourceType;
            public List<ResourceSource> m_ResourceSourceList = new List<ResourceSource>();
        }

        private ResourceSourceList[] ResourceSourcesList = new ResourceSourceList[Enum.GetValues(typeof(ResourceType)).Length];

        /// <summary>
        /// Constructor, initialize the Resource Source List Array
        /// </summary>
        public ResourceManager()
        {
            for (int i = 0; i < ResourceSourcesList.Length; i++)
            {
                ResourceSourcesList[i] = new ResourceSourceList() { m_ResourceType = ((ResourceType)i) };
            }
        }

        /// <summary>
        /// Gets the Resource Source List of the requested ResourceType
        /// </summary>
        /// <param name="resourceType">Requested Resource type</param>
        /// <returns>Array of the ResourceSourceList of this resource</returns>
        public ResourceSourceList GetListForResource(ResourceType resourceType)
        {
            return ResourceSourcesList[((int)resourceType)];
        }

        /// <summary>
        /// Gets the Resource Sources in a perticular area
        /// </summary>
        /// <param name="area">Rectangular are to find resource sources in</param>
        /// <returns>List of the ResourceSources in this area</returns>
        public List<ResourceSource> GetResourceSourcesInArea(Rect area)
        {
            List<ResourceSource> foundResources = new List<ResourceSource>();

            foreach (ResourceSourceList rsl in ResourceSourcesList)
            {
                foreach (ResourceSource rs in rsl.m_ResourceSourceList)
                {
                    if (area.Contains(new Vector2(rs.m_Position.x, rs.m_Position.z)))
                    {
                        foundResources.Add(rs);
                    }
                }
            }

            return foundResources;
        }

        /// <summary>
        /// Gets the Resource Sources of a type in a perticular area
        /// </summary>
        /// <param name="area">Rectangular are to find resource sources in</param>
        /// <param name="resourceType">Type of the searched resource</param>
        /// <returns>List of the ResourceSources of this type in the area</returns>
        public List<ResourceSource> GetResourceSourcesInArea(Rect area, ResourceType resourceType)
        {
            List<ResourceSource> foundResources = new List<ResourceSource>();
            ResourceSourceList rsl = GetListForResource(resourceType);

            if (rsl != null)
            {
                foreach (ResourceSource rs in rsl.m_ResourceSourceList)
                {
                    if (area.Contains(new Vector2(rs.m_Position.x, rs.m_Position.z)))
                    {
                        foundResources.Add(rs);
                    }
                }
            }

            return foundResources;
        }

        /// <summary>
        /// Register a resource Source
        /// </summary>
        /// <param name="resourceSource">the resource source to register</param>
        public void RegisterResourceSource(ResourceSource resourceSource)
        {
            GetListForResource(resourceSource.m_ResourceSourceData.ResourceType).m_ResourceSourceList.Add(resourceSource);
        }

        /// <summary>
        /// Deregister a resource Source
        /// </summary>
        /// <param name="resourceSource">the resource source to deregister</param>
        public void DeregisterResourceSource(ResourceSource resourceSource)
        {
            GetListForResource(resourceSource.m_ResourceSourceData.ResourceType).m_ResourceSourceList.Remove(resourceSource);
        }

        /// <summary>
        /// Update the Manager
        /// </summary>
        public void Update()
        {
            
        }
    }
}