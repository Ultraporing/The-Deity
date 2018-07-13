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
    /// AI Behaviour Block telling the Creature to Find a particular resource of resourceType, 
    /// extract as much as possible and finally deposit the extracted resources in the bonfire.
    /// </summary>
    public class FindAndExtractResource : BehaviourBlock
    {
        /// <summary>
        /// Constructor of the Behaviour Block
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour Block.</param>
        /// <param name="resourceType">The resource type to find and extract.</param>
        public FindAndExtractResource(CreatureAI owningCreatureAI, Resources.ResourceType resourceType) : base(owningCreatureAI,
            new MoveToClosestResourceOfType(owningCreatureAI, resourceType),
            new ExtractResource(owningCreatureAI, resourceType),
            new MoveToClosestBuildingOfType<Bonfire>(owningCreatureAI),
            new DepositResourcesIntoBonfire(owningCreatureAI))
        {

        }
        
    }
}