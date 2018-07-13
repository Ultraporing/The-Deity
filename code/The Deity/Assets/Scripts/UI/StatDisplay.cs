using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDisplay : MonoBehaviour {

    //Lea Kohl
    //put this script on any object that should be interactable with the StatShow Script
    public bool m_showStats;
    //m_StatDisplay should be a child of the object that has this script attached to it
    public GameObject m_StatDisplay;
	
	void Start () {
        m_showStats = true;
        m_StatDisplay.SetActive(true);
	}
	
	void Update () {

        if (m_showStats == true)
        {
            m_StatDisplay.SetActive(true);
        }
        else m_StatDisplay.SetActive(false);
	}
}
