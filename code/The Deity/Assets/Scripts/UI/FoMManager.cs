using Assets.Scripts.Environment.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Creatures.Villager
{
    //Lea Kohl
    //FoM needs to be hihg in order to get lots of PoP to be ablöe to perform miracles
    public class FoMManager : IManager
    {
        //List of each villager's FoM values
        public List<float> m_FoMValues = new List<float>();
        System.Random m_Random = new System.Random(DateTime.Now.Millisecond);
        float m_OneFoMValue;
        //clamps FoM between 0 and 100
        public float m_MaxFoM = 100;
        float m_MinFoM = 0;
        public int m_CurrentFoM;
        float m_RandomRange;
        public float m_TimerCalculation = 0;
        //variables for GoalManager
        bool GoalChanged;
        bool GoalChanged2;

        //Update is only used to make sure the FoM is only tested every 30 seconds
        public void Update()
        {
            m_TimerCalculation -= Time.deltaTime;
            if (m_TimerCalculation <= 0)
            {
                m_TimerCalculation = 30;
                m_CurrentFoM = CalculateFoM();
            }
        }
        //every new villager gets a random fom between 29 and 45 assigned
        public float FoMAssignment()
        {
            m_RandomRange = m_Random.Next(0, 20);
            m_OneFoMValue = Mathf.Clamp(29 + m_RandomRange, 29, 46);
            return m_OneFoMValue;
        }

        public int CalculateFoM()
        {
            
            float addedValues = 0f;
            //the FoM value of each villagers is taken
            foreach (float v in m_FoMValues)
            {
                addedValues += v;
            }
            //and the average is calculated
            if (addedValues / m_FoMValues.Count < m_MaxFoM && addedValues / m_FoMValues.Count > m_MinFoM)
            {
                //needed for the GoalManager
                if (addedValues / m_FoMValues.Count >= 45 && PlanetDatalayer.Instance.GetManager<GoalManager>().m_EnoughFoM == false && PlanetDatalayer.Instance.GetManager<GoalManager>().m_CycleNumber == 2)
                {
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_EnoughFoM = true;
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
                    GoalChanged = true;
                }
                else if (addedValues / m_FoMValues.Count >= 65 && PlanetDatalayer.Instance.GetManager<GoalManager>().m_MuchFoM == false && PlanetDatalayer.Instance.GetManager<GoalManager>().m_CycleNumber == 4)
                {
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_MuchFoM = true;
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
                    GoalChanged2 = true;
                }
                return (int)addedValues / m_FoMValues.Count;
            }
            else if (addedValues / m_FoMValues.Count <= m_MinFoM)
                return (int)m_MinFoM;
            else return (int)m_MaxFoM;
        }
        //function called whenever an increase in FoM is necessary
        public void IncreaseFoM(int index, float amount)
        {
            if (m_FoMValues[index] + amount > m_MinFoM && m_FoMValues[index] + amount < m_MaxFoM)
            {
                m_FoMValues[index] += amount;
            }
            else
            {
                if (m_FoMValues[index] + amount < m_MinFoM)
                {
                    m_FoMValues[index] = m_MinFoM;
                }
                if (m_FoMValues[index] + amount > m_MaxFoM)
                {
                    m_FoMValues[index] = m_MaxFoM;
                }
            }
        }
    }
}
