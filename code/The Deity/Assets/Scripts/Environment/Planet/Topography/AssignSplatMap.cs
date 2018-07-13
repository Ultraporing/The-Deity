/*
    Written by Tobias Lenz
    modified from https://alastaira.wordpress.com/2013/11/14/procedural-terrain-splatmapping/
 */

using UnityEngine;
using System.Collections;
using System.Linq; // used for Sum of array
using Assets.Scripts.Environment.Planet;

namespace Assets.Scripts.Environment.Topography
{
    /// <summary>
    /// Creates the Terrain Material based on specified factors
    /// </summary>
    public class AssignSplatMap : MonoBehaviour
    {
        private float m_Unit;

        private TerrainData m_TerrainData;
        // Splatmap data is stored internally as a 3d array of floats, so declare a new empty array ready for your custom splatmap data:
        private float[,,] m_SplatmapData;

        /// <summary>
        /// Initialize the basic values
        /// </summary>
        void Awake()
        {
            m_TerrainData = Terrain.activeTerrain.terrainData;
            m_Unit = 1f / (m_TerrainData.size.x - 1);
            m_SplatmapData = new float[m_TerrainData.alphamapWidth, m_TerrainData.alphamapHeight, m_TerrainData.alphamapLayers];
            PlanetDatalayer.Instance.GetManager<SurfaceMaterialManager>().CreateGroundMap(m_TerrainData);
        }

        /// <summary>
        /// Create the Terrain Material for the whole map on gamestart
        /// </summary>
        void Start()
        {
            AssignSplatMapToTerrain(0, 0, m_TerrainData.alphamapHeight, m_TerrainData.alphamapWidth);
        }

        /// <summary>
        /// Updates the Class and contains debug key to update the Material for the whole map
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AssignSplatMapToTerrain(0, 0, m_TerrainData.alphamapHeight, m_TerrainData.alphamapWidth);
            }
        }

        /// <summary>
        /// Update the Material in this rectangle containing terrain verticies
        /// </summary>
        /// <param name="ax">X coordinate of the rectangle</param>
        /// <param name="ay">Y coordinate of the rectangle, Z in woldcoords</param>
        /// <param name="ah">height of the rectangle</param>
        /// <param name="aw">width of the rectangle</param>
        public void AssignSplatMapToTerrain(int ax, int ay, int ah, int aw)
        {
            for (int y = ay; y < (ay + ah)-1; y++)
            {
                for (int x = ax; x < (ax + aw)-1; x++)
                {
                    // Normalise x/y coordinates to range 0-1 
                    float y_01 = (float)y / (float)m_TerrainData.alphamapHeight;
                    float x_01 = (float)x / (float)m_TerrainData.alphamapWidth;

                    // Setup an array to record the mix of texture weights at this point
                    float[] splatWeights = new float[m_TerrainData.alphamapLayers];

                    // get height and slope at corresponding point
                    float height = GetHeightAtPoint(x_01 * m_TerrainData.size.x, y_01 * m_TerrainData.size.z);
                    float slope = GetSlopeAtPoint(x_01 * m_TerrainData.size.x, y_01 * m_TerrainData.size.z);

                    //====Rules for applying different textures===========================
                    splatWeights[0] = 1 - slope; // decreases with slope (ground texture)

                    splatWeights[1] = slope; // increases with slope (rocky texture)

                    splatWeights[2] = ( // apply 75% only to "Mesa" uplands (NOTE: the first two textures sum 1, so 1.5 corresponds to 80%)
                        height > 0.5f * m_TerrainData.heightmapHeight && // higher terrain
                        slope < 0.3f) // plain terrain
                            ? 1.5f : 0f;

                    splatWeights[3] = ( // apply 50% only to valley floor (NOTE: textures 2 and 3 never coexist, so 1 corresponds to 50%))
                        height < 0.5f * m_TerrainData.size.y && height > 0.2f * m_TerrainData.size.y && // lower terrain
                        slope < 0.3f) // plain terrain
                        ? 1f : 0f;

                    if (splatWeights[3] == 1f)
                    {
                        splatWeights[0] = 0;
                    }

                    if (splatWeights[3] == 0f)
                    {
                        splatWeights[1] = 0.2f;
                    }

                    if (height > 0.27f * m_TerrainData.heightmapHeight)
                    {
                        splatWeights[3] = 0;
                        splatWeights[1] = 0.75f;
                    }
                    //====================================================================

                    // Sum of all textures weights must add to 1, so calculate normalization factor from sum of weights
                    float z = splatWeights.Sum();

                    // Loop through each terrain texture
                    for (int i = 0; i < m_TerrainData.alphamapLayers; i++)
                    {

                        // Normalize so that sum of all texture weights = 1
                        splatWeights[i] /= z;
                        
                        // Assign this point to the splatmap array
                        m_SplatmapData[y, x, i] = splatWeights[i];
                        // NOTE: Alpha map array dimensions are shifted in relation to heightmap and world space (y is x and x is y or z)
                    }

                    // if its lower than ground level, its water
                    if (height <= 0.27f)
                    {
                        PlanetDatalayer.Instance.GetManager<SurfaceMaterialManager>().UpdateGroundMapAt(x, y, GroundMaterial.Water);
                    }
                    // if its rocky, set our material map to rock at this position
                    else if (splatWeights[1] >= 0.5f || splatWeights[2] >= 0.5f)
                    {
                        PlanetDatalayer.Instance.GetManager<SurfaceMaterialManager>().UpdateGroundMapAt(x, y, GroundMaterial.Rock);
                    }
                    // if its gras, set our material map to rock at this position
                    else if (splatWeights[3] >= 0.5f)
                    {
                        PlanetDatalayer.Instance.GetManager<SurfaceMaterialManager>().UpdateGroundMapAt(x, y, GroundMaterial.Gras);
                    }
                    // if all else fails, its rocks. just because
                    else
                    {
                        PlanetDatalayer.Instance.GetManager<SurfaceMaterialManager>().UpdateGroundMapAt(x, y, GroundMaterial.Rock);
                    }
                }
            }

            // Finally assign the new splatmap to the terrainData:
            m_TerrainData.SetAlphamaps(0, 0, m_SplatmapData);
        }

        /// <summary>
        /// Calculate the slope at this point in the world
        /// </summary>
        /// <param name="pointX">X Coord</param>
        /// <param name="pointZ">Z Coord</param>
        /// <returns>Value of the slope</returns>
        float GetSlopeAtPoint(float pointX, float pointZ)
        {
            return m_TerrainData.GetSteepness(m_Unit * pointX, m_Unit * pointZ) / 90f; // x and z coordinates must be scaled
        }

        /// <summary>
        /// Calculates the Height at this Point
        /// </summary>
        /// <param name="pointX">X Coord</param>
        /// <param name="pointZ">Z Coord</param>
        /// <param name="scaleToTerrain">Scale based on terrain</param>
        /// <returns>The height at this point</returns>
        float GetHeightAtPoint(float pointX, float pointZ, bool scaleToTerrain = false)
        {
            float height = m_TerrainData.GetInterpolatedHeight(m_Unit * pointX, m_Unit * pointZ);

            // x and z coordinates must be scaled with "unit"
            if (scaleToTerrain)
                return height / m_TerrainData.size.y;
            else
                return height;
        }

    }
}