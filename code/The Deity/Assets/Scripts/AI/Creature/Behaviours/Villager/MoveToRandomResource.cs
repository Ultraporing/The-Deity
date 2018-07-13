/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.AI.Creature.Behaviours;
using Assets.Scripts.Resources;
using Assets.Scripts.AI.Creature;
using System;

namespace Assets.Scripts.AI.Creature.Behaviours.Villager
{
    /// <summary>
    /// AI behaviour telling the creature to move to a random resource
    /// </summary>
    public class MoveToRandomResource : CreatureBehaviour
    {
        bool foundViable = false;
        readonly int maxTries = 10;
        int curTry = 0;

        /// <summary>
        /// Constructor of the Behaviour
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour.</param>
        public MoveToRandomResource(CreatureAI owningCreatureAI) : base(owningCreatureAI)
        {
        }

        /// <summary>
        /// Starts the execution of the creature behaviour.
        /// </summary>
        public override void Start()
        {
        }

        /// <summary>
        /// Updates the creature behaviour.
        /// </summary>
        public override void Update()
        {
            if (OwningCreatureAI.reachedEndOfPath && !IsDone && foundViable)
            {
                OwningCreatureAI.SetAnimation("Idle");
                Done();
            }       
            else if (!foundViable && !IsDone)
            {
                ResourceType resource = (ResourceType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(ResourceType)).Length);
                Vector3 coord = Vector3.zero;
                if (FindClosestPathToResource(resource, out coord))
                {
                    OwningCreatureAI.SetAnimation("Moving");
                    OwningCreatureAI.destination = coord;
                    foundViable = true;
                }
                else if (curTry >= maxTries)
                {
                    OwningCreatureAI.SetAnimation("Idle");
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
            MoveToClosestResourceOfType other = (MoveToClosestResourceOfType)this.MemberwiseClone();
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