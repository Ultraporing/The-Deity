/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.AI.Creature.Behaviours;
using Assets.Scripts.Resources;
using Assets.Scripts.AI.Creature;

namespace Assets.Scripts.AI.Creature.Behaviours.Villager
{
    /// <summary>
    /// AI behaviour telling the creature to move to the closest resource with this resourceType
    /// </summary>
    public class MoveToClosestResourceOfType : CreatureBehaviour
    {
        private ResourceType m_TargetResourceType;

        /// <summary>
        /// Constructor of the Behaviour
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour.</param>
        /// <param name="resourceType">The resource type to move to.</param>
        public MoveToClosestResourceOfType(CreatureAI owningCreatureAI, ResourceType resourceType) : base(owningCreatureAI)
        {
            m_TargetResourceType = resourceType;
        }

        /// <summary>
        /// Starts the execution of the creature behaviour.
        /// </summary>
        public override void Start()
        {
            Vector3 coord = Vector3.zero;
            if (FindClosestPathToResource(m_TargetResourceType, out coord))
            {
                OwningCreatureAI.SetAnimation("Moving");
                OwningCreatureAI.destination = coord;
            }
            else
            {
                OwningCreatureAI.SetAnimation("Idle");
                Done();
            }
        }

        /// <summary>
        /// Updates the creature behaviour.
        /// </summary>
        public override void Update()
        {
            if (OwningCreatureAI.GetCurrentAnimation().AnimationName != "Moving")
                OwningCreatureAI.SetAnimation("Moving");

            if (OwningCreatureAI.reachedEndOfPath && !IsDone)
                Done();
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