using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StoryPredicter : MonoBehaviour {
    public static void MainLoop()
    {
        while (!_shouldStop)
        {
            if(CloseEnough())
            {
                continue;
            }

            List<float> storySoFar = StoryRecognizer.PredictClosestStructure(GameManager.MoodyMask.GetAllPeople());

            List<StorySegment> bestStructures = new List<StorySegment>();

            StructureContainer bestContainer = default(StructureContainer);

            foreach (StructureContainer strucCont in go)
            {
                if (!bestContainer.Equals(default(StructureContainer)))
                    bestContainer = (bestContainer.Fitness > strucCont.Fitness) ? bestContainer : strucCont;
                else
                    bestContainer = strucCont;
            }


            for (int i = 0; i < 5; i++)
            {
                bestStructures.Add(StructureCreator(structure, storySoFar));
            }

            if(!_shouldStop)
                CentralStoryManager.newStory++;
        }
    }

    static bool _shouldStop = false;

    public static void RequestStop()
    {
        _shouldStop = true;
    }
    

    static bool CloseEnough()
    {
        return false;
    }


    public static StructureContainer StructureCreator(List<StorySegment> structure, List<float> storySoFar)
    {
        float fitness = 0;

        List<StorySegment> go = new List<StorySegment>();

        int i = 0; 
        while (!_shouldStop && fitness < 0.8f && i < 10)
        {
            i++;

            int index = Random.Range(0, EventLibrary.ActionDramas.Count - 1);

            if(EventLibrary.ActionDramas.Values.ToList()[index] < structure[storySoFar.Count -1].ClimacticEffect - storySoFar[storySoFar.Count-1])
            {
                StorySegment newSegment = new StorySegment(structure[storySoFar.Count - 1].ClimacticEffect);
                newSegment.PreferenceStrength = 0.2f;

                //TODO make fallback or fix actions with big/small letters
                if (!GameManager.MoodyMask.PosActions.ContainsKey(EventLibrary.ActionDramas.Keys.ToList()[index]))
                    continue;

                newSegment.action = GameManager.MoodyMask.PosActions[EventLibrary.ActionDramas.Keys.ToList()[index]];
                fitness += EventLibrary.ActionDramas.Values.ToList()[index];
                go.Add(newSegment);
            }
        }

        
        return new StructureContainer(go, fitness);
    }


    public static List<StorySegment> RecursiveStorySegment(int maxRecursionsLeft, ref float fitness)
    {
        return RecursiveStorySegment(maxRecursionsLeft - 1, ref fitness);
    }
}