using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NMoodyMaskSystem;

public class CentralStoryManager : MonoBehaviour {
    StructureLibrary strucLib = new StructureLibrary();

    List<StorySegment> currentStory = new List<StorySegment>();
    WaitForSeconds selectionWaiter = new WaitForSeconds(5);


    void Start () {
        
        StartCoroutine(StorySelector());
    }

	
	void Update () {
	    
	}


    protected IEnumerator StorySelector()
    {
        currentStory = StoryRecognizer.PredictClosestStructure(GameManager.MoodyMask.GetAllPeople(), strucLib);

        yield return selectionWaiter; 
    }
}