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
    public static List<List<StorySegment>> StoryStructures { get; private set; }
    public static int MaxLength = -1;

    public static void AddStructureLibrary(List<StorySegment> structure)
    {
        lock(StoryStructures)
        {
            if (StoryStructures == null)
                StoryStructures = new List<List<StorySegment>>();

            StoryStructures.Add(structure);

            //Make sure that we always have the data for the longest structure.
            MaxLength = (structure.Count > MaxLength) ? structure.Count : MaxLength;
        }
    }
}
