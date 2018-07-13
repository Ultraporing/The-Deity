/*
    Written by Tobias Lenz
 */

using Assets.Scripts.Environment.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Resources
{
    /// <summary>
    /// Possible Biomes
    /// </summary>
    public enum Biomes
    {
        Unknown,
        GrassLand,
        Forest,
        BushLand,
        Fallow
    }

    /// <summary>
    /// Contains a full terrain map as biomes
    /// </summary>
    public class BiomeManager : IManager
    {
        public Biomes[,] BiomeMap { get; private set; }

        /// <summary>
        /// Creates the BiomeMap based on the provided texture
        /// </summary>
        /// <param name="biomeTex">Biome Texture</param>
        /// <returns>true if successful</returns>
        public bool CreateBiomeMap(Texture2D biomeTex)
        {
            Color[] colorToEnumMapping = {
                new Color32(185, 122, 87, 1), // grassLand
                new Color32(34, 177, 76, 1), // forest
                new Color32(181, 230, 29, 1), // bushLand
                new Color32(255, 242, 0, 1) // fallow
            };

            if (biomeTex == null)
                return false;

            Biomes[,] outArr = new Biomes[biomeTex.width, biomeTex.height];

            Color[] pixels = biomeTex.GetPixels(0, 0, biomeTex.width, biomeTex.height);
            
            for (int i = 0; i < pixels.Length; i++)
            {
                int x = i % biomeTex.width;
                int y = i / biomeTex.width;

                outArr[x, y] = Biomes.Unknown;

                for (int j = 0; j < colorToEnumMapping.Length; j++)
                {
                    if (colorToEnumMapping[j].r == pixels[i].r && colorToEnumMapping[j].g == pixels[i].g && colorToEnumMapping[j].b == pixels[i].b)
                    {
                        outArr[x, y] = (Biomes)j + 1;
                    }
                }
            }

            BiomeMap = outArr;

            return true;
        }

        /// <summary>
        /// Update the Manager
        /// </summary>
        public void Update()
        {
            
        }
    }
}
