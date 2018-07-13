using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawning : MonoBehaviour
{

    ParticleSystem m_Psys1;
    ParticleSystem m_Psys2;
    [SerializeField]

    public SteamVR_TrackedController m_rightHandController;
    public bool m_isActive;

    // Use this for initialization
    void Start()
    {
        m_Psys1 = GetComponent<ParticleSystem>();
        m_Psys2 = GetComponent<ParticleSystem>();
        m_Psys1.Stop(true);
        m_Psys2.Stop(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (m_rightHandController != null)
        {
            // get the normalized position of this game object relative to the controller
            Vector3 m_pos = m_rightHandController.transform.position;
            Vector3 m_change = m_rightHandController.transform.forward * 20;

            transform.parent.transform.position = m_pos + m_change;

            if (m_rightHandController.triggerPressed && m_isActive)
            {
                if (m_Psys1.isStopped && m_Psys2.isStopped)
                {

                    m_Psys1.Play(true);
                    m_Psys1.Play(true);
                }
            }
            else
            {
                if (m_Psys1.isPlaying && m_Psys2.isPlaying)
                {

                    m_Psys1.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    m_Psys2.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
            }
        }
    }
}