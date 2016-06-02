using UnityEngine;
using System.Collections.Generic;

public class EventLibrary {

    public static Dictionary<string, float> ActionDramas = new Dictionary<string, float>();
    

    public static void AddEvent(string eventName, float influence)
    {
        ActionDramas.Add(eventName, influence);
    }
}