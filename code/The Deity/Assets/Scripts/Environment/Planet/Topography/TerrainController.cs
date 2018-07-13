/*
    Written by Tobias Lenz
 */

using Assets.Scripts.Environment.Planet;
using Assets.Scripts.Environment.Planet.Water;
using Assets.Scripts.Environment.Topography;
using Assets.Scripts.Resources;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Environment.Topography
{
    /// <summary>
    /// Contains all Terrain related controlling logic, terrain modification and water
    /// </summary>
    public class TerrainController : MonoBehaviour
    {
        public int m_ModificationDiameter = 50; // the diameter of terrain portion that will raise under the game object 
        public float m_ModificationSpeed = 10f;
        public float m_MinHeight = 85f; // Set the min height of the terrain
        public float m_MaxHeight = 145f; // Set the max height of the terrain
        public bool m_ModifyTerrainActive = false;
        public SteamVR_TrackedController m_RightController;
        

        private Water m_TerrainWater;
        private float[,] m_OldTerrainData;
        private TerrainBrush m_Brush;
        private Terrain m_Terrain;
        private AssignSplatMap m_AssignSplatMap;
        private int m_HmWidth;
        private int m_HmHeight;

        /// <summary>
        /// Initialize the variables
        /// </summary>
        private void Awake()
        {
            m_Terrain = Terrain.activeTerrain;
            m_HmWidth = m_Terrain.terrainData.heightmapWidth;
            m_HmHeight = m_Terrain.terrainData.heightmapHeight;
            m_AssignSplatMap = GetComponent<AssignSplatMap>();
            m_OldTerrainData = m_Terrain.terrainData.GetHeights(0, 0, m_HmWidth, m_HmHeight);
            m_Brush = new TerrainBrush(m_MinHeight, m_MaxHeight);
            m_TerrainWater = transform.parent.Find("Terrain").Find("Water").GetComponent<Water>();
        }

        /// <summary>
        /// Reset Terrain to its defaults when closing the game
        /// </summary>
        private void OnApplicationQuit()
        {
            m_Terrain.terrainData.SetHeights(0, 0, m_OldTerrainData);
            m_AssignSplatMap.AssignSplatMapToTerrain(0, 0, m_HmHeight - 1, m_HmWidth - 1);
        }

        /// <summary>
        /// Check for controller inputs
        /// </summary>
        void Update()
        {
            if (!m_ModifyTerrainActive)
                return;
            // Those are temporary controls to test

            m_Brush.m_Shape = TerrainModificationShape.Square;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                m_Brush.m_Shape = TerrainModificationShape.Circle;
            }

            if (m_RightController != null)
            {
                if (m_RightController.triggerPressed)
                {
                    m_Brush.m_ModificationType = TerrainModificationType.Raise;
                    ModifyTerrain();
                }
                else if (m_RightController.triggerPressed)
                {
                    m_Brush.m_ModificationType = TerrainModificationType.Lower;
                    ModifyTerrain();
                }
                else if (Input.GetMouseButton(2))
                {
                    m_Brush.m_ModificationType = TerrainModificationType.Flatten;
                    ModifyTerrain();
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    if (Input.GetMouseButton(0))
                    {
                        m_Brush.m_ModificationType = TerrainModificationType.Smooth;
                        ModifyTerrain();
                    }
                }
                else
                {
                    if (Input.GetMouseButton(0))
                    {
                        m_Brush.m_ModificationType = TerrainModificationType.Raise;
                        ModifyTerrain();
                    }
                    else if (Input.GetMouseButton(1))
                    {
                        m_Brush.m_ModificationType = TerrainModificationType.Lower;
                        ModifyTerrain();
                    }
                    else if (Input.GetMouseButton(2))
                    {
                        m_Brush.m_ModificationType = TerrainModificationType.Flatten;
                        ModifyTerrain();
                    }
                }             
            }
        }

        /// <summary>
        /// Modify the terrain based on brush settings and updated pathfinding
        /// </summary>
        void ModifyTerrain()
        {
            // get the normalized position of this game object relative to the terrain
            Vector3 hit;

            if (m_RightController != null)
            {
                hit = m_RightController.transform.position;
            }
            else
            {
                RaycastHit hitray;
                Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitray);

                if (hitray.collider == null)
                {
                    return;
                }

                hit = hitray.point;
            }

            // set brush width and height
            m_Brush.m_BrushWidth = m_ModificationDiameter;
            m_Brush.m_BrushHeight = m_ModificationDiameter;

            int xBase = CalculateXbase(hit, m_Brush);
            int yBase = CalculateYbase(hit, m_Brush);

            // get the heights of the terrain under this game object
            float[,] heights = m_Terrain.terrainData.GetHeights(xBase, yBase, m_Brush.m_BrushWidth, m_Brush.m_BrushHeight);

            // Use the brush to modify the terrain
            m_Brush.UseBrush(heights, m_Terrain.terrainData);
            
            // set the new height
            m_Terrain.terrainData.SetHeights(xBase, yBase, heights);

            // create and apply new splat map
            m_AssignSplatMap.AssignSplatMapToTerrain(xBase, yBase, m_Brush.m_BrushHeight, m_Brush.m_BrushWidth);

            PlanetDatalayer.Instance.GetManager<WaterManager>().UpdateWatermapForRegion(new Rect(xBase, yBase, m_Brush.m_BrushWidth, m_Brush.m_BrushHeight), m_Terrain, m_TerrainWater);
            var guo = new GraphUpdateObject(new Bounds(hit, new Vector3(m_Brush.m_BrushWidth, 0, m_Brush.m_BrushHeight)));
            guo.updatePhysics = true;
            AstarPath.active.UpdateGraphs(guo);
        }

        /// <summary>
        /// Calculates the smallest X position based on the raycast hit and brushsettings
        /// </summary>
        /// <param name="hit">Raycast hit position</param>
        /// <param name="brush">The brush used</param>
        /// <returns></returns>
        int CalculateXbase(Vector3 hit, TerrainBrush brush)
        {
            // get position of the hit as percentile of the terrain X axis
            float tempXCoord = (hit - m_Terrain.gameObject.transform.position).x / m_Terrain.terrainData.size.x;

            // get the position of the terrain heightmap where the hit is
            int posXInTerrain = (int)(tempXCoord * m_HmWidth);

            int posXrest = 0;
            bool posXoverflow = false;

            if (posXInTerrain + m_Brush.m_BrushWidth / 2 > m_HmWidth)
            {
                posXrest = (posXInTerrain + m_Brush.m_BrushWidth / 2) % m_HmWidth;
                posXoverflow = true;
            }
            else if (posXInTerrain - m_Brush.m_BrushWidth / 2 < 0)
            {
                posXrest = (posXInTerrain - m_Brush.m_BrushWidth / 2) % m_HmWidth;
            }
            brush.m_BrushWidth = Mathf.Abs(m_ModificationDiameter - Mathf.Abs(posXrest));

            int offsetX = brush.m_BrushWidth / 2;

            return Mathf.Clamp(posXoverflow ? posXInTerrain + offsetX : posXInTerrain - offsetX, 0, m_HmWidth - brush.m_BrushWidth);
        }

        /// <summary>
        /// Calculates the smallest Y/Z position based on the raycast hit and brushsettings
        /// </summary>
        /// <param name="hit">Raycast hit position</param>
        /// <param name="brush">The brush used</param>
        int CalculateYbase(Vector3 hit, TerrainBrush brush)
        {
            // get position of the hit as percentile of the terrain Y axis
            float tempYCoord = (hit - m_Terrain.gameObject.transform.position).z / m_Terrain.terrainData.size.z;

            // get the position of the terrain heightmap where the hit is
            int posYInTerrain = (int)(tempYCoord * m_HmHeight);

            int posYrest = 0;
            bool posYoverflow = false;

            if (posYInTerrain + m_Brush.m_BrushHeight / 2 > m_HmHeight)
            {
                posYrest = (posYInTerrain + m_Brush.m_BrushHeight / 2) % m_HmHeight;
                posYoverflow = true;
            }
            else if (posYInTerrain - m_Brush.m_BrushHeight / 2 < 0)
            {
                posYrest = (posYInTerrain - m_Brush.m_BrushHeight / 2) % m_HmHeight;
            }
            brush.m_BrushHeight = Mathf.Abs(m_ModificationDiameter - Mathf.Abs(posYrest));

            int offsetY = brush.m_BrushHeight / 2;

            return Mathf.Clamp(posYoverflow ? posYInTerrain + offsetY : posYInTerrain - offsetY, 0, m_HmHeight - brush.m_BrushHeight);
        }
    }
}