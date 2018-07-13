/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.AI.Creature.Behaviours.Villager;
using Assets.Scripts.Constructions;

namespace Assets.Scripts.AI.Creature.Behaviours
{
    /// <summary>
    /// AI Behaviour Block telling the Creature to Find Water and drink until it's full
    /// </summary>
    public class FindAndDrink : BehaviourBlock
    {
        /// <summary>
        /// Constructor of the Behaviour Block
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour Block.</param>
        public FindAndDrink(CreatureAI owningCreatureAI) : base(owningCreatureAI,
            new MoveToClosestResourceOfType(owningCreatureAI, Resources.ResourceType.Water),
            new DrinkUntillFull(owningCreatureAI))
        {

        }
        
    }
}