using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoryPredicter : MonoBehaviour {
    public static List<StructureContainer> Get5BestStructures(List<StorySegment> structure)
    {
        List<StructureContainer> bestStructures = new List<StructureContainer>();

        for(int i = 0; i < 5; i++)
        {
            bestStructures.Add(go(structure));
        }

        return bestStructures;
    }


    public static StructureContainer go(List<StorySegment> structure)
    {
        float fitness = 0;

        RecursiveStorySegment(10, ref fitness);
        
        
        return new StructureContainer(structure, fitness);
    }


    public static List<StorySegment> RecursiveStorySegment(int maxRecursionsLeft, ref float fitness)
    {


        return null;
    }
}