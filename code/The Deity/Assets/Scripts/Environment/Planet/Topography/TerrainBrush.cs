/*
    Written by Tobias Lenz
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Environment.Topography
{
    /// <summary>
    /// Possible Shapes for the Modification
    /// </summary>
    public enum TerrainModificationShape
    {
        Square,
        Circle
    }

    /// <summary>
    /// Type of Modification
    /// </summary>
    public enum TerrainModificationType
    {
        Raise,
        Lower,
        Flatten,
        Smooth
    }

    /// <summary>
    /// Stores the information needed for terrain modifications, set the shape and type, width, height and speed for the terrain modification
    /// </summary>
    public class TerrainBrush
    {
        public TerrainModificationShape m_Shape;
        public TerrainModificationType m_ModificationType;
        public int m_BrushWidth;
        public int m_BrushHeight;
        public float m_ModificationSpeed;

        private float m_MinHeight;
        private float m_MaxHeight;

        /// <summary>
        /// Constructor of the Terrain Brush
        /// </summary>
        /// <param name="minHeight">Lower modification bound for height</param>
        /// <param name="maxHeight">High modification bound for height</param>
        /// <param name="shape">The desired shape of the brush</param>
        /// <param name="modificationType">The type of the Modification</param>
        /// <param name="brushWidth">The width of the brush</param>
        /// <param name="brushHeight">The height of the brush</param>
        /// <param name="modSpeed">Speed of the Terrain Modification</param>
        public TerrainBrush(float minHeight, float maxHeight, TerrainModificationShape shape = TerrainModificationShape.Square, TerrainModificationType modificationType = TerrainModificationType.Raise, int brushWidth = 50, int brushHeight = 50, float modSpeed = 10f)
        {
            m_Shape = shape;
            m_BrushWidth = brushWidth;
            m_BrushHeight = brushHeight;
            m_ModificationSpeed = modSpeed;
            m_ModificationType = modificationType;
            m_MinHeight = minHeight;
            m_MaxHeight = maxHeight;
        }

        /// <summary>
        /// Lower the terrain
        /// </summary>
        /// <param name="heightData">Terrain height data</param>
        /// <param name="terrainData">Terrain Data used</param>
        private void LowerTerrain(float[,] heightData, TerrainData terrainData)
        {
            switch (m_Shape)
            {
                case TerrainModificationShape.Square:
                    RectangleBrush(TerrainModificationType.Lower, heightData, terrainData);
                    break;
                case TerrainModificationShape.Circle:
                    CircleBrush(TerrainModificationType.Lower, heightData, terrainData);
                    break;
            }         
        }

        /// <summary>
        /// Raise the terrain
        /// </summary>
        /// <param name="heightData">Terrain height data</param>
        /// <param name="terrainData">Terrain Data used</param>
        private void RaiseTerrain(float[,] heightData, TerrainData terrainData)
        {
            switch (m_Shape)
            {
                case TerrainModificationShape.Square:
                    RectangleBrush(TerrainModificationType.Raise, heightData, terrainData);
                    break;
                case TerrainModificationShape.Circle:
                    CircleBrush(TerrainModificationType.Raise, heightData, terrainData);
                    break;
            }
        }

        /// <summary>
        /// Smooth the terrain
        /// </summary>
        /// <param name="heightData">Terrain height data</param>
        /// <param name="terrainData">Terrain Data used</param>
        private void SmoothTerrain(float[,] heightData, TerrainData terrainData)
        {
            switch (m_Shape)
            {
                case TerrainModificationShape.Square:
                    RectangleBrush(TerrainModificationType.Smooth, heightData, terrainData);
                    break;
                case TerrainModificationShape.Circle:
                    CircleBrush(TerrainModificationType.Smooth, heightData, terrainData);
                    break;
            }
        }

        /// <summary>
        /// Flatten the terrain
        /// </summary>
        /// <param name="heightData">Terrain height data</param>
        /// <param name="terrainData">Terrain Data used</param>
        private void FlattenTerrain(float[,] heightData, TerrainData terrainData)
        {
            switch (m_Shape)
            {
                case TerrainModificationShape.Square:
                    RectangleBrush(TerrainModificationType.Flatten, heightData, terrainData);
                    break;
                case TerrainModificationShape.Circle:
                    CircleBrush(TerrainModificationType.Flatten, heightData, terrainData);
                    break;
            }
        }

        /// <summary>
        /// Apply the Rectangle Brush
        /// </summary>
        /// <param name="modType">The modification type to apply</param>
        /// <param name="heightData">Terrain height data</param>
        /// <param name="terrainData">Terrain Data used</param>
        private void RectangleBrush(TerrainModificationType modType, float[,] heightData, TerrainData terrainData)
        {
            int offsetX = m_BrushWidth / 2;
            int offsetY = m_BrushHeight / 2;

            for (int i = 0; i < m_BrushHeight; i++)
            {
                for (int j = 0; j < m_BrushWidth; j++)
                {
                    switch (modType)
                    {
                        case TerrainModificationType.Raise:
                            heightData[i, j] = NewRaiseHeight(heightData[i, j], terrainData);
                            break;
                        case TerrainModificationType.Lower:
                            heightData[i, j] = NewLowerHeight(heightData[i, j], terrainData);
                            break;
                        case TerrainModificationType.Flatten:
                            heightData[i, j] = NewFlattenHeight(heightData[offsetX, offsetY], terrainData);
                            break;
                        case TerrainModificationType.Smooth:
                            SmoothTerrainOperation(1, heightData, i, j);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Apply the Circle Brush
        /// </summary>
        /// <param name="modType">The modification type to apply</param>
        /// <param name="heightData">Terrain height data</param>
        /// <param name="terrainData">Terrain Data used</param>
        private void CircleBrush(TerrainModificationType modType, float[,] heightData, TerrainData terrainData)
        {
            int offsetX = m_BrushWidth / 2;
            int offsetY = m_BrushHeight / 2;

            for (int i = 0; i < m_BrushHeight; i++)
            {
                for (int j = 0; j < m_BrushWidth; j++)
                {
                    float currentRadiusSqr = new Vector2(offsetX - j, offsetY - i).sqrMagnitude;
                    if (currentRadiusSqr < offsetX * offsetY)
                    {
                        switch (modType)
                        {
                            case TerrainModificationType.Raise:
                                heightData[i, j] = NewRaiseHeight(heightData[i, j], terrainData);
                                break;
                            case TerrainModificationType.Lower:
                                heightData[i, j] = NewLowerHeight(heightData[i, j], terrainData);
                                break;
                            case TerrainModificationType.Flatten:
                                heightData[i, j] = NewFlattenHeight(heightData[offsetX, offsetY], terrainData);
                                break;
                            case TerrainModificationType.Smooth:
                                SmoothTerrainOperation(1, heightData, j, i);
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Calculate the new height after raising
        /// </summary>
        /// <param name="currentHeight">Current terrain height</param>
        /// <param name="terrainData">The used terrain Data</param>
        /// <returns>The new height</returns>
        private float NewRaiseHeight(float currentHeight, TerrainData terrainData)
        {
            return Mathf.Clamp(currentHeight + (m_ModificationSpeed * Time.deltaTime) / terrainData.size.y, m_MinHeight/ terrainData.size.y, m_MaxHeight / terrainData.size.y);
        }

        /// <summary>
        /// Calculate the new height after lowering
        /// </summary>
        /// <param name="currentHeight">Current terrain height</param>
        /// <param name="terrainData">The used terrain Data</param>
        /// <returns>The new height</returns>
        private float NewLowerHeight(float currentHeight, TerrainData terrainData)
        {
            return Mathf.Clamp(currentHeight - (m_ModificationSpeed * Time.deltaTime) / terrainData.size.y, m_MinHeight / terrainData.size.y, m_MaxHeight / terrainData.size.y);
        }

        /// <summary>
        /// Calculate the new height after flattening
        /// </summary>
        /// <param name="currentHeight">Current terrain height</param>
        /// <param name="terrainData">The used terrain Data</param>
        /// <returns>The new height</returns>
        private float NewFlattenHeight(float targetHeight, TerrainData terrainData)
        {
            return Mathf.Clamp(targetHeight, m_MinHeight / terrainData.size.y, m_MaxHeight / terrainData.size.y);
        }

        /// <summary>
        /// Apply the Smooth Terrain operation
        /// </summary>
        /// <param name="Passes">Number of smoothing passes</param>
        /// <param name="heightData">Terrain height data</param>
        /// <param name="x">X Center Pos</param>
        /// <param name="y">Y/Z Center Pos</param>
        private void SmoothTerrainOperation(int Passes, float[,] heightData, int x, int y)
        {
            int hWidth = heightData.GetLength(0);
            int hHeight = heightData.GetLength(1);

            while (Passes > 0)
            {
                Passes--;

                int adjacentSections = 0;
                float sectionsTotal = 0.0f;

                if ((x - 1) > 0) // Check to left
                {
                    sectionsTotal += heightData[x - 1, y];
                    adjacentSections++;

                    if ((y - 1) > 0) // Check up and to the left
                    {
                        sectionsTotal += heightData[x - 1, y - 1];
                        adjacentSections++;
                    }

                    if ((y + 1) < hHeight) // Check down and to the left
                    {
                        sectionsTotal += heightData[x - 1, y + 1];
                        adjacentSections++;
                    }
                }

                if ((x + 1) < hWidth) // Check to right
                {
                    sectionsTotal += heightData[x + 1, y];
                    adjacentSections++;

                    if ((y - 1) > 0) // Check up and to the right
                    {
                        sectionsTotal += heightData[x + 1, y - 1];
                        adjacentSections++;
                    }

                    if ((y + 1) < hHeight) // Check down and to the right
                    {
                        sectionsTotal += heightData[x + 1, y + 1];
                        adjacentSections++;
                    }
                }

                if ((y - 1) > 0) // Check above
                {
                    sectionsTotal += heightData[x, y - 1];
                    adjacentSections++;
                }

                if ((y + 1) < hHeight) // Check below
                {
                    sectionsTotal += heightData[x, y + 1];
                    adjacentSections++;
                }

                heightData[x, y] = (heightData[x, y] + (sectionsTotal / adjacentSections)) * 0.5f;
            }
        }

        /// <summary>
        /// Use the Brush based on its settings
        /// </summary>
        /// <param name="currentHeight">Current terrain height</param>
        /// <param name="terrainData">The used terrain Data</param>
        public void UseBrush(float[,] heightData, TerrainData terrainData)
        {
            switch (m_ModificationType)
            {
                case TerrainModificationType.Raise:
                    RaiseTerrain(heightData, terrainData);
                    break;
                case TerrainModificationType.Lower:
                    LowerTerrain(heightData, terrainData);
                    break;
                case TerrainModificationType.Flatten:
                    FlattenTerrain(heightData, terrainData);
                    break;
                case TerrainModificationType.Smooth:
                    SmoothTerrain(heightData, terrainData);
                    break;
            }
        }
    }
}
