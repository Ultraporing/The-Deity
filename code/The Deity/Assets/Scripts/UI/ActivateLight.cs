using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateLight : MonoBehaviour {

    //Lea Kohl 
    //Activates the player's torch when it is night time
    //GameObject = Staff
    public GameObject m_Torch;
    public Light m_Sun = null;

    private void Awake()
    {
        m_Sun = FindObjectOfType<AutoIntensity>().GetComponent<Light>();
    }

    void Update () {

        //Works in the same way the villagers' torches work
        if (m_Sun.intensity <= 0.6f && !m_Torch.activeInHierarchy)
        {
            m_Torch.SetActive(true);
        }
        else if (m_Torch.activeInHierarchy && m_Sun.intensity > 0.6f)
        {
            m_Torch.SetActive(false);
        }
    }
}
