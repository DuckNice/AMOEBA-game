using UnityEngine;
using System.Collections.Generic;

public class EventLibrary {

    public static Dictionary<string, float> EventTypes = new Dictionary<string, float>();
    
    public static void AddEvent(string eventName, float influence)
    {
        lock(EventTypes)
            EventTypes.Add(eventName, influence);
    }
}