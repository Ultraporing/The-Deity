/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace Assets.Scripts.Animations
{
    /// <summary>
    /// Contains the Animation Name and ID
    /// </summary>
    [Serializable]
    public class AnimationContainer
    {
        public string AnimationName;
        public int AnimationID;
    }

    /// <summary>
    /// Holds all possible Animations a Creature can have
    /// </summary>
    public class AnimationStates
    {
        private AnimationContainer[] m_AnimationStateList;

        /// <summary>
        /// Constructor of the Animation States
        /// </summary>
        /// <param name="initialAnimationStates">A Array of Animation Containers defining the initial Animations</param>
        public AnimationStates(params AnimationContainer[] initialAnimationStates)
        {
            m_AnimationStateList = initialAnimationStates;
        }

        /// <summary>
        /// Tries to get the Animation Container by Name
        /// </summary>
        /// <param name="animationName">Name of the Animation Container</param>
        /// <returns>The container if it is found, else null</returns>
        public AnimationContainer TryGetAnimationState(string animationName)
        {
            if (m_AnimationStateList.Length == 0)
            {
                Debug.LogErrorFormat("AnimationContainer List is Empty, could not locate Animation with name: {0}", animationName);
                return null;
            }
 
            foreach (AnimationContainer anim in m_AnimationStateList)
            {
                if (anim.AnimationName == animationName)              
                    return anim;
            }

            Debug.LogErrorFormat("Failed to find Animation with name: {0}", animationName);
            return null;
        }

        /// <summary>
        /// Tries to get the Animation Container by ID
        /// </summary>
        /// <param name="animationID">ID of the Animation Container</param>
        /// <returns>The container if it is found, else null</returns>
        public AnimationContainer TryGetAnimationState(int animationID)
        {
            if (m_AnimationStateList.Length == 0)
            {
                Debug.LogErrorFormat("AnimationContainer List is Empty, could not locate Animation with ID: {0}", animationID);
                return null;
            }

            foreach (AnimationContainer anim in m_AnimationStateList)
            {
                if (anim.AnimationID == animationID)
                    return anim;
            }

            Debug.LogErrorFormat("Failed to find Animation with ID: {0}", animationID);
            return null;
        }
    }
}
