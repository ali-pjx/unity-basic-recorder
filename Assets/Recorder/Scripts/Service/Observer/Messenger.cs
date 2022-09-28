using System;
using System.Collections.Generic;
using UnityEngine;

namespace Base.Scripts.Observer
{
    public class Messenger<TObserverType>
    {
        #region Internal variables

        //Disable the unused variable warning
#pragma warning disable 0414
        //Ensures that the MessengerHelper will be created automatically upon start of the game.
        //private MessengerHelper messengerHelper = (new GameObject("MessengerHelper")).AddComponent<MessengerHelper>();
#pragma warning restore 0414

        public Dictionary<TObserverType, Delegate> eventTable = new Dictionary<TObserverType, Delegate>();

        //Message handlers that should never be removed, regardless of calling Cleanup
        public List<TObserverType> permanentMessages = new List<TObserverType>();

        #endregion

        #region Helper methods

        //Marks a certain message as permanent.
        public void MarkAsPermanent(TObserverType eventType)
        {
#if LOG_ALL_MESSAGES
		Debug.Log("Messenger MarkAsPermanent \t\"" + eventType + "\"");
#endif
            permanentMessages.Add(eventType);
        }


        public void Cleanup()
        {
#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER Cleanup. Make sure that none of necessary listeners are removed.");
#endif

            List<TObserverType> messagesToRemove = new List<TObserverType>();

            foreach (KeyValuePair<TObserverType, Delegate> pair in eventTable)
            {
                bool wasFound = false;

                foreach (TObserverType message in permanentMessages)
                {
                    //if (pair.Key == message)
                    if (pair.Key.Equals(message))
                    {
                        wasFound = true;
                        break;
                    }
                }

                if (!wasFound)
                    messagesToRemove.Add(pair.Key);
            }

            foreach (TObserverType message in messagesToRemove)
            {
                eventTable.Remove(message);
            }
        }

        public void ClearListener<T>(T message) where T : TObserverType
        {
            eventTable.Remove(message);
        }

        public void PrintEventTable()
        {
            Debug.Log("\t\t\t=== MESSENGER PrintEventTable ===");

            foreach (KeyValuePair<TObserverType, Delegate> pair in eventTable)
            {
                Debug.Log("\t\t\t" + pair.Key + "\t\t" + pair.Value);
            }

            Debug.Log("\n");
        }

        #endregion

        #region Message logging and exception throwing

        public void OnListenerAdding(TObserverType eventType, Delegate listenerBeingAdded)
        {
#if LOG_ALL_MESSAGES || LOG_ADD_LISTENER
		Debug.Log("MESSENGER OnListenerAdding \t\"" + eventType + "\"\t{" + listenerBeingAdded.Target + " -> " + listenerBeingAdded.Method + "}");
#endif

            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }

            Delegate d = eventTable[eventType];
            if (d != null && d.GetType() != listenerBeingAdded.GetType())
            {
                throw new ListenerException(string.Format(
                    "Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}",
                    eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
            }
        }

        public void OnListenerRemoving(TObserverType eventType, Delegate listenerBeingRemoved)
        {
#if LOG_ALL_MESSAGES
		Debug.Log("MESSENGER OnListenerRemoving \t\"" + eventType + "\"\t{" + listenerBeingRemoved.Target + " -> " + listenerBeingRemoved.Method + "}");
#endif

            if (eventTable.ContainsKey(eventType))
            {
                Delegate d = eventTable[eventType];

                if (d == null)
                {
                    throw new ListenerException(string.Format(
                        "Attempting to remove listener with for event type \"{0}\" but current listener is null.",
                        eventType));
                }
                else if (d.GetType() != listenerBeingRemoved.GetType())
                {
                    throw new ListenerException(string.Format(
                        "Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}",
                        eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
                }
            }
            else
            {
                throw new ListenerException(string.Format(
                    "Attempting to remove listener for type \"{0}\" but Messenger doesn't know about this event type.",
                    eventType));
            }
        }

        public void OnListenerRemoved(TObserverType eventType)
        {
            if (eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
            }
        }

        public void OnBroadcasting(TObserverType eventType)
        {
#if REQUIRE_LISTENER
            if (!eventTable.ContainsKey(eventType))
            {
                throw new BroadcastException(string.Format("Broadcasting message \"{0}\" but no listener found. Try marking the message with Messenger.MarkAsPermanent.", eventType));
            }
#endif
        }

        public BroadcastException CreateBroadcastSignatureException(TObserverType eventType)
        {
            return new BroadcastException(string.Format(
                "Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster.",
                eventType));
        }

        public class BroadcastException : Exception
        {
            public BroadcastException(string msg)
                : base(msg)
            {
            }
        }

        public class ListenerException : Exception
        {
            public ListenerException(string msg)
                : base(msg)
            {
            }
        }

        #endregion

        #region AddListener

        //No parameters
        public void AddListener(TObserverType eventType, Action handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action) eventTable[eventType] + handler;
        }

        //Single parameter
        public void AddListener<T>(TObserverType eventType, Action<T> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T>) eventTable[eventType] + handler;
        }

        //Two parameters
        public void AddListener<T, U>(TObserverType eventType, Action<T, U> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T, U>) eventTable[eventType] + handler;
        }

