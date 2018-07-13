/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.AI.Creature;
using Assets.Scripts.Resources;
using System.Collections.Generic;
using Assets.Scripts.Environment.Planet;
using System.Linq;
using Pathfinding;
using Assets.Scripts.Constructions;
using Assets.Scripts.AI.Creature.Villager;

namespace Assets.Scripts.AI.Creature.Behaviours
{
    /// <summary>
    /// Smallest AI part and baseclass for all Behaviours.
    /// Behaviours can be combined into a list which gets executed by the Creature
    /// </summary>
    public abstract class CreatureBehaviour
    {
        public delegate void OnBehaviourCollisionEnterEvent(Collision collision);
        public delegate void OnBehaviourCollisionExitEvent(Collision collision);
        public delegate void OnBehaviourCollisionStayEvent(Collision collision);

        public bool IsDone = false;
        protected CreatureAI OwningCreatureAI = null;

        public OnBehaviourCollisionEnterEvent OnBehaviourCollisionEnter = null;
        public OnBehaviourCollisionEnterEvent OnBehaviourCollisionExit = null;
        public OnBehaviourCollisionEnterEvent OnBehaviourCollisionStay = null;

        /// <summary>
        /// Constructor of the Creature Behaviour
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour.</param>
        public CreatureBehaviour(CreatureAI owningCreatureAI)
        {
            OwningCreatureAI = owningCreatureAI;
        }

        /// <summary>
        /// Starts the execution of the creature behaviour.
        /// </summary>
        public virtual void Start()
        {
            OnBehaviourCollisionEnter += OnCollisionEnter;
            OnBehaviourCollisionExit += OnCollisionExit;
            OnBehaviourCollisionStay += OnCollisionStay;
        }

        /// <summary>
        /// Updates the creature behaviour.
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Provides a deep copy of the creature behaviour.
        /// </summary>
        /// <returns>the new creature behaviour</returns>
        public abstract CreatureBehaviour DeepCopy();

        /// <summary>
        /// Has to be called by derived class once the Behaviour has done what it should do.
        /// This tells the AI Script that this Behaviour is complete and to get a new Behaviour from the Queue. 
        /// </summary>
        public virtual void Done()
        {
            OwningCreatureAI.SetAnimation("Idle");
            IsDone = true;
        }

        /// <summary>
        /// Searches for the shortest path to the provided resource type.
        /// </summary>
        /// <param name="resourceType">The resource type to look for.</param>
        /// <param name="coord">The found position of the resource.</param>
        /// <returns>true if successful</returns>
        protected bool FindClosestPathToResource(ResourceType resourceType, out Vector3 coord)
        {
            ResourceManager.ResourceSourceList resourceList = PlanetDatalayer.Instance.GetManager<ResourceManager>().GetListForResource(resourceType);
            List<Vector3> resourcePosList = resourceList.m_ResourceSourceList.Select(rs => rs.m_Position).ToList();

            List<KeyValuePair<float, ResourceSource>> ResourceListByDistance = new List<KeyValuePair<float, ResourceSource>>();

            for (int i = 0; i < resourceList.m_ResourceSourceList.Count; i++)
            {
                float len = (resourcePosList[i] - OwningCreatureAI.transform.position).sqrMagnitude;
                ResourceListByDistance.Add(new KeyValuePair<float, ResourceSource>(len, resourceList.m_ResourceSourceList[i]));
            }

            ResourceListByDistance.OrderBy(x => x.Key);
            bool found = false;
            Vector3 fVec = Vector3.zero;

                // see if he does already has reserved a resource, if so go there
                foreach (KeyValuePair<float, ResourceSource> kv in ResourceListByDistance)
                {
                    if (kv.Value.m_WorkedByVillager == (VillagerAI)OwningCreatureAI)
                    {
                        fVec = kv.Value.m_Position;
                        found = true;
                        kv.Value.m_WorkedByVillager = (VillagerAI)OwningCreatureAI;

                        break;
                    }
                }

                // if he has no resource reserved get a new one
                if (!found)
                {
                    foreach (KeyValuePair<float, ResourceSource> kv in ResourceListByDistance)
                    {
                        if (kv.Value.m_WorkedByVillager == (VillagerAI)OwningCreatureAI || kv.Value.m_WorkedByVillager == null)
                        {
                            fVec = kv.Value.m_Position;
                            found = true;
                            kv.Value.m_WorkedByVillager = (VillagerAI)OwningCreatureAI;

                            break;
                        }
                    }
                }
            
            coord = fVec;
            return found;
        }

