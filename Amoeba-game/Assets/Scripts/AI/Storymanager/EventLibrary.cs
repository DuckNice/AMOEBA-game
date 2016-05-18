using System.Collections.Generic;

public class EventLibrary : Singleton<EventLibrary> {

    // eat
    // play
    // work
    // sleep
    // flee
    // chase

    public Dictionary<string, float> ActionDramas = new Dictionary<string, float>();

	// Use this for initialization
	void Awake () {
        ActionDramas.Add("eat", 0.3f);
        ActionDramas.Add("play", 0.005f);
        ActionDramas.Add("work", 0.005f);
        ActionDramas.Add("sleep", 0.001f);
        ActionDramas.Add("flee", 0.1f);
        ActionDramas.Add("chase", 0.1f);
    }
}