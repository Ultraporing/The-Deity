/*
    Written by Tobias Lenz
 */

using Assets.Scripts.AI.Creature.Villager;
using Assets.Scripts.Constructions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.AI.Creature.Behaviours.Villager
{
    /// <summary>
    /// AI behaviour telling the creature to bring the resources inside it's inventory to the Bonfire
    /// </summary>
    public class DepositResourcesIntoBonfire : CreatureBehaviour
    {
        Bonfire m_Bonfire = null;

        /// <summary>
        /// Constructor of the Behaviour
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour.</param>
        public DepositResourcesIntoBonfire(CreatureAI owningCreatureAI) : base(owningCreatureAI)
        {
            
        }

        /// <summary>
        /// Starts the execution of the creature behaviour.
        /// </summary>
        public override void Start()
        {
            Vector3 coord = Vector3.zero;
            if (FindClosestPathToBuildingOfType<Bonfire>(out coord, out m_Bonfire))
            {
                OwningCreatureAI.SetAnimation("Moving");
                OwningCreatureAI.destination = coord;
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
        /// Updates the creature behaviour.
        /// </summary>
        public override void Update()
        {
            if (OwningCreatureAI.reachedEndOfPath && !IsDone)
            {
                Inventory inventory = ((VillagerAI)OwningCreatureAI).GetInventory();
                int numWood = inventory.GetTotalAmountOfResource(Resources.ResourceType.Wood);
                int numStone = inventory.GetTotalAmountOfResource(Resources.ResourceType.Rock);

                if (numWood > 0)
                {
                    inventory.RemoveResource(Resources.ResourceType.Wood, numWood);
                    m_Bonfire.m_ResourceInventory.AddResource(Resources.ResourceType.Wood, numWood);
                }
                if (numStone > 0)
                {
                    inventory.RemoveResource(Resources.ResourceType.Rock, numStone);
                    m_Bonfire.m_ResourceInventory.AddResource(Resources.ResourceType.Rock, numStone);
                }

                Done();
            }
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
