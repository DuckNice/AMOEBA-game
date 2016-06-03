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
    public static List<float> PredictClosestStructure(List<Person> peopleToAccountFor)
    {
        //TODO: this makes time static, make a dynamic way to do that.
        int timeForEachSegment = 120 / structure.Count;

        List<List<HistoryItem>> segmentedItems = new List<List<HistoryItem>>();

        int q = 0;

        for (int i = 0; i < structure.Count; i++)
        {
            segmentedItems.Add(new List<HistoryItem>());

            while (q < GameManager.MoodyMask.HistoryBook.Count && GameManager.MoodyMask.HistoryBook[q].GetTime() < timeForEachSegment * i)
            {
                segmentedItems[i].Add(GameManager.MoodyMask.HistoryBook[q]);

                q++;
            }

            if (q >= GameManager.MoodyMask.HistoryBook.Count)
                break;
        }

        List<float> storySoFar = new List<float>();

        for (int i = 0; i < segmentedItems.Count; i++)
        {
            float go = 0f;

            foreach (HistoryItem histItem in segmentedItems[i])
            {
                //TODO make fallback or fix actions with big/small letters
                if (EventLibrary.ActionDramas.ContainsKey(histItem.GetAction().Name))
                    go += EventLibrary.ActionDramas[histItem.GetAction().Name];
            }

            storySoFar.Add(go);
        }

        return storySoFar;
    }
}