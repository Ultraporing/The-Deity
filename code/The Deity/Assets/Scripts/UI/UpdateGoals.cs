using Assets.Scripts.AI.Creature.Villager;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateGoals : MonoBehaviour {

    //Lea Kohl
    /*This script updates the goals at the stats object on the bonfire by receiving information from the GoalManager script
     * The player can complete three goals at a time
     * only after completing all three of these
     * three new ones will appear
     * the goals are a way of guiding the player, however completing them does not give him/her 
     * any special advantages besides a small FoM increase*/

    public GameObject m_TextObject;
    Text m_Text;
    //Object for Sound Feedback
    public GameObject m_AudioObject;
    //Arrays needed to communicate with the goal manager script
    public String[] m_CurrentGoals;
    public bool[] m_BoolCycles;

    int m_Position1 = 0;
    int m_Position2 = 1;
    int m_Position3 = 2;
    int m_CycleNumber = 1;
    //Game Object that leads the player to the "Game over" screen
    public GameObject m_GameOver;

    void Start()
    {
        ChangeCycles(m_CycleNumber);
        m_Text = m_TextObject.GetComponent<Text>();
        m_Text.text = m_CurrentGoals[m_Position1] + "\n" + m_CurrentGoals[m_Position2] + "\n" + m_CurrentGoals[m_Position3];
        m_AudioObject = gameObject.transform.GetChild(0).gameObject;
        m_AudioObject.SetActive(false);
        m_GameOver.SetActive(false);
    }

    private void Update()
    {
        //checks if a goal has been completed
        if (PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged == true)
        {
            ChangeGoals();
            m_AudioObject.SetActive(true);
        }
        //checks if the game is over (all goals are achieved or all villagers are dead)
        if (PlanetDatalayer.Instance.GetManager<GoalManager>().m_CycleNumber == 4 && PlanetDatalayer.Instance.GetManager<GoalManager>().m_MuchFoM == true && PlanetDatalayer.Instance.GetManager<GoalManager>().m_MasterOfDesaster == true
            && PlanetDatalayer.Instance.GetManager<GoalManager>().m_EnoughVillagers == true)
        {
            m_GameOver.SetActive(true);
        }
        else if(PlanetDatalayer.Instance.GetManager<VillagerManager>().NumVillagers == 0)
        {
            m_GameOver.SetActive(true);
        }
    }
    //called if one goal from the current cycle is completed
    void ChangeGoals()
    {
        PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = false;
        int index = -1;
        int counter = 0;
        m_BoolCycles = PlanetDatalayer.Instance.GetManager<GoalManager>().ChangeBoolCycles(m_CycleNumber);
        
        foreach (bool b in m_BoolCycles)
        {
            index++;
            if (b == true)
            {
                counter++;
                m_CurrentGoals[index] = "Completed!";
                m_Text.text = m_CurrentGoals[m_Position1] + "\n" + m_CurrentGoals[m_Position2] + "\n" + m_CurrentGoals[m_Position3];
                if (counter == 3)
                {
                    m_CycleNumber++;
                    ChangeCycles(m_CycleNumber);
                    m_Text.text = m_CurrentGoals[m_Position1] + "\n" + m_CurrentGoals[m_Position2] + "\n" + m_CurrentGoals[m_Position3];
                    foreach (VillagerAI v in PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList)
                    {
                        PlanetDatalayer.Instance.GetManager<FoMManager>().IncreaseFoM(PlanetDatalayer.Instance.GetManager<VillagerManager>().m_VillagerList.IndexOf(v), 4);
                    }
                }
            }
        }
    }
    //called if all 3 goals of the current cycle are completed to change to the next 3 goals
    void ChangeCycles(int cycleNumber)
    {
        m_CurrentGoals = PlanetDatalayer.Instance.GetManager<GoalManager>().ChangeIntCycles(m_CycleNumber);
        m_BoolCycles = PlanetDatalayer.Instance.GetManager<GoalManager>().ChangeBoolCycles(m_CycleNumber);
    }
}