        /// <summary>
        /// Filters unreachable pathfinding nodes based on the start position.
        /// </summary>
        /// <param name="beginning">Start position.</param>
        /// <param name="nodesToFilter">All nodes to check if they are reachable.</param>
        /// <returns></returns>
        protected List<Vector3> FilterUnreachableNodes(Vector3 beginning, List<Vector3> nodesToFilter)
        {
            return nodesToFilter.Where(n => PathUtilities.IsPathPossible(AstarPath.active.GetNearest(beginning, NNConstraint.Default).node, AstarPath.active.GetNearest(n, NNConstraint.Default).node)).ToList();
        }

        /// <summary>
        /// Finds the shortest path between beginning position and the targets.
        /// </summary>
        /// <param name="beginning">Start position.</param>
        /// <param name="targetList">Possible target position list.</param>
        /// <param name="coord">Found nearest target position.</param>
        /// <returns>true if successful</returns>
        protected bool FindShortestPath(Vector3 beginning, List<Vector3> targetList, out Vector3 coord)
        {
            float shortestPathLength = float.MaxValue;
            Vector3 shortestPath = Vector3.zero;
            bool found = false;

            foreach (Vector3 gn in targetList)
            {
                float len = (gn - beginning).sqrMagnitude;
                if (len < shortestPathLength)
                {
                    shortestPath = gn;
                    shortestPathLength = len;
                    found = true;
                }
            }

            coord = shortestPath;

            return found;
        }

        /// <summary>
        /// Finds the shortest path to a building of this type.
        /// </summary>
        /// <typeparam name="T">Type of the Building</typeparam>
        /// <param name="coord">Nearest building position</param>
        /// <returns>true if successful</returns>
        protected bool FindClosestPathToBuildingOfType<T>(out Vector3 coord) where T : Building
        {
            List<Vector3> buildList = PlanetDatalayer.Instance.GetManager<BuildingManager>().GetBuildingsOfType<T>().Select(x => x.transform.position).ToList();
            buildList = FilterUnreachableNodes(OwningCreatureAI.transform.position, buildList);

            return FindShortestPath(OwningCreatureAI.transform.position, buildList, out coord);
        }

        /// <summary>
        /// Finds the shortest path to a building of this type.
        /// </summary>
        /// <typeparam name="T">Type of the Building</typeparam>
        /// <param name="coord">Nearest building position</param>
        /// <param name="building">Reference to the found building</param>
        /// <returns>true if successful</returns>
        protected bool FindClosestPathToBuildingOfType<T>(out Vector3 coord, out T building) where T : Building
        {
            List<Vector3> buildList = PlanetDatalayer.Instance.GetManager<BuildingManager>().GetBuildingsOfType<T>().Select(x => x.transform.position).ToList();
            buildList = FilterUnreachableNodes(OwningCreatureAI.transform.position, buildList);

            Vector3 tmpCoord = Vector3.zero;
            if (FindShortestPath(OwningCreatureAI.transform.position, buildList, out tmpCoord))
            {
                List<T> buList = PlanetDatalayer.Instance.GetManager<BuildingManager>().GetBuildingsOfType<T>().Where(x => { return x.transform.position == tmpCoord; }).ToList();
                if (buList.Count > 0)
                {
                    building = buList.First();
                    coord = tmpCoord;
                    return true;
                }
                else
                {
                    building = null;
                    coord = tmpCoord;
                    return false;
                }
            }

            building = null;
            coord = tmpCoord;
            return false;
        }

        protected abstract void OnCollisionEnter(Collision collision);
        protected abstract void OnCollisionExit(Collision collision);
        protected abstract void OnCollisionStay(Collision collision);
    }
}