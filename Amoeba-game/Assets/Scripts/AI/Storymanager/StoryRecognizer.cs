using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NMoodyMaskSystem;

public struct StructureContainer
{
    public List<StorySegment> Structure { get; private set; }
    public float Fitness { get; private set; }

    public StructureContainer(List<StorySegment> structure, float fitness)
    {
        Structure = structure;
        Fitness = fitness;
    }
}

public class StoryRecognizer : MonoBehaviour {
    public static List<StorySegment> PredictClosestStructure(List<Person> peopleToAccountFor, StructureLibrary structureLibrary)
    {
        List<StructureContainer> go = new List<StructureContainer>();

        foreach (List<StorySegment> structure in structureLibrary.StoryStructures)
        {
            go.AddRange(StoryPredicter.Get5BestStructures(structure));
        }


        StructureContainer bestContainer = default(StructureContainer);

        foreach (StructureContainer strucCont in go)
        {
            if (!bestContainer.Equals(default(StructureContainer)))
                bestContainer = (bestContainer.Fitness > strucCont.Fitness) ? bestContainer : strucCont;
            else
                bestContainer = strucCont;
        }


        return bestContainer.Structure;
    }
}