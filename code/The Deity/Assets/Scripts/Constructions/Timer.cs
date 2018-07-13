using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public float m_TimeLeft;

    public BuildHouse m_Housing;


	// Use this for initialization
	void Start () {

        
	}
	
	// Update is called once per frame
	void Update () {

        m_TimeLeft -= Time.deltaTime;

        if (m_TimeLeft <= 0)
        {
            m_Housing.m_TimerDone = true;
            m_TimeLeft = 10f;
            
        }

	}
}
