using Assets.Scripts;
using Assets.Scripts.Constructions;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildHouse : MonoBehaviour {

    //Lea Kohl, Tobias Lenz

    
    
    //"Wieviele Resourcen werden Pro Haus benötigt" 
    public int m_NumberStonesNeeded = 10;
    public int m_NumberWoodNeeded = 10;

    //Array mit Konstruktionen und fertig gestellten Häusern
    public GameObject[] m_HouseStages;
    public GameObject m_FirstStage;
    public GameObject m_SecondStage;
    public GameObject m_ThirdStage;
    public GameObject m_FinalStage;
    public GameObject m_FinalStage_Res;
    public GameObject m_FinalStage_Chur;
    public GameObject m_FinalStage_Love;

    //Arrays mit Spawnpositionen und Rotationen
    public Vector3[] m_SpawnPositions;
    public Quaternion[] m_Rotation;
    public GameObject m_FalseIdol;
    
    //Timer für den Cooldown und Variable zur Kommunikation mit Timer Skript
    public GameObject m_Timer;
    public bool m_TimerDone;
    public bool isConstructing;

    //Hilfsobjekt zum zerstören und aufstellen von Häusern
    public GameObject m_CurrentStage;

    //Hilfszähler zur Konstruktion
    public int counterStages;
    public int counterHouses;
    public int maxHouses;
    int newBuild;

    public Inventory m_InventoryRef = null;

    public ManageFoM m_FoM;

    public GameObject[] m_BuiltHouses;
    public bool m_HouseDestroyed;

    protected Bonfire m_ThisBonfire;
    
    void Start () {

        //Initialisiere Arrays mit Objekten und Positionen
        m_HouseStages = new GameObject[] { m_FirstStage, m_SecondStage, m_ThirdStage, m_FinalStage, m_FinalStage_Res, m_FinalStage_Chur, m_FinalStage_Love};
        m_SpawnPositions = new Vector3[] { new Vector3(248, 121, 106), new Vector3(241, 121, 100), new Vector3(247, 121, 94), new Vector3(252, 121, 100)};
        m_Rotation = new Quaternion[] { Quaternion.Euler(0, 90, 0), Quaternion.identity, Quaternion.Euler(0, -90, 0), Quaternion.Euler(0, -180, 0) };

        // Hol eine referenz zum Inventar des Bonfeuers
        m_InventoryRef = GetComponent<Bonfire>().m_ResourceInventory;

        m_Timer.SetActive(false);
        m_TimerDone = false;
        isConstructing = false;
        counterStages = 0;
        counterHouses = 0;
        maxHouses = 4;

        m_BuiltHouses = new GameObject[m_SpawnPositions.Length];
        m_ThisBonfire = gameObject.GetComponent<Bonfire>();

    }
	
	void Update () {
        // Activate/Deactivate False Idol if there are some/no heretics
        if (PlanetDatalayer.Instance.GetManager<VillagerManager>().GetResidingHereticsFromBonfire(m_ThisBonfire).Count > 0 && !m_FalseIdol.activeSelf)
            m_FalseIdol.SetActive(true);
        else if (PlanetDatalayer.Instance.GetManager<VillagerManager>().GetResidingHereticsFromBonfire(m_ThisBonfire).Count == 0 && m_FalseIdol.activeSelf)
            m_FalseIdol.SetActive(false);

        //Timer Check
        if (m_TimerDone == true)
        {
            m_Timer.SetActive(false);
            counterStages++;
        }

        //Hausbau Check
        if (PlanetDatalayer.Instance.GetManager<FoMManager>().m_CurrentFoM > 20 && (m_InventoryRef.GetTotalAmountOfResource(Assets.Scripts.Resources.ResourceType.Rock) >= m_NumberStonesNeeded && 
            m_InventoryRef.GetTotalAmountOfResource(Assets.Scripts.Resources.ResourceType.Wood) >= m_NumberWoodNeeded && 
            counterHouses < maxHouses && isConstructing == false) || (counterStages > 0 && m_TimerDone == true && counterHouses < maxHouses))
        {
            m_TimerDone = false;
            isConstructing = true;

            Build(counterStages, counterHouses);

            
            

            //hier setzt sich der Kreislauf fort bzw. bricht ab
            if (counterStages < 3)
            {
                m_Timer.SetActive(true);
            }
         }

        else if(PlanetDatalayer.Instance.GetManager<FoMManager>().m_CurrentFoM > 20 && (m_InventoryRef.GetTotalAmountOfResource(Assets.Scripts.Resources.ResourceType.Rock) >= m_NumberStonesNeeded &&
            m_InventoryRef.GetTotalAmountOfResource(Assets.Scripts.Resources.ResourceType.Wood) >= m_NumberWoodNeeded &&
            counterHouses == maxHouses && m_HouseDestroyed == true && isConstructing == false) || (counterStages > 0 && m_TimerDone == true && counterHouses == maxHouses && m_HouseDestroyed == true))
        {
            m_TimerDone = false;
            isConstructing = true;

            Build(counterStages, newBuild);




            //hier setzt sich der Kreislauf fort bzw. bricht ab
            if (counterStages < 3)
            {
                m_Timer.SetActive(true);
            }
        }

        if (counterStages == 3)
        {
            counterStages = 0;
            isConstructing = false;

            if (m_HouseDestroyed) m_HouseDestroyed = false;
        }

       
        
    }

    //eigentiliche Bau Methode
    void Build(int stages, int positions)
    {
        switch (stages)
        {
            case 0:
                m_CurrentStage = Instantiate(m_HouseStages[stages], m_SpawnPositions[positions], m_Rotation[positions]);
                UpdateGraph(m_CurrentStage);
                m_InventoryRef.RemoveResource(Assets.Scripts.Resources.ResourceType.Rock, m_NumberStonesNeeded);
                m_InventoryRef.RemoveResource(Assets.Scripts.Resources.ResourceType.Wood, m_NumberWoodNeeded);
                break;
            case 1:
                Destroy(m_CurrentStage);
                m_CurrentStage = Instantiate(m_HouseStages[stages], m_SpawnPositions[positions], m_Rotation[positions]);
                break;
            case 2:
                Destroy(m_CurrentStage);
                m_CurrentStage = Instantiate(m_HouseStages[stages], m_SpawnPositions[positions], m_Rotation[positions]);
                break;
            case 3:
                Destroy(m_CurrentStage);
                //hier entscheidet sich was für ein Haus es wird
                if (positions == 0)
                {
                    m_CurrentStage = Instantiate(m_FinalStage, m_SpawnPositions[positions], m_Rotation[positions]);
                    m_BuiltHouses[positions] = m_CurrentStage;
                }
                else if (positions == 1)
                {
                    m_CurrentStage = Instantiate(m_FinalStage_Chur, m_SpawnPositions[positions], m_Rotation[positions]);
                    m_BuiltHouses[positions] = m_CurrentStage;
                }
                else if (positions == 2)
                {
                    m_CurrentStage = Instantiate(m_FinalStage_Res, m_SpawnPositions[positions], m_Rotation[positions]);
                    m_BuiltHouses[positions] = m_CurrentStage;
                }
                else if (positions == 3)
                {
                    m_CurrentStage = Instantiate(m_FinalStage_Love, m_SpawnPositions[positions], m_Rotation[positions]);
                    m_BuiltHouses[positions] = m_CurrentStage;

                }
                    if (counterHouses < maxHouses) counterHouses++;
                break;
        }
    }

    public void HouseDestroyed(int index)
    {
        m_BuiltHouses[index] = null;
        newBuild = index; 
        m_HouseDestroyed = true;
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

