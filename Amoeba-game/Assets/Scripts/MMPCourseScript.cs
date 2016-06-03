using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using NMoodyMaskSystem;

/// <summary>
/// Proof-of-Concept class creating the basic structures needed in the story-structure and event libraries, as well as adding to the history-list
/// </summary>
public class MMPCourseScript : MonoBehaviour {

    MoodyMaskSystem _moodyMask;

    //Initialize all the stuff here.
    void Awake () {
        _moodyMask = GameManager.MoodyMask;

        CreateActions(10000, Random.Range(1, 10), 0);
        CreateStructures(1024, 1, 0);
    }
	

	void Update () {
        //Make up to a million historyitems for he history-book. Add more as we go.
        if(_moodyMask.HistoryBook.Count < 1000000)
        {
            for(int i = 0; i < 10; i++)
            {
                //Since this is a dummy, we are only concerned with the action, not anything else.
                _moodyMask.DidAction(_moodyMask.PosActions.Values.ToList()[Random.Range(0, _moodyMask.PosActions.Count)], Person.Empty, Person.Empty, Rule.Empty);
            }
        }
	}

    //Create 1024 structures of random length (incorporated in a recursion-algorithm for the heck of it).
    void CreateStructures(int iterations, int skips, int number)
    {
        List<StorySegment> structure = new List<StorySegment>();

        int length = Random.Range(5, 20);

        //Add some amount of elements to the structure.
        while (length > 0)
        {
            structure.Add(new StorySegment(Random.Range(0f, 1f)));

            length--;
        }

        StructureLibrary.AddStructureLibrary(structure);

        //Recursive element.
        if (iterations > 0)
        {
            iterations -= skips;
            CreateStructures(iterations, skips, number);
        }
    }


    //Create a list of events (actions) of random strength same recursive structure as the structure creator.
    void CreateActions(int iterations, int skips, int number)
    {
        ActionInvoker action = (text, subject, direct, indPpl, misc) => { };

        GameManager.MoodyMask.AddAction(new MAction("Action" + number, GameManager.MoodyMask, action, 7f));

        EventLibrary.AddEvent("Action" + number++, Random.Range(0f, 1f));

        //Recursive element.
        if (iterations > 0)
        {
            iterations -= skips;
            CreateActions(iterations, skips, number);
        }
    }
}