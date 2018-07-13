/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.Constructions;

namespace Assets.Scripts.AI.Creature.Behaviours.Villager
{
    /// <summary>
    /// AI behaviour telling the creature to move to the closest building with this type T for example Bonfire.
    /// </summary>
    public class MoveToClosestBuildingOfType<T> : CreatureBehaviour where T : Building
    {
        public T TargetBuilding = null;
        float m_Timer = 0;
        Vector3 m_StartPos = Vector3.zero;

        /// <summary>
        /// Constructor of the Behaviour
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour.</param>
        public MoveToClosestBuildingOfType(CreatureAI owningCreatureAI) : base(owningCreatureAI)
        {
            
        }

        /// <summary>
        /// Starts the execution of the creature behaviour.
        /// </summary>
        public override void Start()
        {
            m_StartPos = OwningCreatureAI.position;
            Vector3 coord = Vector3.zero;
            if (FindClosestPathToBuildingOfType<T>(out coord, out TargetBuilding))
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
            m_Timer += Time.deltaTime;

            if (OwningCreatureAI.GetCurrentAnimation().AnimationName != "Moving")
                OwningCreatureAI.SetAnimation("Moving");

            if (OwningCreatureAI.reachedEndOfPath && !IsDone)
                Done();

            // if you didnt move more than 2 units in the last 10sec then you are probably stuck, so just call done
            if (m_Timer >= 10)
                if ((m_StartPos - OwningCreatureAI.position).magnitude < 2f)
                    Done();          
        }

        /// <summary>
        /// Provides a deep copy of the creature behaviour.
        /// </summary>
        /// <returns>the new creature behaviour</returns>
        public override CreatureBehaviour DeepCopy()
        {
            MoveToClosestBuildingOfType<T> other = (MoveToClosestBuildingOfType<T>)this.MemberwiseClone();
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