        //Three parameters
        public void AddListener<T, U, V>(TObserverType eventType, Action<T, U, V> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T, U, V>) eventTable[eventType] + handler;
        }

        //Four parameters
        public void AddListener<T, U, V, W>(TObserverType eventType, Action<T, U, V, W> handler)
        {
            OnListenerAdding(eventType, handler);
            eventTable[eventType] = (Action<T, U, V, W>) eventTable[eventType] + handler;
        }

        #endregion

        #region RemoveListener

        //No parameters
        public void RemoveListener(TObserverType eventType, Action handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action) eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Single parameter
        public void RemoveListener<T>(TObserverType eventType, Action<T> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T>) eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Two parameters
        public void RemoveListener<T, U>(TObserverType eventType, Action<T, U> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U>) eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Three parameters
        public void RemoveListener<T, U, V>(TObserverType eventType, Action<T, U, V> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U, V>) eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        //Four parameters
        public void RemoveListener<T, U, V, W>(TObserverType eventType, Action<T, U, V, W> handler)
        {
            OnListenerRemoving(eventType, handler);
            eventTable[eventType] = (Action<T, U, V, W>) eventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }

        #endregion

        #region Broadcast

        //No parameters
        public void Broadcast(TObserverType eventType)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action Action = d as Action;

                if (Action != null)
                {
                    Action();
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Single parameter
        public void Broadcast<T>(TObserverType eventType, T arg1)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T> Action = d as Action<T>;

                if (Action != null)
                {
                    Action(arg1);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Two parameters
        public void Broadcast<T, U>(TObserverType eventType, T arg1, U arg2)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T, U> Action = d as Action<T, U>;

                if (Action != null)
                {
                    Action(arg1, arg2);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Three parameters
        public void Broadcast<T, U, V>(TObserverType eventType, T arg1, U arg2, V arg3)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T, U, V> Action = d as Action<T, U, V>;

                if (Action != null)
                {
                    Action(arg1, arg2, arg3);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        //Four parameters
        public void Broadcast<T, U, V, W>(TObserverType eventType, T arg1, U arg2, V arg3, W arg4)
        {
#if LOG_ALL_MESSAGES || LOG_BROADCAST_MESSAGE
		Debug.Log("MESSENGER\t" + System.DateTime.Now.ToString("hh:mm:ss.fff") + "\t\t\tInvoking \t\"" + eventType + "\"");
#endif
            OnBroadcasting(eventType);

            Delegate d;
            if (eventTable.TryGetValue(eventType, out d))
            {
                Action<T, U, V, W> Action = d as Action<T, U, V, W>;

                if (Action != null)
                {
                    Action(arg1, arg2, arg3, arg4);
                }
                else
                {
                    throw CreateBroadcastSignatureException(eventType);
                }
            }
        }

        #endregion
    }

    //This manager will ensure that the messenger's eventTable will be cleaned up upon loading of a new level.
    public sealed class MessengerHelper : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        //Clean up eventTable every time a new level loads.
        public void OnLevelWasLoaded<T>(int unused, Messenger<T> messenger)
        {
            messenger.Cleanup();
        }
    }
}