/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.AI.Creature.Behaviours.Villager;
using Assets.Scripts.Constructions;
using Assets.Scripts.AI.Creature.Villager;

namespace Assets.Scripts.AI.Creature.Behaviours
{
    /// <summary>
    /// AI Behaviour Block telling the Creature to Find the closest Bonfire and walk to it.
    /// </summary>
    public class GoToBonfire : BehaviourBlock
    {
        public GoToBonfire(CreatureAI owningCreatureAI) : base(owningCreatureAI,
            new MoveToClosestBuildingOfType<Bonfire>(owningCreatureAI))
        {

        }
    }
}