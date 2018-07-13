/*
    Co-Written by Tobias Lenz and Lea Kohl
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Raincloud object and checks for controller input
/// </summary>
public class RainController : MonoBehaviour {

    ParticleSystem m_Psys;
    [SerializeField]
    public bool IsRaining { get; private set; }
    public SteamVR_TrackedController m_rightHandController;
    public GameObject m_ModelRain;
    //Tools to extinguish fires
    public GameObject m_ColliderObject;
    public KillFire m_KillFire;
    //Rain Sound effect
    public AudioSource m_Rain;
    //Balancing purposes
    public ManagePoP m_ManagePoP;

    /// <summary>
    /// Initialize variables
    /// </summary>
    void Start () {
        m_Psys = GetComponent<ParticleSystem>();
        m_Psys.Stop(true);
        m_ColliderObject.SetActive(false);
        m_KillFire = m_ColliderObject.GetComponent<KillFire>();
        m_Rain = GetComponent<AudioSource>();
        
	}
	
    /// <summary>
    /// Check for controller input and enable/disable sound/particles and set IsRaining Flag
    /// </summary>
	void Update ()
    {
        if (m_rightHandController != null)
        {
            // get the normalized position of this game object relative to the controller
            if (m_rightHandController.triggerPressed && m_ModelRain.activeInHierarchy && m_ManagePoP.m_PoP - 4 >= 0 && m_KillFire.m_CanRain == true)
            {
                if (m_Psys.isStopped)
                {
                    IsRaining = true;
                    m_Psys.Play(true);
                    m_ColliderObject.SetActive(true);
                    m_Rain.Play();  
                }
            }
            else
            {
                if (m_Psys.isPlaying)
                {
                    IsRaining = false;
                    m_Psys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    m_ColliderObject.SetActive(false);
                    m_Rain.Stop();
                }
            }
        }
        //else condition for testing purposes without VR
        else
        {
            RaycastHit hit;
            Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit);
            transform.parent.transform.position = new Vector3(hit.point.x, hit.point.y + 50, hit.point.z);

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (m_Psys.isStopped)
                {
                    IsRaining = true;
                    m_Psys.Play(true);
                }
            }
            else if (Input.GetKeyUp(KeyCode.R))
            {
                if (m_Psys.isPlaying)
                {
                    IsRaining = false;
                    m_Psys.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
            }
        }
	}
}
