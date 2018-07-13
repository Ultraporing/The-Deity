/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Assets.Scripts.AI.Creature.Behaviours
{
    /// <summary>
    /// Takes AI Behaviours to build and execute the Behaviour List, as well as the baseclass for AI BehaviourBlocks
    /// </summary>
    public abstract class BehaviourBlock
    {
        private Queue<CreatureBehaviour> m_Behaviours = new Queue<CreatureBehaviour>();
        public CreatureBehaviour m_CurrentBehaviour = null;
        protected CreatureAI m_OwningCreatureAI = null;

        /// <summary>
        /// Checks if all behaviours inside the block are done.
        /// </summary>
        /// <returns>true if all behaviours in the block are done.</returns>
        public bool AllDone()
        {
            bool allDone = m_Behaviours.Count <= 0 && m_CurrentBehaviour == null;
            if (allDone)
            {
                OnDone();
            }

            return allDone;
        }

        /// <summary>
        /// Constructor of the Behaviour Block
        /// </summary>
        /// <param name="owningCreatureAI">Reference to the creature owning the Behaviour Block.</param>
        /// <param name="behaviours">Parameterized Array of CreatureBehaviours that should be executed in order.</param>
        public BehaviourBlock(CreatureAI owningCreatureAI, params CreatureBehaviour[] behaviours)
        {
            m_OwningCreatureAI = owningCreatureAI;

            foreach (CreatureBehaviour cb in behaviours)
            {
                m_Behaviours.Enqueue(cb);
            }
        }

        /// <summary>
        /// Copys all variables and their states into a new Behaviour Block.
        /// </summary>
        /// <returns>The new Behaviour Block</returns>
        public BehaviourBlock DeepCopy()
        {
            BehaviourBlock other = (BehaviourBlock)this.MemberwiseClone();
            other.m_Behaviours = new Queue<CreatureBehaviour>(m_Behaviours);
            if (other.m_CurrentBehaviour != null)
            {
                other.m_CurrentBehaviour = m_CurrentBehaviour.DeepCopy();
            }

            return other;
        }

        /// <summary>
        /// Virtual function to be overridden by inheriting Behaviour blocks.
        /// Gets called when all behaviours in the block are done executing.
        /// </summary>
        protected virtual void OnDone()
        {

        }

        /// <summary>
        /// Update Method for the Behaviour Block.
        /// Checks if there is a current executing Behaviour if so then Update it, otherwise it gets a new one from the queue.
        /// </summary>
        public void Update()
        {
            if (m_CurrentBehaviour != null)
            {
                if (!m_CurrentBehaviour.IsDone)
                {
                    m_CurrentBehaviour.Update();
                }
                else
                {
                    m_CurrentBehaviour = null;
                }
            }
            else
            {
                if (m_Behaviours.Count > 0)
                {
                    m_CurrentBehaviour = m_Behaviours.Dequeue();
                    m_CurrentBehaviour.Start();
                }
            }
        }
    }
}