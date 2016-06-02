using UnityEngine;
using NMoodyMaskSystem;

public class MMPCourseScript : MonoBehaviour {

    MoodyMaskSystem _moodyMask;

    void Awake () {
        _moodyMask = GameManager.MoodyMask;

        CreateActions(10000, Random.Range(1, 100), 0);
    }
	
	void Update () {
        Debug.Log("Number of possible actions: " + _moodyMask.PosActions.Count + ", number of action specifiers: " + EventLibrary.ActionDramas.Count + ".");
	}

    
    void CreateActions(int iterations, int skips, int number)
    {
        ActionInvoker action = (text, subject, direct, indPpl, misc) => { };

        GameManager.MoodyMask.AddAction(new MAction("Action" + number, GameManager.MoodyMask, action, 7f));

        EventLibrary.AddEvent("Action" + number++, Random.Range(0f, 1f));

        if (iterations > 0)
        {
            iterations -= skips;
            CreateActions(iterations, skips, number);
        }
    }
}