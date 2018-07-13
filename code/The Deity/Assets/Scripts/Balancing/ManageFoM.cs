using Assets.Scripts.AI.Creature.Villager;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageFoM : MonoBehaviour {

    //Lea Kohl
    //add: Fom drop when bonfire goes out, change register villager, villager prozentual berechnen

    public float m_FoM;
    int m_MaxFoM;
    
    public List<float> m_SingleFoMValues = new List<float>();
    public List<bool> m_ChangeFoMforHouses = new List<bool>();
    public Fertility m_ResourceManager;
    public RockSpawning m_Stones;
    float m_RandomRange;
    float m_OneFoMValue;

    public int count;

    int m_KillCounter; //Counts how many people the deity has killed in a short time

    public float m_TimerCalculation;
    public float m_TimerStatCheck;
    public float m_TimerResourceCheck;
    public float m_TimerKillCheck;
    System.Random m_Random = new System.Random(DateTime.Now.Millisecond);
    public int m_MaxFoMSpawning ;
    public int m_MinFoMSpawning ;
    

    int m_NumberVillagers;
    float percentage = 0.33f;



    void Start () {
     
        //every 30 seconds the current fom is calculated
        m_TimerCalculation = 30;
        m_TimerStatCheck = 10;
        m_TimerResourceCheck = 60;
        m_TimerKillCheck = 20;
        //assigns each Villager a Random FoM between 29 and 46
        foreach (VillagerAI a in PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList)
        {
            m_RandomRange = m_Random.Next(0, 20);
            m_OneFoMValue = Mathf.Clamp(m_MinFoMSpawning + m_RandomRange, m_MinFoMSpawning, m_MaxFoMSpawning);
            m_SingleFoMValues.Add(m_OneFoMValue);
            m_ChangeFoMforHouses.Add(true);
        }
        m_FoM = CalculateFoM(m_SingleFoMValues);
        m_MaxFoM = 100;

        m_KillCounter = 0;

        m_NumberVillagers = PlanetDatalayer.Instance.GetManager<VillagerManager>().NumVillagers;
    }


    void Update()
    {
        //Checkt Zeitintervalle
        m_TimerCalculation -= Time.deltaTime;
        m_TimerStatCheck -= Time.deltaTime;
        m_TimerResourceCheck -= Time.deltaTime;
        m_TimerKillCheck -= Time.deltaTime;

        if (m_TimerCalculation <= 0)
        {
            m_TimerCalculation = 30;
            m_FoM = CalculateFoM(m_SingleFoMValues);
        }
        if (m_TimerStatCheck <= 0)
        {
            CheckStats();
            m_TimerStatCheck = 10;
        }
        if (m_TimerResourceCheck <= 0)
        {
            if (m_ResourceManager.m_NumberTrees > 5) ChangeTrees();
            if (m_Stones.m_NumberStones > 5) ChangeStones();
            if (m_ResourceManager.m_NumberBushes > 10) ChangeBushes();
            m_TimerResourceCheck = 60;
        }
        if (m_TimerKillCheck <= 0)
        {
            KillerCheck();
            m_KillCounter = Mathf.Clamp(m_KillCounter - 1, 0, int.MaxValue);
            m_TimerKillCheck = 0;
        }
        //checks if new villagers have appeared
        if (m_NumberVillagers != PlanetDatalayer.Instance.GetManager<VillagerManager>().NumVillagers)
        {
            if (m_NumberVillagers < PlanetDatalayer.Instance.GetManager<VillagerManager>().NumVillagers)
                VillagerKilled();
            else VillagerSpawned();
        }

        


    }
    

    //Calculates the total FoM
    int CalculateFoM(List<float> values)
    {
        float addedValues = 0f;
        foreach (float v in values)
        {
            addedValues += v;
        }
        if ((int)addedValues / values.Count < m_MaxFoM)
            return (int)addedValues / values.Count;
        else return m_MaxFoM;
    }

    void CheckStats()
    {
        int count1 = 0;
        foreach (VillagerAI a in PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList)
        {
            if (a.CreatureStats.NeedsFood() || a.CreatureStats.NeedsDrink())
            {
                m_OneFoMValue = m_SingleFoMValues[count1];
                m_OneFoMValue -= 5;
                m_SingleFoMValues[count1] = m_OneFoMValue;
            }
            if (a.CreatureStats.IsFoodFull() || a.CreatureStats.IsDrinkFull())
            {
                m_OneFoMValue = m_SingleFoMValues[count1];
                m_OneFoMValue += 0.1f;
                m_SingleFoMValues[count1] = m_OneFoMValue;
            }

            if (a.m_Home != null && m_ChangeFoMforHouses[count1] == true)
            {
                m_OneFoMValue = m_SingleFoMValues[count1];
                m_OneFoMValue += 10;
                m_SingleFoMValues[count1] = m_OneFoMValue;
                m_ChangeFoMforHouses[count1] = false;
            }
            count1++;
        }
    }

    void VillagerSpawned()
    {
        m_NumberVillagers = PlanetDatalayer.Instance.GetManager<VillagerManager>().NumVillagers;
        m_RandomRange = m_Random.Next(0, 20);
        m_OneFoMValue = Mathf.Clamp(m_MinFoMSpawning + m_RandomRange, m_MinFoMSpawning, m_MaxFoMSpawning);
        m_SingleFoMValues.Add(m_OneFoMValue);
        m_ChangeFoMforHouses.Add(true);
    }

    void VillagerKilled()
    {
        m_NumberVillagers = PlanetDatalayer.Instance.GetManager<VillagerManager>().NumVillagers;
        m_SingleFoMValues.RemoveAt(m_SingleFoMValues.Count - 1);
        m_ChangeFoMforHouses.RemoveAt(m_ChangeFoMforHouses.Count - 1);
        m_KillCounter++;
    }

    void ChangeTrees()
    {
        int counter = 0;
        foreach (VillagerAI a in PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList)
        {

            switch (a.m_VillagerSpecialization)
            {
                case VillagerSpecialization.WoodCutter:
                    m_OneFoMValue = m_SingleFoMValues[counter];
                    m_OneFoMValue += 0.7f;
                    m_SingleFoMValues[counter] = m_OneFoMValue;
                    break;
                case VillagerSpecialization.StoneMason:
                    m_OneFoMValue = m_SingleFoMValues[counter];
                    m_OneFoMValue += 0.3f;
                    m_SingleFoMValues[counter] = m_OneFoMValue;
                    break;

            }

            counter++;
        }
        counter = 0;
    }

    void ChangeStones()
    {
        int counter = 0;
        foreach (VillagerAI a in PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList)
        {
            switch (a.m_VillagerSpecialization)
            {
                case VillagerSpecialization.StoneMason:
                    m_OneFoMValue = m_SingleFoMValues[counter];
                    m_OneFoMValue += 0.7f;
                    m_SingleFoMValues[counter] = m_OneFoMValue;
                    break;
                case VillagerSpecialization.WoodCutter:
                    m_OneFoMValue = m_SingleFoMValues[counter];
                    m_OneFoMValue += 0.3f;
                    m_SingleFoMValues[counter] = m_OneFoMValue;
                    break;
            }
            counter++;
        }
        
    }

    void ChangeBushes()
    {
        int counter = 0;
        foreach (VillagerAI a in PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList)
        {
            m_OneFoMValue = m_SingleFoMValues[counter];
            m_OneFoMValue += 1;
            m_SingleFoMValues[counter] = m_OneFoMValue;
            counter++;
        }
    }

    void KillerCheck()
    {
        if(m_KillCounter >= PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList.Count * percentage)
        {
            int counter = 0;
            foreach (VillagerAI a in PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList)
            {
                m_OneFoMValue = m_SingleFoMValues[counter];
                m_OneFoMValue -= 10;
                m_SingleFoMValues[counter] = m_OneFoMValue;
                counter++;
            }
        }
        else if(m_KillCounter > 0)
        {
            int counter = 0;
            foreach (VillagerAI a in PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList)
            {
                m_OneFoMValue = m_SingleFoMValues[counter];
                m_OneFoMValue += 5;
                m_SingleFoMValues[counter] = m_OneFoMValue;
                counter++;
            }
        }
    }
}
