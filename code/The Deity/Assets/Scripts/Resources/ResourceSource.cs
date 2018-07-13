/*
    Written by Tobias Lenz
 */

using Assets.Scripts.AI.Creature.Villager;
using Assets.Scripts.Environment.Planet;
using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Resources
{
    /// <summary>
    /// Contains the Resource type and amount as well as if it is an Infinite resource
    /// </summary>
    public class ResourceSource
    {
        /// <summary>
        /// Actual Resource source Data class
        /// </summary>
        public class ResourceSourceData
        {
            private readonly ResourceType m_ResourceType;
            public ResourceType ResourceType
            {
                get
                {
                    return m_ResourceType;
                }
            }

            private int m_Amount = 0;
            public int Amount
            {
                get
                {
                    return m_Amount;
                }
                set
                {
                    m_Amount = value < 0 ? 0 : value;
                }
            }

            public bool IsEmpty
            {
                get
                {
                    return Amount > 0 || m_IsInfinite ? false : true;
                }
            }

            private bool m_IsInfinite = false;
            public bool IsInfinite
            {
                get
                {
                    return m_IsInfinite;
                }
                private set
                {
                    m_IsInfinite = value;
                }
            }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="resType">Resource Type</param>
            /// <param name="amount">Resource Amount</param>
            /// <param name="isInfinite">Is it infinite?</param>
            public ResourceSourceData(ResourceType resType, int amount, bool isInfinite = false)
            {
                m_ResourceType = resType;
                Amount = amount;
                IsInfinite = isInfinite;
            }

            /// <summary>
            /// Extract the amount of resource
            /// </summary>
            /// <param name="amountToExtract">Amount to extract</param>
            /// <returns>Extracted amount</returns>
            public int ExtractResource(int amountToExtract)
            {
                if (IsInfinite)
                {
                    return amountToExtract;
                }
                else if (Amount == 0)
                {
                    return 0;
                }

                
                int rest = m_Amount - amountToExtract;
                int extracted = rest < 0 ? amountToExtract + rest : amountToExtract;
                Amount -= amountToExtract;

                return extracted;
            }
        }

        public Vector3 m_Position;
        public GraphNode m_NearestNode;
        public ResourceSourceData m_ResourceSourceData = null;
        public delegate void OnResourceEmptyEvent();
        public OnResourceEmptyEvent OnResourceEmpty = null;
        public VillagerAI m_WorkedByVillager = null;
        public ResourceSourceWrapper ResourceSourceWrapperRef = null;

        /// <summary>
        /// Constructor, initialize variables, register resource
        /// </summary>
        /// <param name="resourceSourceWrapper">Reference to the Unity Resource Source Wrapper</param>
        /// <param name="position">Position of the Resource</param>
        /// <param name="resourceType">Type of the Resource</param>
        /// <param name="amount">Amount of the Resource</param>
        /// <param name="isInfinite">Is the resource infinite?</param>
        public ResourceSource(ResourceSourceWrapper resourceSourceWrapper, Vector3 position, ResourceType resourceType = ResourceType.Water, int amount = 10, bool isInfinite = false)
        {
            m_Position = position;
            AstarPath p = AstarPath.active;
            m_NearestNode = p.GetNearest(position).node;
            m_ResourceSourceData = new ResourceSourceData(resourceType, amount, isInfinite);
            ResourceSourceWrapperRef = resourceSourceWrapper;
            PlanetDatalayer.Instance.GetManager<ResourceManager>().RegisterResourceSource(this);
        }

        /// <summary>
        /// Constructor, initialize variables, register resource
        /// </summary>
        /// <param name="position">Position of the Resource</param>
        /// <param name="resourceType">Type of the Resource</param>
        /// <param name="amount">Amount of the Resource</param>
        /// <param name="isInfinite">Is the resource infinite?</param>
        public ResourceSource(Vector3 position, ResourceType resourceType = ResourceType.Water, int amount = 10, bool isInfinite = false)
        {
            m_Position = position;
            AstarPath p = AstarPath.active;
            m_NearestNode = p.GetNearest(position).node;
            m_ResourceSourceData = new ResourceSourceData(resourceType, amount, isInfinite);
            PlanetDatalayer.Instance.GetManager<ResourceManager>().RegisterResourceSource(this);
        }

        /// <summary>
        /// Deregister Resource
        /// </summary>
        ~ResourceSource()
        {
            PlanetDatalayer.Instance.GetManager<ResourceManager>().DeregisterResourceSource(this);
        }

        /// <summary>
        /// Extract from the Resource
        /// </summary>
        /// <param name="amountToExtract">Amount to extract</param>
        /// <returns>Key Value Pair with the Type extracted and amount</returns>
        public KeyValuePair<ResourceType, int> ExtractResource(int amountToExtract)
        {
            KeyValuePair<ResourceType, int> extr = new KeyValuePair<ResourceType, int>(m_ResourceSourceData.ResourceType, m_ResourceSourceData.ExtractResource(amountToExtract));

            if (m_ResourceSourceData.ResourceType != ResourceType.Water)
                ResourceSourceWrapperRef.Amount = m_ResourceSourceData.Amount;

            if (m_ResourceSourceData.IsEmpty)
            {
                PlanetDatalayer.Instance.GetManager<ResourceManager>().DeregisterResourceSource(this);
                if (OnResourceEmpty != null)
                    OnResourceEmpty();
            }

            return extr;
        }
    }
}
