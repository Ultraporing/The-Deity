using Assets.Scripts.AI.Creature.Villager;
using Assets.Scripts.Constructions;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseDestruction : HouseBuilding {
    //Lea Kohl
    public GameObject m_Fires;
    public AudioSource m_SoundFire;
    protected Burnable m_Burnable;
    //Bool for the Destruction Timer script
    public bool m_StartDestruction;

    private void Start()
    {
        m_Fires = this.gameObject.transform.GetChild(0).gameObject;
        m_SoundFire = GetComponent<AudioSource>();
        m_Burnable = GetComponent<Burnable>();
        m_Fires.SetActive(false);
    }

    private void Update()
    {
        if (m_Burnable.isOnFire)
        {
            m_StartDestruction = true;
            m_SoundFire.Play();
            m_Fires.SetActive(true);

        }
        else
        {
            m_StartDestruction = false;
            m_SoundFire.Stop();
            m_Fires.SetActive(false);
        }
    }
    //Function is called on by the Destruction Timer script
    public void DestroyHouse(int index)
    {
        gameObject.transform.parent.parent.GetComponent<HouseBuilding>().m_WaitingQueue.Add(index);
        gameObject.transform.parent.parent.GetComponent<HouseBuilding>().m_BuiltHouses[index] = null;
        this.gameObject.SetActive(false);
    }
}
