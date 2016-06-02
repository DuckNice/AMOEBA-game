using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CentralStoryManager : MonoBehaviour {
    StructureLibrary _strucLib = new StructureLibrary();
    EventLibrary _eventLib = new EventLibrary();

    List<StorySegment> _currentStory = new List<StorySegment>();
    WaitForSeconds _selectionWaiter = new WaitForSeconds(5);


    void Start () {
        StartCoroutine(StorySelector());
    }
    
    
    protected IEnumerator StorySelector()
    {
        while (true)
        {
            try
            {
                _currentStory = StoryRecognizer.PredictClosestStructure(GameManager.MoodyMask.GetAllPeople(), _strucLib);

                Being.actionPreferenceModifiers.Clear();

                foreach (StorySegment segment in _currentStory)
                {
                    if (!Being.actionPreferenceModifiers.ContainsKey(segment.action))
                        Being.actionPreferenceModifiers.Add(segment.action, segment.PreferenceStrength);
                    else
                        Being.actionPreferenceModifiers[segment.action] += segment.PreferenceStrength;
                }
            }
            catch { }

            yield return _selectionWaiter;
        }
    }
}