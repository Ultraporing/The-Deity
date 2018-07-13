/*
    Written by Tobias Lenz
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Helper.Threads
{
    /// <summary>
    /// A Threaded Function which gets executet in its own CPU thread
    /// </summary>
    public class FunctionThread
    {
        /// <summary>
        /// Contains the Reference to the Thread and his array of parameters
        /// </summary>
        class ThreadContainer
        {
            public Thread Thread;
            public object[] Params;
        }

        public delegate void OnDoneCallback(object result);
        private static Stack<ThreadContainer> m_ThreadStack = new Stack<ThreadContainer>();
        private static bool m_WorkerRunning = false;

        /// <summary>
        /// Starts Stack Worker thread launching all threads on the stack
        /// </summary>
        public static void RunStackWorker()
        {
            Thread funcThread = new Thread(StackWorker)
            {
                IsBackground = true
            };
            m_WorkerRunning = true;
            funcThread.Start();
        }

        /// <summary>
        /// Stops the Stack Worker thread
        /// </summary>
        public static void StopStackWorker()
        {
            m_WorkerRunning = false;
        }

        /// <summary>
        /// The actual Stack Worker method that gets executed
        /// </summary>
        private static void StackWorker()
        {
            while (m_WorkerRunning)
            {
                if (m_ThreadStack.Count > 0)
                {
                    ThreadContainer tc = m_ThreadStack.Pop();
                    tc.Thread.Start(tc.Params);
                }
            }
        }

        /// <summary>
        /// Executes a perticular function in it's own thread and calls after completion the onDoneCallback with the return value as parameter
        /// </summary>
        /// <param name="functionToCall">The function you like to call.</param>
        /// <param name="functionOwner">null if it is a static function. Otherwise refrence to instance of the object owning the function.</param>
        /// <param name="onDoneCallback">Callback which gets executed once the work in the functionToCall is done.</param>
        /// <param name="functionParameters">Parameters needed for the functionToCall.</param>
        public static void ExecuteFunction(Delegate functionToCall, object functionOwner, OnDoneCallback onDoneCallback, params object[] functionParameters)
        {
            if (AreParametersCorrect(functionToCall.Method, functionParameters) && functionToCall != null)
            {
                
                Thread funcThread = new Thread(new ParameterizedThreadStart(CallFunction))
                {
                    IsBackground = true
                };
                m_ThreadStack.Push(new ThreadContainer() { Thread = funcThread, Params = new object[] { functionToCall.Method, onDoneCallback, functionParameters, functionOwner } });
            }
        }

        /// <summary>
        /// Checks if the provided parameters against the Method Information to see if they are correct
        /// </summary>
        /// <param name="methodInfo">The MethodInfo</param>
        /// <param name="functionParameters">The Provided Parameters</param>
        /// <returns>true if they are correct</returns>
        private static bool AreParametersCorrect(MethodInfo methodInfo, object[] functionParameters)
        {
            ParameterInfo[] parameterInfoMethod = methodInfo.GetParameters();

            if (functionParameters != null)
            {
                if (parameterInfoMethod.Length != functionParameters.Length)
                {
                    Debug.LogError("<" + methodInfo.DeclaringType.Name + "." + methodInfo.Name + "> Parameter number mismatch!");
                    return false;
                }     

                for (int i = 0; i < parameterInfoMethod.Length; i++)
                {
                    if (parameterInfoMethod[i].ParameterType != functionParameters[i].GetType())
                    {
                        Debug.LogError("<" + methodInfo.DeclaringType.Name + "." + methodInfo.Name + "> Parameter Type mismatch! Expected: " + parameterInfoMethod[i].ParameterType.Name + ", Received: " + functionParameters[i].GetType().Name);
                        return false;
                    }
                }
            }
            else if (parameterInfoMethod.Length != 0)
            {
                Debug.LogError("<" + methodInfo.DeclaringType.Name + "." + methodInfo.Name + "> Parameter number mismatch!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Method which calls the function
        /// </summary>
        /// <param name="data">Array containing MethodInfo, OnDoneCallback, functionParameters Array, functionOwner reference</param>
        private static void CallFunction(object data)
        {
            object[] dataParams = (object[])data;
            MethodInfo funcToCall = (MethodInfo)dataParams[0];
            OnDoneCallback onDoneCallback = (OnDoneCallback)dataParams[1];
            object[] functionParams = (object[])dataParams[2];
            object funcOwner = dataParams[3];

            object result = funcToCall.Invoke(funcToCall.IsStatic ? null : funcOwner, functionParams);
            if (onDoneCallback != null)
                onDoneCallback(result);
        }
    }
}
