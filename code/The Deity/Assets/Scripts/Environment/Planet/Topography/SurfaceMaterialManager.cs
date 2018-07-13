/*
    Written by Tobias Lenz
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Environment.Topography
{
    /// <summary>
    /// Contains a Map of the whole terrain in ground material (Water, Gras, Stone).
    /// </summary>
    public class SurfaceMaterialManager : IManager
    {
        public GroundMaterial[,] GroundMaterial { get; private set; }

        /// <summary>
        /// Initializes the GroundMaterial 2D Array based on the Terrain
        /// </summary>
        /// <param name="terrainData">The TerrainData to be used</param>
        /// <returns>true on success</returns>
        public bool CreateGroundMap(TerrainData terrainData)
        {
            if (terrainData == null)
                return false;

            GroundMaterial = new GroundMaterial[(int)terrainData.size.x, (int)terrainData.size.z];

            return true;
        }

        /// <summary>
        /// Update the GroundMaterial on this point
        /// </summary>
        /// <param name="x">X Coord</param>
        /// <param name="z">Z Coord</param>
        /// <param name="groundMaterial">New Material</param>
        /// <returns></returns>
        public bool UpdateGroundMapAt(int x, int z, GroundMaterial groundMaterial)
        {
            if ((x >= 0 && x < GroundMaterial.GetLength(0)) && (z >= 0 && z < GroundMaterial.GetLength(1)))
            {
                GroundMaterial[x, z] = groundMaterial;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check point against the provided material
        /// </summary>
        /// <param name="x">X Coord</param>
        /// <param name="z">Y Coord</param>
        /// <param name="groundMaterial">Material to check against</param>
        /// <returns>true if the Materials are the same</returns>
        public bool IsGroundMaterialAt(int x, int z, GroundMaterial groundMaterial)
        {
            if ((x >= 0 && x < GroundMaterial.GetLength(0)) && (z >= 0 && z < GroundMaterial.GetLength(1)))
            {
                return GroundMaterial[x, z] == groundMaterial;
            }

            return false;
        }

        /// <summary>
        /// Update the Manager
        /// </summary>
        public void Update()
        {
            
        }
    }
}