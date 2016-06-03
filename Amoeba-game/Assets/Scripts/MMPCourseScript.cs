using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using NMoodyMaskSystem;

public class MMPCourseScript : MonoBehaviour {

    MoodyMaskSystem _moodyMask;

    void Start () {
        _moodyMask = GameManager.MoodyMask;

        CreateActions(10000, Random.Range(1, 10), 0);
        CreateStructures(10000, Random.Range(1, 10), 0);
    }
	

	void Update () {
        Debug.Log("Number of possible actions: " + _moodyMask.PosActions.Count + ", number of action specifiers: " + EventLibrary.ActionDramas.Count + ".");

        if(_moodyMask.HistoryBook.Count < 1000000)
        {
            for(int i = 0; i < 10; i++)
            {
                _moodyMask.DidAction(_moodyMask.PosActions.Values.ToList()[Random.Range(0, _moodyMask.PosActions.Count)], Person.Empty, Person.Empty, Rule.Empty);
            }
        }
	}


    void CreateStructures(int iterations, int skips, int number)
    {
        List<StorySegment> structure = new List<StorySegment>();

        int length = Random.Range(5, 20);

        while(length > 0)
        {
            structure.Add(new StorySegment(Random.Range(0f, 1f)));

            length--;
        }

        StructureLibrary.StoryStructures.Add(structure);

        if (iterations > 0)
        {
            iterations -= skips;
            CreateStructures(iterations, skips, number);
        }
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