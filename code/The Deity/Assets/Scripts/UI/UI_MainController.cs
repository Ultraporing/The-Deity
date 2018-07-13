using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MainController : MonoBehaviour {

    //Lea Kohl
    //this script is the main controller for the radial menu

    //the array of models that are later displayed on the left hand
    public GameObject[] m_Models;
    public GameObject m_Models_Cloud;
    public GameObject m_Models_Stone;
    public GameObject m_Models_Fire;
    public GameObject m_Models_Stats;

    //to gameobject that are later displayed on the spawn position (right hand)
    public GameObject m_RainCloud;
    public GameObject m_ShowStats;

    public int m_pos;
    
    void Start () {

        //default starting point: fire
        m_Models = new GameObject[] { m_Models_Cloud, m_Models_Fire, m_Models_Stone, m_Models_Stats }; //important that cloud is at the beginning and stats at the end
        m_pos = 1;
        m_Models_Cloud.SetActive(false);
        m_Models_Stone.SetActive(false);
        m_Models_Stats.SetActive(false);
        m_Models_Fire.SetActive(true);
        
        m_RainCloud.SetActive(false);
        m_ShowStats.SetActive(false);
    }
	
    //go right and go left are called through the radial menu button functions
	public void GoRight()
    {
        if (m_pos + 1 > m_Models.Length - 1) m_pos = 0;
        else m_pos++;
        SwitchPositions("right");
    }
	
    public void GoLeft()
    {
        if (m_pos - 1 < 0) m_pos = m_Models.Length - 1;
        else m_pos--;
        SwitchPositions("left");
    }

    void SwitchPositions(string direction)
    {
        if (direction.Equals("left"))
        {
            //check, if stats are activated in RightHand
            if (m_pos == m_Models.Length - 1)
            {
                m_Models[0].SetActive(false);
                m_RainCloud.SetActive(false);

                m_ShowStats.SetActive(true);
            }
            //check, if stats deactivated in righthand
            else if (m_pos == m_Models.Length - 2)
            {
                m_ShowStats.SetActive(false);
                m_Models[m_Models.Length - 1].SetActive(false);
            }

            //everything else
            else m_Models[m_pos + 1].SetActive(false);

            //check, if raincloud in righthand
            if (m_pos == 0)
            {
                m_RainCloud.SetActive(true);

            }
            m_Models[m_pos].SetActive(true);
        }

        if (direction.Equals("right"))
        {
            //check if activation of raincloud an deactivation of stats in righthand
            if (m_pos == 0)
            {
                m_Models[m_Models.Length - 1].SetActive(false);
                m_RainCloud.SetActive(true);
                m_ShowStats.SetActive(false);
            }

            //check if activation of stats in righthand
            else if (m_pos == m_Models.Length - 1)
            {
                m_ShowStats.SetActive(true);
                m_Models[m_Models.Length - 2].SetActive(false);
            }
            else
            {
                m_Models[m_pos - 1].SetActive(false);
                //check if deactivation of raincloud in righthand
                if (m_pos - 1 == 0)
                    m_RainCloud.SetActive(false);
                    
            }
            m_Models[m_pos].SetActive(true);
        }
    }
}
