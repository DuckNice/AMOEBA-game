using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using NMoodyMaskSystem;

public class CentralStoryManager : MonoBehaviour {
    public static List<StorySegment> _currentStory = new List<StorySegment>();
    public static volatile int newStory;
    Thread _predicterThread;
    Thread _selectorThread;


    void Start () {
        //Setup the compute shader and the static buffers.
        StoryRecognizer.SetupComputeShader();
        //Start the seperate threads to make sure that story selection and prediction is not cluttering the main thread..
        _predicterThread = new Thread(StoryPredicter.MainLoop);
        _selectorThread =  new Thread(StorySelector);
        //CHANGE: threads should terminate when main does.
        _predicterThread.IsBackground = true;
        _selectorThread.IsBackground = true;
        _selectorThread.Priority = System.Threading.ThreadPriority.BelowNormal;
    }


    void Update()
    {
        if (Input.anyKey)
            Destroy(this);
    }


    //Called when the Story manager is destroyed.
    void OnDestroy()
    {
        //Stop tthe threads and wait for them to finish (NOTE: Given that normal destructurs only have a set amount of time before they are brute-forced, this might also be the case OnDestroy. In that case there's a risk that the threads might not close in time). 
        _shouldStop = true;
        StoryPredicter.RequestStop();
        _selectorThread.Join();
        _predicterThread.Join();
    }


    static Dictionary<MAction, float> _actionPreferenceModifiers0 = new Dictionary<MAction, float>();
    static Dictionary<MAction, float> _actionPreferenceModifiers1 = new Dictionary<MAction, float>();
    static volatile int curModifier = -1;
    bool _shouldStop = false;

    //The main output to the rest of the world from our program. The returned structure is a copy of the currently valid Dictionary
    public Dictionary<MAction, float> GetActionModifiers()
    {
        if(curModifier == 0)
        {
            lock (_actionPreferenceModifiers0)
            {
                return new Dictionary<MAction, float>(_actionPreferenceModifiers0);
            }
        }
        else
        {
            lock (_actionPreferenceModifiers1)
            {
                return new Dictionary<MAction, float>(_actionPreferenceModifiers1);
            }
        }
    }


    protected void StorySelector()
    {
        int curStory = 0;

        while (!_shouldStop)
        {
            while (curStory == newStory)
            {
                if (_predicterThread.IsAlive) { }
                else
                { _shouldStop = true; }

                Thread.Sleep(10);
            }

            //I did not incorporate the 2-alternating array system here. Since this system want the newest data, and would need to re-iterate on the new data anyway, I chose to let this function finish 
            //before the new structure could be inserted. This will give the rest of the outside system enough time to use this data for anything, before it's given new stuff again.
            //Future: Have this function break what it's doing, and wait for the newer data to get written in a seperate array, if this program notices that new data is being written.
            //NOTE: Since The predicter system is not finished, no data is ever passed through here.
            Dictionary<MAction, float> modifiers = (curModifier != 0) ? _actionPreferenceModifiers0 : _actionPreferenceModifiers1;
            modifiers.Clear();

            lock (_currentStory)
            {
                try
                {
                    modifiers.Clear();

                    foreach (StorySegment segment in _currentStory)
                    {
                        if (!modifiers.ContainsKey(segment.action))
                            modifiers.Add(segment.action, segment.PreferenceStrength);
                        else
                            modifiers[segment.action] += segment.PreferenceStrength;
                    }
                }
                catch { }
            }

            //Change to new ModifierList and update which version is used.
            curStory = newStory;
            
            curModifier = (curModifier != 0) ? 0 : 1;
            
        }
    }
}