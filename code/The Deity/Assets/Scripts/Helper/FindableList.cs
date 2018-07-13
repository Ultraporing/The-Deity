/*
    Written by Tobias Lenz
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Helper
{
    /// <summary>
    /// Basic Dictionary implementation which can find items based on conditions
    /// </summary>
    /// <typeparam name="TK">Key Type</typeparam>
    /// <typeparam name="T">Value Type</typeparam>
    public class FindableList<TK, T> 
    {
        public Dictionary<TK, T> m_Elements = new Dictionary<TK, T>();

        /// <summary>
        /// Insert item into dictionary
        /// </summary>
        /// <param name="key">The Key</param>
        /// <param name="value">The Value</param>
        public void Insert(TK key, T value)
        {
            m_Elements.Add(key, value);
        }

        /// <summary>
        /// Find items based on the provided condition
        /// </summary>
        /// <param name="condition">A function detailing the search condition</param>
        /// <returns>List with the found items, otherwise empty list</returns>
        public List<T> Find(Func<KeyValuePair<TK, T>, bool> condition)
        {
            List<T> outList = new List<T>();

            foreach (KeyValuePair<TK, T> kv in m_Elements.Where(condition))
            {
                outList.Add(kv.Value);
            }

            return outList;
        }
    }
}