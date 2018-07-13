using Assets.Scripts;
using Assets.Scripts.Constructions;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuilding : MonoBehaviour {

    //Written by Lea Kohl Heretics added by Tobias Lenz
    //Responsible for the whole House Building process

    //Everything Bonfire related 
    protected Bonfire m_ThisBonfire;
    public Inventory m_InventoryRef = null;
    //Variables responsible for the House Building Process
    public float timer;
    bool startTimer;
    public int index;
    public int stages;
    public bool isBuilding;
    //Needed for smooth build and destruction processes
    public House m_House;
    public List<int> m_WaitingQueue;
    public GameObject[] m_BuiltHouses;
    //needed for Herecy event
    public GameObject m_FalseIdol;
    
    void Start () {
        timer = 10f;
        index = -1;
        stages = 0;
        startTimer = false;
        m_ThisBonfire = gameObject.GetComponent<Bonfire>();
        m_InventoryRef = GetComponent<Bonfire>().m_ResourceInventory;
        //Waiting Queue has the indexes of the houses that are waiting to be built
        m_WaitingQueue.Add(0);
        m_WaitingQueue.Add(1);
        m_WaitingQueue.Add(2);
        m_WaitingQueue.Add(3);
        //Built Houses has the houses that have already been built
        m_BuiltHouses = new GameObject [4];
        m_BuiltHouses[0] = null;
        m_BuiltHouses[1] = null;
        m_BuiltHouses[2] = null;
        m_BuiltHouses[3] = null;
        
    }
	
	// Update is called once per frame
	void Update () {

        // Activate/Deactivate False Idol if there are some/no heretics
        if (PlanetDatalayer.Instance.GetManager<VillagerManager>().GetResidingHereticsFromBonfire(m_ThisBonfire).Count > 0 && !m_FalseIdol.activeSelf)
            m_FalseIdol.SetActive(true);
        else if (PlanetDatalayer.Instance.GetManager<VillagerManager>().GetResidingHereticsFromBonfire(m_ThisBonfire).Count == 0 && m_FalseIdol.activeSelf)
            m_FalseIdol.SetActive(false);
        
        if (startTimer)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                startTimer = false;
                timer = 10;
                if (stages == 3)
                {
                    stages = 0;
                }
                else stages++;
            }
        }
        //checks Prerequisites for house building when no house is currently being built
		else if(PlanetDatalayer.Instance.GetManager<FoMManager>().m_CurrentFoM > 44 && (m_InventoryRef.GetTotalAmountOfResource(Assets.Scripts.Resources.ResourceType.Rock) >= 10 &&
            m_InventoryRef.GetTotalAmountOfResource(Assets.Scripts.Resources.ResourceType.Wood) >= 10 && stages <= 5))
        {
            if(stages == 0)//when the else if condition is true for the first time
            {
                if(m_WaitingQueue.Count > 0)
                {
                    index = m_WaitingQueue[0];
                    this.gameObject.transform.GetChild(index).gameObject.SetActive(true);
                    startTimer = true;
                    m_InventoryRef.RemoveResource(Assets.Scripts.Resources.ResourceType.Rock, 10);
                    m_InventoryRef.RemoveResource(Assets.Scripts.Resources.ResourceType.Wood, 10);
                }
            }
            else if (m_WaitingQueue.Count > 0)
            {
                BuildHouse(index, stages);
                isBuilding = true;
            }
        }
        //Check for goal achieving script
        if(PlanetDatalayer.Instance.GetManager<GoalManager>().m_Houses == false && PlanetDatalayer.Instance.GetManager<GoalManager>().m_CycleNumber == 2 && m_BuiltHouses[0] != null)
        {
            PlanetDatalayer.Instance.GetManager<GoalManager>().m_Houses = true;
            PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
        }
	}
    //original building method (mainly focused o ngetting children of the bonfire object and setting the right ones active
    void BuildHouse(int index, int stages)
    {
        if(stages-1 > 0) this.gameObject.transform.GetChild(index).GetChild(stages-1).gameObject.SetActive(false);
        this.gameObject.transform.GetChild(index).GetChild(stages).gameObject.SetActive(true);
        if (gameObject.transform.GetChild(index).GetChild(stages).GetComponent<House>() != null)
        {
            gameObject.transform.GetChild(index).GetChild(stages).GetComponent<House>().Index = index;
            m_BuiltHouses[index] = gameObject.transform.GetChild(index).GetChild(stages).gameObject;
            m_WaitingQueue.Remove(index);
            //Check for goal achieving script
            if (m_WaitingQueue.Count == 0 && PlanetDatalayer.Instance.GetManager<GoalManager>().m_EnoughHouses == false && PlanetDatalayer.Instance.GetManager<GoalManager>().m_CycleNumber == 3)
            {
                PlanetDatalayer.Instance.GetManager<GoalManager>().m_EnoughHouses = true;
                PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
            }
        }
        startTimer = true;
    }

    void UpdateGraph(GameObject go)
    {
        BoxCollider bc = go.GetComponent<BoxCollider>();
        var guo = new GraphUpdateObject(bc.bounds);
        guo.updatePhysics = true;
        AstarPath.active.UpdateGraphs(guo);
    }

    private void SpawnFalseIdol()
    {
        if (!m_FalseIdol.activeSelf)
        {
            m_FalseIdol.SetActive(true);
        }
    }

}
