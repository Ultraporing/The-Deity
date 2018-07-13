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
    /// AI behaviour telling the creature to drink until it's full
    /// </summary>
    public class DrinkUntillFull : CreatureBehaviour
    {
        float DrinkTimer = 0;

        /// <summary>
        /// Constructor of the Behaviour
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour.</param>
        public DrinkUntillFull(CreatureAI owningCreatureAI) : base(owningCreatureAI)
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
            if (OwningCreatureAI.reachedEndOfPath && !IsDone && OwningCreatureAI.CreatureStats.IsDrinkFull())
            {
                OwningCreatureAI.SetAnimation("Idle");
                Done();
            }              
            else if (!IsDone)
            {
                if (DrinkTimer >= 1)
                {
                    if (OwningCreatureAI.GetCurrentAnimation().AnimationName != "Extracting")
                    {
                        OwningCreatureAI.SetAnimation("Extracting");
                    }

                    OwningCreatureAI.CreatureStats.AddDrink(10);
                    DrinkTimer = 0;
                }
                else
                {
                    DrinkTimer += Time.deltaTime;
                }
            }
        }

        /// <summary>
        /// Provides a deep copy of the creature behaviour.
        /// </summary>
        /// <returns>the new creature behaviour</returns>
        public override CreatureBehaviour DeepCopy()
        {
            DrinkUntillFull other = (DrinkUntillFull)this.MemberwiseClone();
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