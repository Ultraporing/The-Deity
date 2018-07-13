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
    /// Contains all possible worldevents as well as their probabilities and checks if a event should be triggered
    /// </summary>
    public class WorldEventManager : IManager
    {
        public WorldEvent m_CurrentEvent = null;
        public KeyValuePair<double, WorldEvent>[] m_WorldEvents;
        public delegate void EventDoneCallback(WorldEvent sender);
        private EventDoneCallback EventDoneCallbackHandler;
        public float elapsed = 0;
        private float checkEvents = 120; 

        /// <summary>
        /// Constructor of the Manager initializing the World Events Array with all possible Events
        /// </summary>
        public WorldEventManager()
        {
            EventDoneCallbackHandler = EventDone;

            m_WorldEvents = new KeyValuePair<double, WorldEvent>[]
            {
                new KeyValuePair<double, WorldEvent>(0.3, new Famine(EventDoneCallbackHandler)),
                new KeyValuePair<double, WorldEvent>(0.2, new ForestFire(EventDoneCallbackHandler)),
                new KeyValuePair<double, WorldEvent>(0.2, new Herecy(EventDoneCallbackHandler)),
            };
        }

        /// <summary>
        /// Updates running events and checks if a new event should be started
        /// </summary>
        public void Update()
        {
            if (m_CurrentEvent == null)
            {
                if (elapsed >= checkEvents)
                {
                    
                    IEnumerable<KeyValuePair<double, WorldEvent>> aviableEvents = m_WorldEvents.Where(ev => ev.Value.PrerequisitesMet());
                    foreach (KeyValuePair<double, WorldEvent> ev in aviableEvents)
                    {
                        float probability = UnityEngine.Random.Range(0, 100) / 100f;
                        if (probability <= ev.Key)
                        {
                            m_CurrentEvent = ev.Value;
                            m_CurrentEvent.Start();
                            break;
                        }
                    }
                    elapsed = 0;
                }
                elapsed += Time.deltaTime;
            }
            else
            {
                m_CurrentEvent.Update();
            }
        }

        /// <summary>
        /// Recieves the Event Done Event and sets the current event to null
        /// </summary>
        /// <param name="sender">The event sending that it is done</param>
        private void EventDone(WorldEvent sender)
        {
            m_CurrentEvent = null;
        }
    }
}
