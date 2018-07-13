using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Resources;
using Assets.Scripts.Events;

public class UpdatePrayer : MonoBehaviour {
    //Lea Kohl
    /* used to display the people's prayers over the bonfire
     * to give the player small hints in what to do next
     * and to warn them in case a tragedy is happening*/

    //objects needed to display the prayers
    public GameObject m_TextObject;
    Text m_Text;
    //List to store prayers
    List<String> m_Prayers = new List<String>();
    //to figure out if a tragic event has happened
    bool m_EventHappened;
    
	
	void Start () {
        m_Prayers.Add("Oh Great Deity, \n Hear us, Your People!");
        m_Prayers.Add("Oh Great Deity,\nBe mercyful! Do not let\n us starve! You can see the\n fertile land with your cloud!");
        m_Prayers.Add("Oh Great Deity,\nGive us stones that\nwe may build\nshelters from the cold!");
        m_Prayers.Add("Oh Great Deity,\nThere used to be a forest\nin the West near the mountains \nLet it grow again!");
        m_Prayers.Add("Oh Great Deity,\nGive us a sign of\nYour great power!");
        m_Text = m_TextObject.GetComponent<Text>();
        //m_Prayers[0] is default setting
        m_Text.text = m_Prayers[0];
	}
	
	void Update () {
        //checks if an event has happened
        if (PlanetDatalayer.Instance.GetManager<WorldEventManager>().m_CurrentEvent != null)
        {
            if (PlanetDatalayer.Instance.GetManager<WorldEventManager>().m_CurrentEvent.GetType().Equals(typeof(Famine)))
            {
                m_Text.text = "Our crops have vanished!\nMake the ground fertile again!";
            }
            else if(PlanetDatalayer.Instance.GetManager<WorldEventManager>().m_CurrentEvent.GetType().Equals(typeof(Herecy)))
            {
                m_Text.text = "Some people have turned\nagainst you!\nPunish them!";
            }
            else if (PlanetDatalayer.Instance.GetManager<WorldEventManager>().m_CurrentEvent.GetType().Equals(typeof(ForestFire)))
            {
                m_Text.text = "The forest is burning!\n Stop it!";
            }
            //used to play the alarm sound and to remove some pop for additional difficulty
            if(m_EventHappened == false)
            {
                m_EventHappened = true;
                GetComponent<AudioSource>().Play();
                if(GetComponent<ManagePoP>().m_PoP >= 20)
                {
                    GetComponent<ManagePoP>().m_PoP -= 20;
                }
            }
        }
        else
        {
            //the people have certain priorities, the prayers are according to them and to the order of the goals
            if (PlanetDatalayer.Instance.GetManager<ResourceManager>().GetListForResource(ResourceType.Food).m_ResourceSourceList.Count <= 5)
            {
                m_Text.text = m_Prayers[1];
            }
            else if (PlanetDatalayer.Instance.GetManager<ResourceManager>().GetListForResource(ResourceType.Rock).m_ResourceSourceList.Count < 5)
            {
                m_Text.text = m_Prayers[2];
            }
            else if (PlanetDatalayer.Instance.GetManager<ResourceManager>().GetListForResource(ResourceType.Wood).m_ResourceSourceList.Count <= 5)
            {
                m_Text.text = m_Prayers[3];
            }
            else if (PlanetDatalayer.Instance.GetManager<FoMManager>().m_CurrentFoM < 45)
            {
                m_Text.text = m_Prayers[4];
            }
            else m_Text.text = m_Prayers[0];
            //makes sure that the player is warned when a second or third event is happening 
            if(m_EventHappened == true)
            {
                m_EventHappened = false;
                if(PlanetDatalayer.Instance.GetManager<GoalManager>().m_MasterOfDesaster == false && PlanetDatalayer.Instance.GetManager<GoalManager>().m_CycleNumber == 4)
                {
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_MasterOfDesaster = true;
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
                }
            }
        }
    }
}
