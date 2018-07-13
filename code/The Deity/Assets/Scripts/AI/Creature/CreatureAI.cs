/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using Pathfinding;
using Assets.Scripts.Creatures;
using System.Collections.Generic;
using Assets.Scripts.AI.Creature.Behaviours;
using Assets.Scripts.Animations;
using Assets.Scripts.Constructions;
using System.Linq;
using Assets.Scripts.Environment.Planet;
using Assets.Scripts.Resources;
using System;
using Assets.Scripts.AI.Creature.Behaviours.Villager;
using Assets.Scripts.AI.Creature.Villager;

namespace Assets.Scripts.AI.Creature
{
    /// <summary>
    /// Baseclass for the AI, it inherits from AIPath which is part of the A*Pathfinding Plugin.
    /// It contains all necessary references for the creature to be animated, play sounds and execute behaviour blocks.
    /// </summary>
    public abstract class CreatureAI : AIPath
    {
        public CreatureStats CreatureStats = null;

        protected AnimationContainer CurrentAnimationState = null;
        protected LinkedList<BehaviourBlock> BehaviourBlockQueue = new LinkedList<BehaviourBlock>();
        protected BehaviourBlock CurrentCreatureBehaviourBlock = null;
        protected abstract AnimationStates CreatureAnimationStates { get; }
        protected Animator CreatureAnimator = null;
        protected Vector3 LastPosition = Vector3.zero;
        protected BehaviourBlock LastCompletedBehaviourBlock = null;
        protected AudioSource FXAudioSource = null;
        protected CreatureFX CreatureFX;

        /// <summary>
        /// Gets the current Animation
        /// </summary>
        /// <returns>The Animation</returns>
        public AnimationContainer GetCurrentAnimation()
        {
            return CurrentAnimationState;
        }

        /// <summary>
        /// Initialize necessary variables and get needed Unity components
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            CreatureAnimator = GetComponent<Animator>();
            FXAudioSource = GetComponent<AudioSource>();
            LastPosition = transform.position;
            CreatureFX = FindObjectOfType<CreatureFX>();
        }

        /// <summary>
        /// Calls the inherited start Method
        /// </summary>
        protected override void Start()
        {
            base.Start();
        }

        /// <summary>
        /// Checks if creature is already planning to eat.
        /// </summary>
        /// <returns>true if its going to eat.</returns>
        protected bool IsAlreadyGoingToEat()
        {
            if (CurrentCreatureBehaviourBlock == null)
                return false;

            if (CurrentCreatureBehaviourBlock.GetType() == typeof(FindAndEat))
            {
                return true;
            }
            else
            {
                foreach (BehaviourBlock bh in BehaviourBlockQueue)
                {
                    if (bh.GetType() == typeof(FindAndEat))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if creature is already planning to drink.
        /// </summary>
        /// <returns>true if its going to drink.</returns>
        protected bool IsAlreadyGoingToDrink()
        {
            if (CurrentCreatureBehaviourBlock == null)
                return false;

            if (CurrentCreatureBehaviourBlock.GetType() == typeof(FindAndDrink))
            {
                return true;
            }
            else
            {
                foreach (BehaviourBlock bh in BehaviourBlockQueue)
                {
                    if (bh.GetType() == typeof(FindAndDrink))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Updates the Creature and checks if it needs food/water.
        /// Updates the behaviour block if there is none, add the GoToBonfire Behaviour block.
        /// </summary>
        protected override void Update()
        {
            base.Update();
            CreatureStats.Update();

            if (CreatureStats.NeedsFood() && !IsAlreadyGoingToEat())
            {
                BehaviourBlockQueue.AddFirst(new FindAndEat(this));
                if (CurrentCreatureBehaviourBlock != null)
                {
                    if (CurrentCreatureBehaviourBlock.GetType() != typeof(FindAndDrink) && CurrentCreatureBehaviourBlock.GetType() != typeof(FindAndEat))
                    {
                        CurrentCreatureBehaviourBlock = null;
                    }
                }
            }

            if (CreatureStats.NeedsDrink() && !IsAlreadyGoingToDrink())
            {
                BehaviourBlockQueue.AddFirst(new FindAndDrink(this));
                if (CurrentCreatureBehaviourBlock != null)
                {
                    if (CurrentCreatureBehaviourBlock.GetType() != typeof(FindAndDrink) && CurrentCreatureBehaviourBlock.GetType() != typeof(FindAndEat))
                    {
                        CurrentCreatureBehaviourBlock = null;
                    }
                }
            }

            if (CurrentCreatureBehaviourBlock != null)
            {
                if (!CurrentCreatureBehaviourBlock.AllDone())
                {
                    CurrentCreatureBehaviourBlock.Update();
                }
                else
                {
                    LastCompletedBehaviourBlock = CurrentCreatureBehaviourBlock;
                    CurrentCreatureBehaviourBlock = null;
                }
            }
            else
            {
                if (BehaviourBlockQueue.Count > 0)
                {
                    CurrentCreatureBehaviourBlock = BehaviourBlockQueue.First().DeepCopy();
                    BehaviourBlockQueue.RemoveFirst();
                }
                else if (BehaviourBlockQueue.Count <= 0 && this.GetType() == typeof(VillagerAI))
                {
                    if (LastCompletedBehaviourBlock != null)
                        if (LastCompletedBehaviourBlock.GetType() != typeof(MoveToClosestBuildingOfType<Bonfire>))
                            BehaviourBlockQueue.AddFirst(new GoToBonfire(this));
                        else
                            BehaviourBlockQueue.AddFirst(new GoToBonfire(this));
                }
            }

            LastPosition = transform.position;
        }

        /// <summary>
        /// Sets an animation and plays the corrisponding sound effect
        /// </summary>
        /// <param name="animationName">Name of the Animation</param>
        public virtual void SetAnimation(string animationName)
        {
            CurrentAnimationState = CreatureAnimationStates.TryGetAnimationState(animationName);
            CreatureAnimator.SetInteger("Animation", CurrentAnimationState.AnimationID);           
            FXAudioSource.clip = CreatureFX.GetClip(animationName);
            FXAudioSource.loop = true;
            FXAudioSource.Play();
        }
    }
}