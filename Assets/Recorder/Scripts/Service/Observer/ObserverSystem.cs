using System;
using Base.Scripts.Observer;
using UnityEngine;

namespace Recorder.Scripts.Service.Observer
{
    public enum EObserver
    {
        // Replay
        REPLAY_AUTO,
        REPLAY_KEYBOARD,
        REPLAY_STOP,
        REPLAY_SLIDER_VALUE
    }

    public class ObserverSystem : MonoBehaviour
    {
        public static ObserverSystem ins;

        private const int INITIALIZE_EVENT_ID = -1000;
        
        private void Awake()
        {
            if (ins == null)
            {
                ins = this;
                DontDestroyOnLoad(transform.root.gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        // Observer system supports up to 4 properties
        private Messenger<int> _observerMessenger = new Messenger<int>();

        public void ListenToEvent(EObserver messageId, Action callEvent)
        {
            _observerMessenger.AddListener((int) messageId, callEvent);
        }

        public void ListenToEvent<T1>(EObserver messageId, Action<T1> callEvent)
        {
            _observerMessenger.AddListener<T1>((int) messageId, callEvent);
        }

        public void ListenToEvent<T1, T2>(EObserver messageId, Action<T1, T2> callEvent)
        {
            _observerMessenger.AddListener<T1, T2>((int) messageId, callEvent);
        }

        public void ListenToEvent<T1, T2, T3>(EObserver messageId, Action<T1, T2, T3> callEvent)
        {
            _observerMessenger.AddListener<T1, T2, T3>((int) messageId, callEvent);
        }

        public void ListenToEvent<T1, T2, T3, T4>(EObserver messageId, Action<T1, T2, T3, T4> callEvent)
        {
            _observerMessenger.AddListener<T1, T2, T3, T4>((int) messageId, callEvent);
        }

        public void BroadcastEvent(EObserver messageId)
        {
            _observerMessenger.Broadcast((int) messageId);
        }

        public void BroadcastEvent<T1>(EObserver messageId, T1 inputValue)
        {
            _observerMessenger.Broadcast<T1>((int) messageId, inputValue);
        }

        public void BroadcastEvent<T1, T2>(EObserver messageId, T1 inputValue1, T2 inputValue2)
        {
            _observerMessenger.Broadcast((int) messageId, inputValue1, inputValue2);
        }

        public void BroadcastEvent<T1, T2, T3>(EObserver messageId, T1 inputValue1, T2 inputValue2, T3 inputValue3)
        {
            _observerMessenger.Broadcast((int) messageId, inputValue1, inputValue2, inputValue3);
        }

        public void BroadcastEvent<T1, T2, T3, T4>(EObserver messageId, T1 inputValue1, T2 inputValue2,
            T3 inputValue3,
            T4 inputValue4)
        {
            _observerMessenger.Broadcast((int) messageId, inputValue1, inputValue2, inputValue3, inputValue4);
        }

        public void RemoveEventListener<T1>(EObserver messageId, Action<T1> callEvent)
        {
            _observerMessenger.RemoveListener<T1>((int) messageId, callEvent);
        }

        public void RemoveEventListener<T1, T2>(EObserver messageId, Action<T1, T2> callEvent)
        {
            _observerMessenger.RemoveListener<T1, T2>((int) messageId, callEvent);
        }

        public void RemoveEventListener<T1, T2, T3>(EObserver messageId, Action<T1, T2, T3> callEvent)
        {
            _observerMessenger.RemoveListener<T1, T2, T3>((int) messageId, callEvent);
        }

        public void RemoveEventListener<T1, T2, T3, T4>(EObserver messageId, Action<T1, T2, T3, T4> callEvent)
        {
            _observerMessenger.RemoveListener<T1, T2, T3, T4>((int) messageId, callEvent);
        }

        public void RemoveEventListener(EObserver messageId, Action callEvent)
        {
            _observerMessenger.RemoveListener((int) messageId, callEvent);
        }

        public void ListenToInitializeEvent(Action<IServiceProvider> callEvent)
        {
            _observerMessenger.AddListener(INITIALIZE_EVENT_ID, callEvent);
        }

        public void BroadcastInitializeEvent(IServiceProvider inputValue)
        {
            _observerMessenger.Broadcast(INITIALIZE_EVENT_ID, inputValue);
        }
    }
}