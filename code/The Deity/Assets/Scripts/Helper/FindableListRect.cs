/*
    Written by Tobias Lenz
 */

using Assets.Scripts.Helper.Primitive;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Helper
{
    /// <summary>
    /// Inherits FindableList and uses the key as Rect.
    /// Allowes searching based on Overlapping of rectangles and circles
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class FindableListRect<T> : FindableList<Rect, T>
    {
        /// <summary>
        /// Insert into Dictionary
        /// </summary>
        /// <param name="value">The Value</param>
        /// <param name="bounds">The Rectangle</param>
        public void Insert(T value, Rect bounds)
        {
            m_Elements.Add(bounds, value);
        }

        /// <summary>
        /// Find items in the Dictionary wich overlapp with this Rectangle
        /// </summary>
        /// <param name="area">Rectangle to check against</param>
        /// <returns>List of overlapping Items, otherwise empty list</returns>
        public List<T> Find(Rect area)
        {
            List<T> outList = new List<T>();

            foreach (KeyValuePair<Rect, T> kv in m_Elements)
            {
                if (kv.Key.Overlaps(area, true))
                {
                    outList.Add(kv.Value);
                }
            }

            return outList;
        }

        /// <summary>
        /// Find items in the Dictionary wich overlapp with this Circle
        /// </summary>
        /// <param name="area">Circle to check against</param>
        /// <returns>List of overlapping Items, otherwise empty list</returns>
        public List<T> Find(Circle area)
        {
            List<T> outList = new List<T>();

            foreach (KeyValuePair<Rect, T> kv in m_Elements)
            {
                if (area.Overlaps(kv.Key))
                {
                    outList.Add(kv.Value);
                }
            }

            return outList;
        }
    }
}
