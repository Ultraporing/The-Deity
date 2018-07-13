using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablingUI : MonoBehaviour
{
    //Lea Kohl
    public SteamVR_TrackedController m_LeftController;
    public GameObject m_ControllerModel;
    //Object that shows the Position where the Resources will be Spawned
    public GameObject m_SpawnPosition;
    public bool isActive;
    bool m_isPressed = false;
    
    void Start()
    {
        m_ControllerModel.SetActive(true);
        m_SpawnPosition.SetActive(true);
        isActive = true;
    }

    void Update()
    {
        //By pressing left trigger the UI and all its options are enabled/disabled
        if (m_LeftController.triggerPressed && !m_isPressed)
        {
            if (isActive == false)
            {
                m_ControllerModel.SetActive(true);
                m_SpawnPosition.SetActive(true);
                isActive = true;
            }
            else
            {
                m_ControllerModel.SetActive(false);
                m_SpawnPosition.SetActive(false);
                isActive = false;
            }
            m_isPressed = true;
        }
        if (!m_LeftController.triggerPressed)
        {
            m_isPressed = false;
        }
    }
}
