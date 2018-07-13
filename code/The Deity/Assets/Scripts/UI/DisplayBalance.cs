using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBalance : MonoBehaviour {

    //Lea Kohl
    //needed for informing the player about FoM and PoP 
    public GameObject m_TextObject;
    Text m_Text;
    public Image bar;
    //get the numbers
    public ManagePoP m_ManagePoP;
    float m_FoM;
    float m_PoP;

	void Start () {
        m_FoM = PlanetDatalayer.Instance.GetManager<FoMManager>().m_CurrentFoM;
        m_ManagePoP = GetComponent<ManagePoP>();
        m_PoP = m_ManagePoP.m_PoP;
        m_Text = m_TextObject.GetComponent<Text>();
        m_Text.text = "Faith'o Meter: " + m_FoM + "% \n Power of Prayer: " + m_PoP;
	}
	
	void Update () {

        if (PlanetDatalayer.Instance.GetManager<FoMManager>().m_CurrentFoM != m_FoM || m_ManagePoP.m_PoP != m_PoP)
        {
            m_FoM = PlanetDatalayer.Instance.GetManager<FoMManager>().m_CurrentFoM;
            m_PoP = m_ManagePoP.m_PoP;
            m_Text.text = "Faith'o Meter: " + m_FoM + "% \n Power of Prayer: " + m_PoP;
            FoMBar();
        }
    }
    //Function to make the FoM visible on the FoM bar
    public void FoMBar()
    {
        bar.fillAmount = (m_FoM / PlanetDatalayer.Instance.GetManager<FoMManager>().m_MaxFoM);
    }
}
