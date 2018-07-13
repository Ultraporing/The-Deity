/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.AI.Creature.Behaviours;
using Assets.Scripts.Resources;
using Assets.Scripts.AI.Creature;
using System;
using Assets.Scripts.Environment.Planet;
using System.Collections.Generic;
using Assets.Scripts.AI.Creature.Villager;
using Assets.Scripts.Creatures.Villager;

namespace Assets.Scripts.AI.Creature.Behaviours.Villager
{
    /// <summary>
    /// AI behaviour telling the creature to extract this resourcetype
    /// </summary>
    public class ExtractResource : CreatureBehaviour
    {
        float ExtractTimer = 0;
        private ResourceSource m_CurrentResourceSource = null;
        private ResourceType m_TargetResourceType;

        /// <summary>
        /// Constructor of the Behaviour
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour.</param>
        /// <param name="targetResourceType">The resource type to extract.</param>
        public ExtractResource(CreatureAI owningCreatureAI, ResourceType targetResourceType) : base(owningCreatureAI)
        {
            m_TargetResourceType = targetResourceType;
        }

        /// <summary>
        /// Starts the execution of the creature behaviour.
        /// </summary>
        public override void Start()
        {
            List<ResourceSource> rs = PlanetDatalayer.Instance.GetManager<ResourceManager>().GetResourceSourcesInArea(new Rect(OwningCreatureAI.transform.position.x -2, OwningCreatureAI.transform.position.z -2, 4, 4), m_TargetResourceType);
            if (rs.Count > 0) { m_CurrentResourceSource = rs[0]; }
        }

        /// <summary>
        /// Checks if the Inventory is full.
        /// </summary>
        /// <returns>true if the Inventory is full</returns>
        private bool IsInventoryFull()
        {
            return ((VillagerAI)OwningCreatureAI).GetVillagerStats().VillagerInventory.FindNextSlotWithSpace(m_TargetResourceType) == null;
        }

        /// <summary>
        /// Updates the creature behaviour.
        /// </summary>
        public override void Update()
        {
            if (OwningCreatureAI.reachedEndOfPath && !IsDone && (m_CurrentResourceSource == null || IsInventoryFull()))
            {
                if (m_CurrentResourceSource != null)
                    m_CurrentResourceSource.m_WorkedByVillager = null;
                Done();
            }             
            else if (OwningCreatureAI.reachedEndOfPath && !IsDone && (m_CurrentResourceSource != null && !IsInventoryFull()))
            {
                if (OwningCreatureAI.GetCurrentAnimation().AnimationName != "Extracting")              
                    OwningCreatureAI.SetAnimation("Extracting");
                
                if (ExtractTimer >= 1)
                {
                    if (m_CurrentResourceSource != null)
                        if (m_CurrentResourceSource.ExtractResource(1).Value != 0)
                        {
                            ((VillagerAI)OwningCreatureAI).GetVillagerStats().VillagerInventory.AddResource(m_TargetResourceType, 1);
                            PlanetDatalayer.Instance.GetManager<FoMManager>().IncreaseFoM(PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList.IndexOf((VillagerAI)OwningCreatureAI), 1);
                            ExtractTimer = 0;
                        }
                }
                else
                {
                    ExtractTimer += Time.deltaTime;
                }
            }
            else
            {
                List<ResourceSource> rs = PlanetDatalayer.Instance.GetManager<ResourceManager>().GetResourceSourcesInArea(new Rect(OwningCreatureAI.transform.position.x - 2, OwningCreatureAI.transform.position.z - 2, 4, 4), m_TargetResourceType);
                if (rs.Count > 0) { m_CurrentResourceSource = rs[0]; }
            }
        }

        /// <summary>
        /// Provides a deep copy of the creature behaviour.
        /// </summary>
        /// <returns>the new creature behaviour</returns>
        public override CreatureBehaviour DeepCopy()
        {
            ExtractResource other = (ExtractResource)this.MemberwiseClone();
            return other;
        }

        /// <summary>
        /// (Unused) Collision Event
        /// </summary>
        /// <param name="collision">The collision triggering the event.</param>
        protected override void OnCollisionEnter(Collision collision)
        {
        }

        /// <summary>
        /// (Unused) Collision Event
        /// </summary>
        /// <param name="collision">The collision triggering the event.</param>
        protected override void OnCollisionExit(Collision collision)
        {
        }

        /// <summary>
        /// (Unused) Collision Event
        /// </summary>
        /// <param name="collision">The collision triggering the event.</param>
        protected override void OnCollisionStay(Collision collision)
        {
        }
    }
}