using Assets.Scripts;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : IManager
{
    //Lea Kohl

    public List<String> m_Goals;
    public List<bool> m_CompletionCheck;
    public String[] m_CurrentGoals;
    public bool[] m_BoolOfGoals;

    //different bools to check the goals
    //cycle 1
    public bool m_BurningFire;
    public bool m_StonesSpawned;
    public bool m_Bushes;
    //cycle 2
    public bool m_Trees;
    public bool m_EnoughFoM;
    public bool m_Houses;
    //cycle 3
    public bool m_NewVillager;
    public bool m_EnoughHouses;
    public bool m_KillAVillager;
    //cycle 4
    public bool m_MuchFoM;
    public bool m_MasterOfDesaster;
    public bool m_EnoughVillagers;

    //bool to check for changes (for update goals)
    public bool m_SomethingChanged;
    public bool m_LastBoolChanged;

    public int m_CycleNumber;

	void Start () {
        m_Goals.Add("Completed!");
        m_Goals.Add("");
        m_SomethingChanged = false;
        m_CycleNumber = 1;
    }

	public void Update () {
	}
    //used to switch after all goals in an array  are completed
    public String[] ChangeIntCycles(int cycleNumber)
    {
        m_CycleNumber = cycleNumber;
        switch (cycleNumber)
        {
            case 1:

                m_CurrentGoals = new String[] { "Start the bonfire", "Let stones appear", "Let bushes grow" };
                return m_CurrentGoals;
                
            case 2:
                m_CurrentGoals = new String[] { "Let trees grow", "Achieve a FoM of 45%", "Build a house" };
                return m_CurrentGoals;

            case 3:
                m_CurrentGoals = new String[] { "Let your villagers multiply", "Have four houses", "Kill a villager" };
                return m_CurrentGoals;

            case 4:
                m_CurrentGoals = new String[] { "Have a FoM of 65%", "Stop a Desaster", "Have 10 Villagers" };
                return m_CurrentGoals;
        }
        return null;
    }
    //used by update goals to figure out which goal have already been completed 
    public bool[] ChangeBoolCycles(int cycleNumber)
    {
        switch (cycleNumber)
        {
            case 1:

                m_BoolOfGoals = new bool[] { m_BurningFire, m_StonesSpawned, m_Bushes };
                return m_BoolOfGoals;

            case 2:
                m_BoolOfGoals = new bool[] { m_Trees, m_EnoughFoM, m_Houses};
                return m_BoolOfGoals;

            case 3:
                m_BoolOfGoals = new bool[] { m_NewVillager, m_EnoughHouses, m_KillAVillager };
                return m_BoolOfGoals;

            case 4:
                m_BoolOfGoals = new bool[] { m_MuchFoM, m_MasterOfDesaster, m_EnoughVillagers };
                return m_BoolOfGoals;
        }
        return null;
    }

    public void ChangeBoolToTrue (bool changedStatus)
    {
        changedStatus = true;
        m_SomethingChanged = true;
    }
}
