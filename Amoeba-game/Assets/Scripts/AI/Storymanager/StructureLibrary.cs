using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public struct StorySegment
{
    public StorySegment(float climaxticEffect)
    {
        ClimacticEffect = climaxticEffect;
        action = default(NMoodyMaskSystem.MAction);
        PreferenceStrength = 0.0f;
    }

    public float ClimacticEffect { get; private set; }
    public NMoodyMaskSystem.MAction action;
    public float PreferenceStrength;
};

public class StructureLibrary {
    public static List<List<StorySegment>> StoryStructures = new List<List<StorySegment>>();
    

    public static void AddStructureLibrary(List<StorySegment> structure)
    {
        StoryStructures.Add(structure);
    }
}
