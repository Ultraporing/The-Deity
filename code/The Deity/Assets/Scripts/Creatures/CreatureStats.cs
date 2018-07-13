/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;
using Assets.Scripts.AI.Creature;
using System;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using Assets.Scripts.AI.Creature.Villager;

namespace Assets.Scripts.Creatures
{
    /// <summary>
    /// Baseclass for creature statistics. Contains Health, Food, Drink and Starvation mechanics
    /// </summary>
    [Serializable]
    public abstract class CreatureStats
    {
        /// <summary>
        /// Type of the Creature
        /// </summary>
        public enum CreatureType
        {
            Wildlife,
            Villager
        }

        public float m_MaxHealth = 100;
        public float m_CurHealth = 100;
        public float m_FoodLevel = 60f;
        public float m_DrinkLevel = 60f;
        public readonly float m_GettingHungryRatePerSec = 0.1f;
        public readonly float m_GettingThirstyRatePerSec = 0.3f;
        public readonly float m_StarvingDamageRatePerSec = 2f;
        public readonly CreatureType m_CreatureType;

        private readonly CreatureAI m_OwnerAI;

        /// <summary>
        /// Constructor of the CreatureStats
        /// </summary>
        /// <param name="creatureType">Type of the Creature</param>
        /// <param name="creatureAI">Reference to the CreatureAI of the Creature</param>
        public CreatureStats(CreatureType creatureType, CreatureAI creatureAI)
        {
            m_OwnerAI = creatureAI;
            m_CreatureType = creatureType;
        }

        /// <summary>
        /// Checks if the Creature needs food
        /// </summary>
        /// <returns>true if it needs food</returns>
        public bool NeedsFood()
        {
            return m_FoodLevel <= 30;
        }

        /// <summary>
        /// Checks if the Creature needs drink
        /// </summary>
        /// <returns>true if it needs drink</returns>
        public bool NeedsDrink()
        {
            return m_DrinkLevel <= 30;
        }

        /// <summary>
        /// Checks if the Creature is starving
        /// </summary>
        /// <returns>true if it is starving</returns>
        public bool IsStarving()
        {
            return m_FoodLevel <= 0 || m_DrinkLevel <= 0;
        }

        /// <summary>
        /// Applies damage to the creature and kills it if the HP reach 0
        /// </summary>
        /// <param name="dmgAmount">Amount to apply</param>
        public void TakeDamage(float dmgAmount)
        {
            m_CurHealth = Mathf.Clamp(m_CurHealth - dmgAmount, 0, m_MaxHealth);
            if (m_CurHealth <= 0)
                Die();
        }

        /// <summary>
        /// Checks if the Creatures food is full
        /// </summary>
        /// <returns>true if it is full</returns>
        public bool IsFoodFull()
        {
            return m_FoodLevel >= 90;
        }

        /// <summary>
        /// Add food to the Creature
        /// </summary>
        /// <param name="amount">Amount to add</param>
        public void AddFood(int amount)
        {
            m_FoodLevel = Mathf.Clamp(m_FoodLevel + amount, 0, 100);
        }

        /// <summary>
        /// Checks if the Creatures drink is full
        /// </summary>
        /// <returns>true if it is full</returns>
        public bool IsDrinkFull()
        {
            return m_DrinkLevel >= 90;
        }

        /// <summary>
        /// Add drink to the Creature
        /// </summary>
        /// <param name="amount">Amount to add</param>
        public void AddDrink(int amount)
        {
            m_DrinkLevel = Mathf.Clamp(m_DrinkLevel + amount, 0, 100);
        }

        /// <summary>
        /// Kill the Creature and destroy it
        /// </summary>
        public virtual void Die()
        {
            GameObject.Destroy(m_OwnerAI.gameObject);
        }

        /// <summary>
        /// Updates the Creature and checks for food/drink levels as well as apply damage if it is starving
        /// </summary>
        public virtual void Update()
        {
            if (m_CreatureType == CreatureType.Villager)
            {
                m_FoodLevel = Mathf.Clamp(m_FoodLevel - m_GettingHungryRatePerSec * Time.deltaTime, 0, 100);
                m_DrinkLevel = Mathf.Clamp(m_DrinkLevel - m_GettingThirstyRatePerSec * Time.deltaTime, 0, 100);

                if (IsStarving())
                {
                    TakeDamage(m_StarvingDamageRatePerSec * Time.deltaTime);
                    PlanetDatalayer.Instance.GetManager<FoMManager>().IncreaseFoM(PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList.IndexOf((VillagerAI)m_OwnerAI), -0.01f);
                }
            }
        }
    }
}