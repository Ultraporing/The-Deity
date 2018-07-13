using Assets.Scripts.Environment.Planet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawning : MonoBehaviour {
    //Lea Kohl

    public GameObject m_Rock;
    public SteamVR_TrackedController m_RightHandController;
    //SpawnPosition can be found on Right Hand (pressed is a helper variable to not keep spamming stones)
    public GameObject m_SpawnPosition;
    bool m_pressed = false;
    //Balancing tools
    public ManagePoP m_ManagePoP;
    public int m_Costs;
    //Number Stones is needed to check if one of the first goals is achieved
    public int m_NumberStones = 0;

    void Update () {

        if (m_RightHandController != null)
        {
            if (m_RightHandController.triggerPressed && !m_pressed && m_ManagePoP.m_PoP - m_Costs >= 0)
            {
                Instantiate(m_Rock, new Vector3(m_SpawnPosition.transform.position.x, m_SpawnPosition.transform.position.y + 5, m_SpawnPosition.transform.position.z), Quaternion.identity);
                m_pressed = true;
                m_ManagePoP.m_PoP -= m_Costs;
                m_NumberStones++;
                //Checks if goal is achieved
                if (m_NumberStones == 5)
                {
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_StonesSpawned = true;
                    PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
                }
            }
            if (!m_RightHandController.triggerPressed)
            {
                m_pressed = false;
            }
        }
        //else condition for testing purposes without VR
        else
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            transform.position = new Vector3(hit.point.x, hit.point.y + 20, hit.point.z);

            if (Input.GetKeyDown(KeyCode.F))
            {
                Instantiate(m_Rock, transform.position, Quaternion.identity);
            }
        }
    }
}
