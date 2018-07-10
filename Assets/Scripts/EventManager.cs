using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    private Dictionary<string, UnityEvent> mEventDictionary;

    private static EventManager mEventManager;

    private void Start()
    { }

    public static EventManager Instance
    {
        get
        {
            if (!mEventManager)
            {
                mEventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!mEventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    mEventManager.Initialise();
                }
            }
            return mEventManager;
        }
    }

    public static void StartListening(string eventName, UnityAction listener)
    {
        UnityEvent thisEvent = null;
        if (Instance.mEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.mEventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
        if (mEventManager == null)
        {
            return;
        }
        UnityEvent thisEvent = null;
        if (Instance.mEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (Instance.mEventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke();
        }
    }

    private void Initialise()
    {
        if (mEventDictionary == null)
        {
            mEventDictionary = new Dictionary<string, UnityEvent>();
        }
    }
}
