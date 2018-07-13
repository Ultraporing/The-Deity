/*
    Co-Written by Tobias Lenz
 */

using Assets.Scripts.Environment.Planet;
using Assets.Scripts.Environment.Topography;
using Assets.Scripts.Helper;
using Assets.Scripts.Helper.Primitive;
using Assets.Scripts.Resources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Contains a full list of all trees and bushes, as well as determines where they can grow
/// </summary>
public class Fertility : MonoBehaviour
{
    public Terrain m_Terrain;
    public GameObject m_TreePrefab;
    public GameObject m_BushPrefab;
    public RainController m_RainController;
    public Texture2D m_ImageBio;
    public LayerMask m_LayerMask;

    public float m_FertilityForest = 0.1f;
    public float m_FertilityGrass = 0.1f;
    public float m_FertilityBush = 0.1f;

    public int m_NumberTrees = 0;
    int m_MaxTrees = 20;
    public int m_NumberBushes = 0;
    int m_MaxBushes = 20;

    float m_Probability = 0.5f;
    public FindableListRect<GameObject> m_TreeList = new FindableListRect<GameObject>();
    public FindableListRect<GameObject> m_BushList = new FindableListRect<GameObject>();

    bool m_ChangeGoals;

    /// <summary>
    /// Creates the Biome map
    /// </summary>
    private void Awake()
    {
        if (!PlanetDatalayer.Instance.GetManager<BiomeManager>().CreateBiomeMap(m_ImageBio))
        {
            Debug.LogError("Failed to create biomemap");
        }
    }

    /// <summary>
    /// Update Goals, grow bushes and trees where applicable when it is raining
    /// </summary>
    void FixedUpdate()
    {
        if (m_NumberTrees >= 5 && PlanetDatalayer.Instance.GetManager<GoalManager>().m_CycleNumber == 2 && !m_ChangeGoals)
        {
            PlanetDatalayer.Instance.GetManager<GoalManager>().m_Trees = true;
            PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
            m_ChangeGoals = true;
        }
        if (m_RainController.IsRaining == true)
        {
            int x = (int)m_RainController.transform.position.x;
            int z = (int)m_RainController.transform.position.z;

            if (!((x >= 0 && x < PlanetDatalayer.Instance.GetManager<BiomeManager>().BiomeMap.GetLength(0)) &&
                (z >= 0 && z < PlanetDatalayer.Instance.GetManager<BiomeManager>().BiomeMap.GetLength(1))))
            {
                return;
            }

            if (PlanetDatalayer.Instance.GetManager<BiomeManager>().BiomeMap[x, z] == Biomes.Forest)
            {
                m_FertilityForest = Mathf.Clamp01(m_FertilityForest + 2*Time.deltaTime);

                if (m_FertilityForest >= 0.7)
                {
                   
                    if (m_NumberTrees >= m_MaxTrees)
                    {
                        return;
                    }

                    Vector3 position = new Vector3(x + Random.Range(-20, 20), m_RainController.transform.position.y, z + Random.Range(-20, 20));
                    if (!PlanetDatalayer.Instance.GetManager<SurfaceMaterialManager>().IsGroundMaterialAt((int)position.x, (int)position.z, GroundMaterial.Gras) ||
                        PlanetDatalayer.Instance.GetManager<BiomeManager>().BiomeMap[x, z] != Biomes.Forest)
                    {
                        return;
                    }
                    else
                    {
                        RaycastHit hitB;
                        if (Physics.Raycast(new Ray(position, Vector3.down), out hitB, 1000f, m_LayerMask))
                        {
                            Vector3 targetPos = m_Terrain.transform.InverseTransformPoint(hitB.point);
                            if (m_TreeList.Find(new Circle(new Vector2(targetPos.x, targetPos.z), 1f)).Count != 0)
                            {
                                return;
                            }

                            m_TreeList.Insert(Instantiate(m_TreePrefab, targetPos, Quaternion.identity, m_Terrain.transform), new Rect(targetPos.x - 0.5f, targetPos.z - 0.5f, 1f, 1f));
                            m_NumberTrees++;

                            
                        }
                    }
                }
            }
            else if (PlanetDatalayer.Instance.GetManager<BiomeManager>().BiomeMap[x, z] == Biomes.BushLand)
            {
                m_FertilityBush = Mathf.Clamp01(m_FertilityBush + 2 * Time.deltaTime);

                if (m_FertilityBush >= 0.4)
                {
                    if (m_NumberBushes >= m_MaxBushes)
                    {
                        return;
                    }

                    Vector3 position = new Vector3(x + Random.Range(-75, 75), m_RainController.transform.position.y, z + Random.Range(-75, 75));
                    if (!PlanetDatalayer.Instance.GetManager<SurfaceMaterialManager>().IsGroundMaterialAt((int)position.x, (int)position.z, GroundMaterial.Gras) ||
                        PlanetDatalayer.Instance.GetManager<BiomeManager>().BiomeMap[x, z] != Biomes.BushLand)
                    {
                        return;
                    }
                    else
                    {
                        RaycastHit hitB;
                        if (Physics.Raycast(new Ray(position, Vector3.down), out hitB, 1000f, m_LayerMask))
                        {
                            Vector3 targetPos = m_Terrain.transform.InverseTransformPoint(hitB.point);
                            if (m_BushList.Find(new Circle(new Vector2(targetPos.x, targetPos.z), 1f)).Count != 0)
                            {
                                return;
                            }
                      
                            m_BushList.Insert(Instantiate(m_BushPrefab, targetPos, Quaternion.identity, m_Terrain.transform), new Rect(targetPos.x - 0.5f, targetPos.z - 0.5f, 1f, 1f));
                            m_NumberBushes++;
                            if (m_NumberBushes == 5)
                            {
                                PlanetDatalayer.Instance.GetManager<GoalManager>().m_Bushes = true;
                                PlanetDatalayer.Instance.GetManager<GoalManager>().m_SomethingChanged = true;
                            }
                        }
                    }
                }
            }
        }
    }
}