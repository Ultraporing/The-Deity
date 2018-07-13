using Assets.Scripts.AI.Creature.Villager;
using Assets.Scripts.Constructions;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using System;
using UnityEngine;

namespace Assets.Scripts.Constructions
{
    //Tobias Lenz, Lea Kohl

    /// <summary>
    /// House with Residents and is responsible to spawn more Villagers
    /// </summary>
    public class LoveHouse : House
    {
        public int m_SpawnDelayMin = 0;
        public int m_SpawnDelayMax = 0;
        public float m_SpawnDist = 2;
        public float m_SpawnDelay = 0;

        public GameObject m_VillagerPrefab = null;

        public float m_Timer = 0f;
        float m_RandomDelay = 0f;
        System.Random m_Random = new System.Random(DateTime.Now.Millisecond);
        bool m_OneThingSpawned;
        
        /// <summary>
        /// Constructor of the LoveHouse
        /// </summary>
        public LoveHouse()
        {
            m_MaxResidents = 4;
        }

        /// <summary>
        /// Initialize with default values
        /// </summary>
        void Start()
        {
            m_RandomDelay = m_Random.Next(0, 61);
            m_SpawnDelay = Mathf.Clamp(m_SpawnDelayMin + m_RandomDelay, m_SpawnDelayMin, m_SpawnDelayMax);
            GetComponent<House>().Index = 1;
            Index = GetComponent<House>().Index;
        }

        /// <summary>
        /// Update the LoveHouse and try to spawn villagers based on the delay
        /// </summary>
        void Update()
        {
            if (TotalVillagerCapacity <= PlanetDatalayer.Instance.GetManager<VillagerManager>().NumVillagers)
            {
                m_Timer = 0;
                return;
            }

            m_SpawnDelay-= Time.deltaTime;
            if (m_SpawnDelay <= 0)
            {
                GameObject Go = Instantiate(m_VillagerPrefab, transform.position + transform.right * m_SpawnDist, Quaternion.identity);
                VillagerSpecialization specialization = (VillagerSpecialization)m_Random.Next(0, 2);
                Go.GetComponent<VillagerAI>().m_VillagerSpecialization = specialization;
                if (!m_OneThingSpawned && PlanetDatalayer.Instance.GetManager<GoalManager>().m_CycleNumber == 3)
                {
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_NewVillager = true;
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
                    m_OneThingSpawned = true;
                }
                m_RandomDelay = m_Random.Next(10, 60);
                m_SpawnDelay = Mathf.Clamp(m_SpawnDelayMin + m_RandomDelay, m_SpawnDelayMin, m_SpawnDelayMax);
                m_Timer = 0;

            }
        }
    }
}