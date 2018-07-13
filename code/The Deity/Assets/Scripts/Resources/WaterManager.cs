/*
    Written by Tobias Lenz
 */

using Assets.Scripts.Environment.Planet.Water;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Resources
{
    /// <summary>
    /// Contains a map of all waterdepths and list of riverbanks/coasts
    /// </summary>
    public class WaterManager : IManager
    {
        public float[,] WaterlevelMap { get; private set; }
        public Dictionary<Vector3, ResourceSource> RiverbankMap { get; set; }
        ResourceSourceWrapper WaterResourceSourceWrapper = null;

        /// <summary>
        /// Creates the Watermap based on the water and terrain
        /// </summary>
        /// <param name="terrain">Used Terrain</param>
        /// <param name="water">Used Water</param>
        /// <returns>true on success</returns>
        public bool CreateWaterMap(Terrain terrain, Water water)
        {
            if (terrain == null)
                return false;

            WaterlevelMap = new float[(int)terrain.terrainData.size.x, (int)terrain.terrainData.size.z];
            RiverbankMap = new Dictionary<Vector3, ResourceSource>();

            WaterResourceSourceWrapper = GameObject.Find("Water").GetComponent<ResourceSourceWrapper>();

            return UpdateWatermapForRegion(new Rect(0, 0, terrain.terrainData.size.x, terrain.terrainData.size.z), terrain, water);
        }

        /// <summary>
        /// Updates the Watermap in this Rectangular region
        /// </summary>
        /// <param name="region">Region to update</param>
        /// <param name="terrain">Used Terrain</param>
        /// <param name="water">Used Water</param>
        /// <returns>true if succesful</returns>
        public bool UpdateWatermapForRegion(Rect region, Terrain terrain, Water water)
        {
            if (WaterlevelMap == null)
                return false;

            for (int lx = (int)region.xMin; lx < (int)Mathf.Clamp(region.xMax, 0, WaterlevelMap.GetLength(0)); lx++)
            {
                for (int lz = (int)region.yMin; lz < (int)Mathf.Clamp(region.yMax, 0, WaterlevelMap.GetLength(1)); lz++)
                {
                    float terrainHeight = terrain.SampleHeight(new Vector3(lx, 0, lz));
                    float delta = water.transform.position.y - terrainHeight;
                    if (delta < 0)
                    {
                        delta = 0;
                    }

                    WaterlevelMap[lx, lz] = delta;
                }
            }

            for (int lx = (int)region.xMin; lx < (int)Mathf.Clamp(region.xMax, 0, WaterlevelMap.GetLength(0)); lx++)
            {
                for (int lz = (int)region.yMin; lz < (int)Mathf.Clamp(region.yMax, 0, WaterlevelMap.GetLength(1)); lz++)
                {
                    
                    UpdateRiverBankAt(new Vector2(lx, lz), terrain);
                }
            }

            return true;
        }

        /// <summary>
        /// Checks if the position has water
        /// </summary>
        /// <param name="pos">position to check</param>
        /// <returns>true if there is water</returns>
        public bool IsWaterAtPosition(Vector2 pos)
        {
            if (WaterlevelMap[(int)pos.x, (int)pos.y] > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if water is within the Rectangular area
        /// </summary>
        /// <param name="area">Area to check</param>
        /// <returns>true if it contains water</returns>
        public bool IsWaterInArea(Rect area)
        {
            for (float x = area.x; x < (area.x + area.width); x++)
            {
                for (float y = area.y; y < (area.y + area.height); y++)
                {
                    if (IsWaterAtPosition(new Vector2(x, y)))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Checks for riverbank at this point and updates it
        /// </summary>
        /// <param name="pos">Point to check</param>
        /// <param name="terrain">Terrain used</param>
        private void UpdateRiverBankAt(Vector2 pos, Terrain terrain)
        {
            float heightAtPos = terrain.SampleHeight(new Vector3(pos.x, 0, pos.y));

            float wLevel = WaterlevelMap[(int)pos.x, (int)pos.y];
            if (wLevel > 0)
            {
                RiverbankMap.Remove(new Vector3(pos.x, heightAtPos, pos.y));
                return;
            }

            bool isCurRiverbank = RiverbankMap.ContainsKey(new Vector3(pos.x, heightAtPos, pos.y));

            // Check if the bordering tiles are outside the map or are not water. 
            // If so check if the current pos is designates as riverbank.
            // If so remove the designation.
            // Else we have at least one water pos around the current pos, therefore its a riverbank.
            if ((((int)pos.x-1 < 0 || (int)pos.y-1 < 0 || (int)pos.x-1 >= WaterlevelMap.GetLength(0) || (int)pos.y-1 >= WaterlevelMap.GetLength(1)) || WaterlevelMap[(int)pos.x-1, (int)pos.y-1] == 0) &&
                (((int)pos.x < 0 || (int)pos.y-1 < 0 || (int)pos.x >= WaterlevelMap.GetLength(0) || (int)pos.y-1 >= WaterlevelMap.GetLength(1)) || WaterlevelMap[(int)pos.x, (int)pos.y-1] == 0) &&
                (((int)pos.x+1 < 0 || (int)pos.y-1 < 0 || (int)pos.x+1 >= WaterlevelMap.GetLength(0) || (int)pos.y-1 >= WaterlevelMap.GetLength(1)) || WaterlevelMap[(int)pos.x+1, (int)pos.y-1] == 0) &&
                (((int)pos.x+1 < 0 || (int)pos.y < 0 || (int)pos.x+1 >= WaterlevelMap.GetLength(0) || (int)pos.y >= WaterlevelMap.GetLength(1)) || WaterlevelMap[(int)pos.x+1, (int)pos.y] == 0) &&
                (((int)pos.x+1 < 0 || (int)pos.y+1 < 0 || (int)pos.x+1 >= WaterlevelMap.GetLength(0) || (int)pos.y+1 >= WaterlevelMap.GetLength(1)) || WaterlevelMap[(int)pos.x+1, (int)pos.y+1] == 0) &&
                (((int)pos.x < 0 || (int)pos.y+1 < 0 || (int)pos.x >= WaterlevelMap.GetLength(0) || (int)pos.y+1 >= WaterlevelMap.GetLength(1)) || WaterlevelMap[(int)pos.x, (int)pos.y+1] == 0) &&
                (((int)pos.x-1 < 0 || (int)pos.y+1 < 0 || (int)pos.x-1 >= WaterlevelMap.GetLength(0) || (int)pos.y+1 >= WaterlevelMap.GetLength(1)) || WaterlevelMap[(int)pos.x-1, (int)pos.y+1] == 0) &&
                (((int)pos.x-1 < 0 || (int)pos.y < 0 || (int)pos.x-1 >= WaterlevelMap.GetLength(0) || (int)pos.y >= WaterlevelMap.GetLength(1)) || WaterlevelMap[(int)pos.x - 1, (int)pos.y] == 0))
            {
                if (isCurRiverbank)
                {
                    RiverbankMap.Remove(new Vector3(pos.x, heightAtPos, pos.y));
                    return;
                }
            }
            else
            {
                if (!isCurRiverbank)
                {
                    RiverbankMap.Add(new Vector3(pos.x, heightAtPos, pos.y), new ResourceSource(WaterResourceSourceWrapper, new Vector3(pos.x, heightAtPos, pos.y), ResourceType.Water, 0, true));
                }
            }
        }

        /// <summary>
        /// Update Manager
        /// </summary>
        public void Update()
        {
            
        }
    }
}
