using Assets.Scripts.Constructions;
using Assets.Scripts.Creatures.Villager;
using Assets.Scripts.Environment.Planet;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionTimer : HouseDestruction {
    //Lea Kohl
    public float m_BurnTime;
    public int number;

    void Awake()
    {
        m_BurnTime = 30;
    }
    
    void Update()
    {
        if (GetComponent<HouseDestruction>().m_StartDestruction)
        {
            m_BurnTime -= Time.deltaTime;
        }
        if(m_BurnTime <= 0)
        {
            m_BurnTime = 30;
            m_Burnable = GetComponent<Burnable>();
            m_Burnable.isOnFire = false;
            //Removes the Residents from the destroyed house
            GetComponent<House>().Residents.ForEach((x) =>
            {
                int id = PlanetDatalayer.Instance.GetManager<VillagerManager>().GetVillagerIndex(x);
                PlanetDatalayer.Instance.GetManager<FoMManager>().IncreaseFoM(id, -10f);
            });
            GetComponent<HouseDestruction>().m_StartDestruction = false;
            GetComponent<HouseDestruction>().m_Fires.SetActive(false);
            GetComponent<HouseDestruction>().m_SoundFire.Stop();
            DestroyHouse(GetComponent<House>().Index);
            
        }
    }
}
