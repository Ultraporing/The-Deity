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
    /// AI Behaviour Block telling the Creature to Find Food and eat until it's full
    /// </summary>
    public class FindAndEat : BehaviourBlock
    {
        /// <summary>
        /// Constructor of the Behaviour Block
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour Block.</param>
        public FindAndEat(CreatureAI owningCreatureAI) : base(owningCreatureAI,
            new MoveToClosestResourceOfType(owningCreatureAI, Resources.ResourceType.Food),
            new EatUntillFull(owningCreatureAI))
        {

        }
        
    }
}