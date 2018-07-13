/*
    Written by Tobias Lenz
 */

using Assets.Scripts.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// General Inventory Implementation used by Villagers and Buildings
    /// </summary>
    [Serializable]
    public class Inventory
    {
        /// <summary>
        /// Contains the Resource Type and Amount
        /// </summary>
        [Serializable]
        public class ResourceSlot
        {
            public ResourceType ResourceType;
            public uint Amount;
        }

        public uint NumSlots = 1;
        public int MaxAmountPerSlot = 10;
        public List<ResourceSlot> ResourceInventory = new List<ResourceSlot>();

        /// <summary>
        /// Add a certain amount of Resource into the inventory
        /// </summary>
        /// <param name="resourceType">Type of the Resource</param>
        /// <param name="amount">Amount to add</param>
        /// <returns>Amount added</returns>
        public int AddResource(ResourceType resourceType, int amount)
        {
            int added = 0;
            int toAdd = amount;
            ResourceSlot rs = null;
            while ((rs = FindNextSlotWithSpace(resourceType)) != null && toAdd != 0)
            {
                int leftToAdd = (int)rs.Amount + toAdd;
                if (leftToAdd > MaxAmountPerSlot)
                {
                    toAdd = leftToAdd - MaxAmountPerSlot;
                    added += (int)(MaxAmountPerSlot - rs.Amount);
                    rs.Amount = (uint)MaxAmountPerSlot;
                    toAdd = 0;
                }
                else
                {
                    rs.Amount += (uint)Mathf.Clamp(rs.Amount + toAdd, 0, MaxAmountPerSlot); ;
                    added += toAdd;
                    toAdd = 0;
                }
            }

            return added;
        }

        /// <summary>
        /// Remove a certain amount of Resource from the inventory
        /// </summary>
        /// <param name="resourceType">Type of the Resource</param>
        /// <param name="amount">Amount to remove</param>
        /// <returns>Amount removed</returns>
        public int RemoveResource(ResourceType resourceType, int amount)
        {
            int removed = 0;
            int toRemove = amount;
            ResourceSlot rs = null;
            while ((rs = FindNextSlotWithResource(resourceType)) != null && toRemove != 0)
            {
                int leftToRemove = (int)rs.Amount - toRemove;
                if (leftToRemove < 0)
                {
                    toRemove = amount - Math.Abs(leftToRemove);
                    removed += toRemove;
                    rs.Amount = 0;
                    toRemove = 0;
                }
                else
                {
                    rs.Amount = (uint)Mathf.Clamp(rs.Amount - toRemove, 0, uint.MaxValue);
                    removed += toRemove;
                    toRemove = 0;
                }
            }

            return removed;
        }

        /// <summary>
        /// Finds the next ResourceSlot with space to accomidate the Resource Type or add a new slot when we are not at the maximum
        /// </summary>
        /// <param name="resourceType">Type of the Resource</param>
        /// <returns>Reference to the resource slot, otherwise null</returns>
        public ResourceSlot FindNextSlotWithSpace(ResourceType resourceType)
        {
            foreach (ResourceSlot rs in ResourceInventory)
            {
                if (rs.ResourceType != resourceType)
                    continue;

                if (rs.Amount < MaxAmountPerSlot)
                    return rs;
            }

            if (ResourceInventory.Count < NumSlots)
            {
                ResourceInventory.Add(new ResourceSlot() { ResourceType = resourceType, Amount = 0 });

                return ResourceInventory[ResourceInventory.Count - 1];
            }

            return null;
        }

        /// <summary>
        /// Finds the next ResourceSlot which contains the Resource
        /// </summary>
        /// <param name="resourceType">Type of the Resource</param>
        /// <returns>Reference to the resource slot, otherwise null</returns>
        public ResourceSlot FindNextSlotWithResource(ResourceType resourceType)
        {
            foreach (ResourceSlot rs in ResourceInventory)
            {
                if (rs.ResourceType == resourceType)
                    return rs;
            }

            return null;
        }

        /// <summary>
        /// Calculates the total amount of the resource type in the inventory
        /// </summary>
        /// <param name="resourceType">Type of the Resource</param>
        /// <returns>Total amount in the Inventory</returns>
        public int GetTotalAmountOfResource(ResourceType resourceType)
        {
            int total = 0;
            foreach (ResourceSlot rs in ResourceInventory)
            {
                if (rs.ResourceType == resourceType)
                    total += (int)rs.Amount;
            }

            return total;
        }
    }
}