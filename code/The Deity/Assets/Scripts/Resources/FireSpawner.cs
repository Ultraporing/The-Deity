using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawner : MonoBehaviour {

    //Lea Kohl
    public GameObject m_FirePrefab;
    public SteamVR_TrackedController m_RightHandController;
    //SpawnPosition can be found on Right Hand (pressed is a helper variable to not keep spamming fire balls)
    public GameObject m_SpawnPosition;
    bool m_pressed = false;
    //Balancing tools
    public ManagePoP m_ManagePoP;
    public int m_Costs;
    public AudioClip m_FireBall;

	void Start () {
        m_Costs = 2;
	}
	
	void Update () {

        if (m_RightHandController != null)
        {
            if (m_RightHandController.triggerPressed && !m_pressed && m_ManagePoP.m_PoP - m_Costs >= 0)
            {
                Instantiate(m_FirePrefab,new Vector3(m_SpawnPosition.transform.position.x, m_SpawnPosition.transform.position.y + 5, m_SpawnPosition.transform.position.z), Quaternion.identity);
                AudioSource.PlayClipAtPoint(m_FireBall, m_SpawnPosition.transform.position, 1f);
                m_pressed = true;
                m_ManagePoP.m_PoP -= m_Costs;
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
                Instantiate(m_FirePrefab, transform.position, Quaternion.identity);
            }
        }
    }
}
