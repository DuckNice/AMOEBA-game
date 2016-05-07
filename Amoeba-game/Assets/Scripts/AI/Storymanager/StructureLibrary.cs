using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct StorySegment
{
    float climaxicEffect;
};

public class StructureLibrary : MonoBehaviour {
    public List<List<StorySegment>> StoryStructures = new List<List<StorySegment>>();
    

	// Use this for initialization
	void Awake () {



        //Freytag's model

        List<StorySegment> freytag = new List<StorySegment>();


        StoryStructures.Add(freytag);


        //Tri-punct model
        List<StorySegment> triPunct = new List<StorySegment>();


        StoryStructures.Add(triPunct);

    }
}
