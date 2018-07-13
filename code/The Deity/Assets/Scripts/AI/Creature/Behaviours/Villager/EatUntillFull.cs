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

namespace Assets.Scripts.AI.Creature.Behaviours.Villager
{
    /// <summary>
    /// AI behaviour telling the creature to eat until it's full
    /// </summary>
    public class EatUntillFull : CreatureBehaviour
    {
        float EatTimer = 0;
        int m_Tries = 0;
        private ResourceSource m_CurrentResourceSource = null;

        /// <summary>
        /// Constructor of the Behaviour
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour.</param>
        public EatUntillFull(CreatureAI owningCreatureAI) : base(owningCreatureAI)
        {
        }

        /// <summary>
        /// Starts the execution of the creature behaviour.
        /// </summary>
        public override void Start()
        {
            List<ResourceSource> rs = PlanetDatalayer.Instance.GetManager<ResourceManager>().GetResourceSourcesInArea(new Rect(OwningCreatureAI.transform.position.x - 2, OwningCreatureAI.transform.position.z - 2, 4, 4), Resources.ResourceType.Food);
            if (rs.Count > 0) { m_CurrentResourceSource = rs[0]; }
        }

        /// <summary>
        /// Updates the creature behaviour.
        /// </summary>
        public override void Update()
        {
            if (OwningCreatureAI.reachedEndOfPath && !IsDone && OwningCreatureAI.CreatureStats.IsFoodFull())
            {
                OwningCreatureAI.SetAnimation("Idle");
                if (m_CurrentResourceSource != null)
                    m_CurrentResourceSource.m_WorkedByVillager = null;
                Done();
            }             
            else if (!IsDone && m_CurrentResourceSource != null)
            {
                if (EatTimer >= 1)
                {
                    if (m_CurrentResourceSource != null)
                        if (m_CurrentResourceSource.ExtractResource(1).Value != 0)
                        {
                            OwningCreatureAI.CreatureStats.AddFood(10);
                            EatTimer = 0;
                        }
                }
                else
                {
                    EatTimer += Time.deltaTime;
                }
            }
            else
            {
                List<ResourceSource> rs = PlanetDatalayer.Instance.GetManager<ResourceManager>().GetResourceSourcesInArea(new Rect(OwningCreatureAI.transform.position.x - 2, OwningCreatureAI.transform.position.z - 2, 4, 4), Resources.ResourceType.Food);
                if (rs.Count > 0) { m_CurrentResourceSource = rs[0]; }
                else
                    m_Tries++;

                if (m_Tries >= 3)
                {
                    OwningCreatureAI.SetAnimation("Idle");
                    if (m_CurrentResourceSource != null)
                        m_CurrentResourceSource.m_WorkedByVillager = null;

                    Done();
                }   
            }
        }

        /// <summary>
        /// Provides a deep copy of the creature behaviour.
        /// </summary>
        /// <returns>the new creature behaviour</returns>
        public override CreatureBehaviour DeepCopy()
        {
            EatUntillFull other = (EatUntillFull)this.MemberwiseClone();
            return other;
        }

        /// <summary>
        /// Collision Event
        /// </summary>
        /// <param name="collision">The collision triggering the event.</param>
        protected override void OnCollisionEnter(Collision collision)
        {
            if (m_CurrentResourceSource == null)
            {
                ResourceSource resourceSource = collision.gameObject.GetComponent<ResourceSourceWrapper>().ResourceSource;
                if (resourceSource != null)
                {
                    if (resourceSource.m_ResourceSourceData.ResourceType == ResourceType.Food)
                    {
                        m_CurrentResourceSource = resourceSource;
                    }
                }
            }
        }

        /// <summary>
        /// Collision Event
        /// </summary>
        /// <param name="collision">The collision triggering the event.</param>
        protected override void OnCollisionExit(Collision collision)
        {
            if (m_CurrentResourceSource == null)
            {
                ResourceSourceWrapper resourceSource = collision.gameObject.GetComponent<ResourceSourceWrapper>();
                if (resourceSource == null)
                {
                    OwningCreatureAI.SetAnimation("Idle");
                    Done();
                }
            }
        }

        /// <summary>
        /// Collision Event
        /// </summary>
        /// <param name="collision">The collision triggering the event.</param>
        protected override void OnCollisionStay(Collision collision)
        {
            if (m_CurrentResourceSource == null)
            {
                ResourceSource resourceSource = collision.gameObject.GetComponent<ResourceSourceWrapper>().ResourceSource;
                if (resourceSource != null)
                {
                    if (resourceSource.m_ResourceSourceData.ResourceType == ResourceType.Food)
                    {
                        m_CurrentResourceSource = resourceSource;
                    }
                }
            }
        }
    }
}