/*
    Written by Tobias Lenz
 */

using UnityEngine;
using System.Collections;

namespace Assets.Scripts.Helper.Threads
{
    /// <summary>
    /// Unity wrapper to execute the FunctionThreads
    /// </summary>
    public class ThreadManager : MonoBehaviour
    {
        /// <summary>
        /// Start the Stack Worker
        /// </summary>
        void Start()
        {
            FunctionThread.RunStackWorker();
        }

        /// <summary>
        /// Stop the Stack Worker when the Program closes
        /// </summary>
        private void OnApplicationQuit()
        {
            FunctionThread.StopStackWorker();
        }
    }
}