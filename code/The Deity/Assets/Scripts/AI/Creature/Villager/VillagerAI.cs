/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using Pathfinding;
using Assets.Scripts.Resources;
using Assets.Scripts.Environment.Planet;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Helper.Threads;
using System;
using Assets.Scripts.Constructions;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Creatures;
using Assets.Scripts.Animations;
using Assets.Scripts.AI.Creature.Behaviours;

namespace Assets.Scripts.AI.Creature.Villager
{
    /// <summary>
    /// Villager specialisations
    /// </summary>
    public enum VillagerSpecialization
    {
        WoodCutter,
        StoneMason,
        Builder

    }

    /// <summary>
    /// Inherits from CreatureAI and is the Villagers Main controlling Class
    /// </summary>
    public class VillagerAI: CreatureAI
    {
        public GameObject m_Torch = null;
        public Light m_Sun = null;
        public bool m_HasTorch = false;
        public VillagerSpecialization m_VillagerSpecialization = VillagerSpecialization.WoodCutter;
        public string currentBlock, lastCompletedBlock = "";
        public House m_Home = null;
                
        /// <summary>
        /// Provides the Villager Animation States
        /// </summary>
        protected override AnimationStates CreatureAnimationStates
        {
            get
            {
                return new AnimationStates(
                    new AnimationContainer() { AnimationID = 0, AnimationName = "Idle" },
                    new AnimationContainer() { AnimationID = 1, AnimationName = "Moving" },
                    new AnimationContainer() { AnimationID = 2, AnimationName = "Extracting" }
                );
            }
        }

        /// <summary>
        /// Deregister Villager from Manager when destroyed
        /// </summary>
        private void OnDestroy()
        {
            DeregisterVillager();
        }

        /// <summary>
        /// Register Villager with Manager
        /// </summary>
        protected virtual void RegisterVillager()
        {
            PlanetDatalayer.Instance.GetManager<VillagerManager>().RegisterVillager(this);
            List<House> houses = PlanetDatalayer.Instance.GetManager<BuildingManager>().GetBuildingsOfType<House>();
            foreach (House h in houses)
            {
                if (h.AddResident(this))
                    break;
            }
            PlanetDatalayer.Instance.GetManager<FoMManager>().m_FoMValues.Add(PlanetDatalayer.Instance.GetManager<FoMManager>().FoMAssignment());
        }

        /// <summary>
        /// Deregister Villager with Manager
        /// </summary>
        protected virtual void DeregisterVillager()
        {
            int index = PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList.IndexOf(this);
            float number = PlanetDatalayer.Instance.GetManager<FoMManager>().m_FoMValues[index];
            PlanetDatalayer.Instance.GetManager<VillagerManager>().DeregisterVillager(this);

            if (m_Home != null)
                m_Home.RemoveResident(this);
            PlanetDatalayer.Instance.GetManager<FoMManager>().m_FoMValues.Remove(number);
        }

        /// <summary>
        /// Gets the Villager Stats
        /// </summary>
        /// <returns>The Villager Stats</returns>
        public VillagerStats GetVillagerStats()
        {
            return (VillagerStats)CreatureStats;
        }

        /// <summary>
        /// Gets the Villager Inventory
        /// </summary>
        /// <returns>The Inventory</returns>
        public Inventory GetInventory()
        {
            return GetVillagerStats().VillagerInventory;
        }

        /// <summary>
        /// Initialize necessary variables and get needed Unity components. Then Register the Villager.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            CreatureStats = new VillagerStats(this, transform.Find("HereticIndicator"));
            m_Sun = FindObjectOfType<AutoIntensity>().GetComponent<Light>();
            RegisterVillager();
        }

        /// <summary>
        /// Calls the base Start Method and sets the Animation as well as the Behaviour to its default values.
        /// </summary>
        protected override void Start()
        {
            base.Start();
            SetAnimation("Idle");
            BehaviourBlockQueue.AddFirst(new GoToBonfire(this));

            switch(m_VillagerSpecialization)
            {
                case VillagerSpecialization.WoodCutter:
                    BehaviourBlockQueue.AddLast(new FindAndExtractResource(this, ResourceType.Wood));
                    break;
                case VillagerSpecialization.StoneMason:
                    BehaviourBlockQueue.AddLast(new FindAndExtractResource(this, ResourceType.Rock));
                    break;
            }
        }

        /// <summary>
        /// Updates the Villager, checks for a free home if he is homeless and gets one if there is a free spot in the house.
        /// Activates and deactivates the torch based on the time of day as well as add the default work behaviour block if the villager got nothing todo.
        /// </summary>
        protected override void Update()
        {
            base.Update();

            if (m_Home == null)
            {
                List<House> houses = PlanetDatalayer.Instance.GetManager<BuildingManager>().GetBuildingsOfType<House>();
                foreach (House h in houses)
                {
                    if (h.AddResident(this))
                    {
                        m_Home = h;
                        PlanetDatalayer.Instance.GetManager<FoMManager>().IncreaseFoM(PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList.IndexOf(this), 10);
                        break;
                    }    
                }
            }

            if (CurrentCreatureBehaviourBlock != null)
                currentBlock = CurrentCreatureBehaviourBlock.GetType().Name;
            else
                currentBlock = "None";

            if (LastCompletedBehaviourBlock != null)
                lastCompletedBlock = LastCompletedBehaviourBlock.GetType().Name;
            else
                lastCompletedBlock = "None";

            if (m_Torch != null)
            {
                if (m_Sun != null)
                {
                    if (m_HasTorch)
                    {
                        if (m_Sun.intensity <= 0.6f && !m_Torch.activeInHierarchy)
                        {
                            m_Torch.SetActive(true);
                        }
                        else if (m_Torch.activeInHierarchy && m_Sun.intensity > 0.6f)
                        {
                            m_Torch.SetActive(false);
                        }
                    }
                }
            }

            if (BehaviourBlockQueue.Count <= 0 && CurrentCreatureBehaviourBlock == null)
            {
                switch (m_VillagerSpecialization)
                {
                    case VillagerSpecialization.WoodCutter:
                        BehaviourBlockQueue.AddLast(new FindAndExtractResource(this, ResourceType.Wood));
                        break;
                    case VillagerSpecialization.StoneMason:
                        BehaviourBlockQueue.AddLast(new FindAndExtractResource(this, ResourceType.Rock));
                        break;
                }
            }
        }

        /// <summary>
        /// Gets called when the Villager reached its target
        /// </summary>
        public override void OnTargetReached()
        {
            if (CurrentCreatureBehaviourBlock != null)
            {
                if (CurrentCreatureBehaviourBlock.m_CurrentBehaviour != null)
                {
                    base.OnTargetReached();
                }
            }
        }
    }
}