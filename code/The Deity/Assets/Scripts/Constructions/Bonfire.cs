/*
    Written by Tobias Lenz
 */

using Assets.Scripts.Environment.Planet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Constructions
{
    /// <summary>
    /// Defines the Bonfire Building
    /// </summary>
    public class Bonfire : Building
    {
        private Transform m_FireOnBonfire = null;
        public float m_MaxFuel = 100f;
        public float m_CurFuel = 5f;
        public bool m_IsBurning = false;
        public float m_FuelBurnRatePerSec = 0.33f;
        public Inventory m_ResourceInventory = new Inventory();

        /// <summary>
        /// Initialize the default variables and get the needed Unity Objects
        /// </summary>
        void Start()
        {
            m_FireOnBonfire = transform.Find("TinyFireVisual");
            m_ResourceInventory.MaxAmountPerSlot = 100;
        }

        /// <summary>
        /// Updates the Bonfire
        /// </summary>
        void Update()
        {
            if (m_IsBurning)
            {
                if (m_CurFuel <= 0)
                {
                    m_IsBurning = false;
                    m_FireOnBonfire.gameObject.SetActive(false);
                    m_CurFuel = 0;
                }
                else
                {
                    m_CurFuel -= m_FuelBurnRatePerSec * Time.deltaTime;
                }

                if (PlanetDatalayer.Instance.GetManager<GoalManager>().m_BurningFire == false)
                {
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_BurningFire = true;
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
                }
            }
            else
            {
               if(m_ResourceInventory.GetTotalAmountOfResource(Resources.ResourceType.Wood) > 10 && m_CurFuel <= 0.2f)
                {
                    int wood = m_ResourceInventory.RemoveResource(Resources.ResourceType.Wood, 10);
                    AddWood(wood);
                    StartFire();
                }
            }
        }

        /// <summary>
        /// Adds wood to the Bonfire
        /// </summary>
        /// <param name="amount">Amount of wood to add</param>
        public void AddWood(int amount)
        {
            m_CurFuel = Mathf.Clamp(m_CurFuel + amount * ResourceDetails.WoodDetails.FuelProvided, 0, 100);
        }

        /// <summary>
        /// Start the Fire
        /// </summary>
        public void StartFire()
        {
            if (m_CurFuel > 0)
            {
                m_IsBurning = true;
                m_FireOnBonfire.gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// Checks for collisions with the Fire Object and start the fire
        /// </summary>
        /// <param name="collision">Collision triggering the Event</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.tag == "Fire")
            {
                StartFire();
                Destroy(collision.collider.gameObject);
            }
        }
    }
}