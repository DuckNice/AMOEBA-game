using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

public class StoryPredicter : MonoBehaviour {
    public static void MainLoop()
    {
        while (!_shouldStop)
        {
            //Should only re-evaluate the story if we are detering significantly from the current structure.
            //Not gona call spinwait Thread.sleep bloacks immediately which is fine.
            //CHANGE: this was an if-statement for some reason.
            while(CloseEnough())
            {
                Thread.Sleep(1000);
            }

            StoryRecognizer.ShouldStartCompute = true;


            using (Mutex m = new Mutex(false, StoryRecognizer.FinishedComputeMutexName))
            {
                //CHANGE: Really wanted to use a spin-wait here, but not .net2
                while (true)
                {
                    if (m.WaitOne())
                    {
                        int storyStructure = StoryRecognizer.Closeststructure;

                        lock (CentralStoryManager._currentStory)
                        {
                            CentralStoryManager._currentStory = StructureLibrary.StoryStructures[storyStructure];
                        }

                        m.ReleaseMutex();
                    }
                    //CHANGE: If we are to incorporate multithreading here we should use a thread-pool.
                    //Relevant code ends here (This is part of the old system which will be improved after this exam).

                    /*List<StorySegment> bestStructures = new List<StorySegment>();

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
                        CentralStoryManager.newStory++;*/
                }
            }
        }
    }

    static bool _shouldStop = false;
    
    //Function requesting a stop of the system, which will allow the thread to finish by itself.
    public static void RequestStop()
    {
        _shouldStop = true;
    }
    

    //At a later point this will evaluate the new additions to the story going forward from the recognized story, and evaluate if it fits within a threshold.
    static bool CloseEnough()
    {
        return false;
    }

    
    ////////////////////////////////////////////////////IGNORE CONTENT BELOW//////////////////////////////////////////////////////
    

    //Prediction-part of the system. At the moment very basic and naive. Not part of the delivery.
    public static StructureContainer StructureCreator(List<StorySegment> structure, List<float> storySoFar)
    {
        float fitness = 0;

        List<StorySegment> go = new List<StorySegment>();

        int i = 0; 
        while (!_shouldStop && fitness < 0.8f && i < 10)
        {
            i++;

            int index = Random.Range(0, EventLibrary.EventTypes.Count - 1);

            if(EventLibrary.EventTypes.Values.ToList()[index] < structure[storySoFar.Count -1].ClimacticEffect - storySoFar[storySoFar.Count-1])
            {
                StorySegment newSegment = new StorySegment(structure[storySoFar.Count - 1].ClimacticEffect);
                newSegment.PreferenceStrength = 0.2f;

                //TODO make fallback or fix actions with big/small letters
                if (!GameManager.MoodyMask.PosActions.ContainsKey(EventLibrary.EventTypes.Keys.ToList()[index]))
                    continue;

                newSegment.action = GameManager.MoodyMask.PosActions[EventLibrary.EventTypes.Keys.ToList()[index]];
                fitness += EventLibrary.EventTypes.Values.ToList()[index];
                go.Add(newSegment);
            }
        }

        return new StructureContainer(go, fitness);
    }


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
}