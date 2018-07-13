using Assets.Scripts.Constructions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatShow : MonoBehaviour {

    //Lea Kohl 
    //Used to interact between stat show and stat display
    public StatDisplay m_bonfire;
    public AudioSource m_Plop;

    //object that has this script is trigger
    private void OnTriggerEnter(Collider collision)
    {
        m_bonfire = collision.GetComponent<StatDisplay>();
        if (m_bonfire != null)
        {
            m_Plop.Play();
            if (m_bonfire.m_showStats == true)
            {
                m_bonfire.m_showStats = false;  
            }

            else if(m_bonfire.m_showStats == false)
            {
                m_bonfire.m_showStats = true;               
            }
        }
    }
}
