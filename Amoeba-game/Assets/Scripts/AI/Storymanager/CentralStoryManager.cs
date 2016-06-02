using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CentralStoryManager : MonoBehaviour {
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
                _currentStory = StoryPredicter.Get5BestStructures();

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