/*
    Written by Tobias Lenz
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Events
{
    /// <summary>
    /// Baseclass for World Events, contains references to the FoM Manager and the Event Done Callback
    /// </summary>
    public abstract class WorldEvent
    {
        private WorldEventManager.EventDoneCallback m_EventDoneCallback;
        protected ManageFoM m_ManageFoM = null;

        /// <summary>
        /// Constructor of the World Event Baseclass
        /// </summary>
        /// <param name="eventDoneCallback">Reference to the Event done Callback</param>
        public WorldEvent(WorldEventManager.EventDoneCallback eventDoneCallback)
        {
            m_EventDoneCallback = eventDoneCallback;
            m_ManageFoM = Transform.FindObjectOfType<ManageFoM>();
        }

        /// <summary>
        /// Checks if the prerequisites are met to start the Event
        /// </summary>
        /// <returns>true if prerequisites are met</returns>
        public abstract bool PrerequisitesMet();

        /// <summary>
        /// Starts the Event
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Updates the event
        /// </summary>
        public abstract void Update();

        /// <summary>
        /// Calls the Event Done Callback
        /// </summary>
        protected virtual void End()
        {
            m_EventDoneCallback(this);
        }
    }
}
