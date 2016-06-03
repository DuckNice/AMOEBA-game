using UnityEngine;
using System.Threading;
using System.Collections.Generic;
using NMoodyMaskSystem;

public class CentralStoryManager : MonoBehaviour {
    public static List<StorySegment> _currentStory = new List<StorySegment>();
    public static volatile int newStory;
    WaitForSeconds _selectionWaiter = new WaitForSeconds(5);
    Thread _predicterThread;
    Thread _selectorThread;


    void Start () {
        //Start the seperate threads to make sure that story selection and prediction is not cluttering the main thread..
        _predicterThread = new Thread(StoryPredicter.MainLoop);
        _selectorThread =  new Thread(StorySelector);
    }


    void Update()
    {
        if (Input.anyKey)
            Destroy(this);
    }


    void OnDestroy()
    {
        _shouldStop = true;
        StoryPredicter.RequestStop();
        _selectorThread.Join();
        _predicterThread.Join();
    }


    static Dictionary<MAction, float> _actionPreferenceModifiers0 = new Dictionary<MAction, float>();
    static Dictionary<MAction, float> _actionPreferenceModifiers1 = new Dictionary<MAction, float>();
    static volatile int curModifier = -1;
    bool _shouldStop = false;

    //The main output from our program. The returned structure is a copy of the currently valid Dictionary
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

            lock(_currentStory)
            {
                try
                {
                    Being.actionPreferenceModifiers.Clear();

                    foreach (StorySegment segment in _currentStory)
                    {
                        if (!_actionPreferenceModifiers0.ContainsKey(segment.action))
                            _actionPreferenceModifiers0.Add(segment.action, segment.PreferenceStrength);
                        else
                            _actionPreferenceModifiers0[segment.action] += segment.PreferenceStrength;
                    }
                }
                catch { }
            }

            //
            Dictionary<MAction, float> modifiers = (curModifier != 0) ? _actionPreferenceModifiers0 : _actionPreferenceModifiers1;
            modifiers.Clear();

            

            //Change to new ModifierList and update which version is used.
            curStory = newStory;
            
            curModifier = (curModifier != 0) ? 0 : 1;
            
        }
    }
}