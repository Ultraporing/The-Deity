using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateProjector : MonoBehaviour {
    //Lea Kohl
    //Activates the Biome Projector whenever the Stat Object is active
    public GameObject m_Stats;
    public GameObject m_Projector;

    private void Start()
    {
        m_Projector.SetActive(false);
    }
   
    void Update () {

        if (m_Stats.activeSelf)
        {
            m_Projector.SetActive(true);
        }
        else
        {
            m_Projector.SetActive(false);
        }
	}
}
