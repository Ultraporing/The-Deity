using Assets.Scripts;
using Assets.Scripts.Constructions;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using Assets.Scripts.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBoards : MonoBehaviour {
    //Lea Kohl
    //Script for displaying additional information for the player on houses
    public House m_ThisHouse;
    public Bonfire m_Bonfire;
    public Inventory m_InventoryRef = null;

    void Start () {
        m_ThisHouse = gameObject.transform.parent.parent.GetComponent<House>();
        m_Bonfire = gameObject.transform.parent.parent.parent.parent.GetComponent<Bonfire>();
        m_InventoryRef = m_Bonfire.m_ResourceInventory;
	}
	
	void Update () {
        switch (m_ThisHouse.Index)
        {
            case 0:
                GetComponent<Text>().text = "Wood: " + m_InventoryRef.GetTotalAmountOfResource(Assets.Scripts.Resources.ResourceType.Wood);
                break;
            case 1:
                GetComponent<Text>().text = "Time till new\nvillager appears: " + (int) m_ThisHouse.GetComponent<LoveHouse>().m_SpawnDelay;
                break;
            case 2:
                GetComponent<Text>().text = "Stones: " + m_InventoryRef.GetTotalAmountOfResource(Assets.Scripts.Resources.ResourceType.Rock);
                break;
            case 3:
                GetComponent<Text>().text = "Total number\nof villagers: " + PlanetDatalayer.Instance.GetManager<VillagerManager>().NumVillagers;
                break;
        }
	}
}
