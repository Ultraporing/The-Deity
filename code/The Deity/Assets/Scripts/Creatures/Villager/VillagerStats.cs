/*
    Written by Tobias Lenz
 */

using Assets.Scripts.AI.Creature;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Creatures.Villager
{
    /// <summary>
    /// Inherits from CreatureStats and contains the Villager Inventory as well as Heretic flags
    /// </summary>
    [Serializable]
    public class VillagerStats : CreatureStats
    {
        public Inventory VillagerInventory = new Inventory();
        private bool m_IsHeretic = false;
        private Transform m_HereticIndicator = null;

        /// <summary>
        /// Gets/Sets Heretic indicator
        /// </summary>
        public bool IsHeretic
        {
            get
            {
                return m_IsHeretic;
            }
            set
            {
                if (m_HereticIndicator != null)
                {
                    if (m_HereticIndicator.gameObject.activeSelf && !value)
                        m_HereticIndicator.gameObject.SetActive(false);
                    else if (!m_HereticIndicator.gameObject.activeSelf && value)
                        m_HereticIndicator.gameObject.SetActive(true);
                }
                m_IsHeretic = value;               
            }
        }

        /// <summary>
        /// Villager Constructor
        /// </summary>
        /// <param name="creatureAI">CreatureAI of the Villager</param>
        /// <param name="hereticIndicator">Reference to the Heretic Indicator</param>
        public VillagerStats(CreatureAI creatureAI, Transform hereticIndicator) : base(CreatureType.Villager, creatureAI)
        {
            VillagerInventory.MaxAmountPerSlot = 10;
            VillagerInventory.NumSlots = 1;
            m_HereticIndicator = hereticIndicator;
            IsHeretic = false;
        }
    }
}