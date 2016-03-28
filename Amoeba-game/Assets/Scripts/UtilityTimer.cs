using UnityEngine;
using System.Collections;
using System.Threading;

public class UtilityTimer : MonoBehaviour {

    public delegate void TimerFunction();
    TimerFunction timerFunctionality;

    float _secondsBetweenTicks;
    public float SecondsBetweenTicks {
        get { return _secondsBetweenTicks; } set { _secondsBetweenTicks = (value > 0.01f) ? value : 0.01f ; }
    }

    int multiThreaded = -1;
    Thread _timerThread;
    private volatile bool _shouldTerminate = false;
    float _dT = 0;


    public static UtilityTimer CreateUtilityTimer(GameObject utilityHolder, float tickPaceInSeconds, TimerFunction delegateToExecute, bool useSeperateThread = false)
    {
        if(utilityHolder != null && delegateToExecute != null)
        {
            UtilityTimer timer = utilityHolder.AddComponent<UtilityTimer>();
            timer._secondsBetweenTicks = tickPaceInSeconds;
            timer.timerFunctionality = delegateToExecute;
            timer.multiThreaded = (useSeperateThread) ? 1 : 0;

            return timer;
        }
        else
        {
#if ALL_DEBUG_MODE || MULTITHREAD_DEBUG
            Debug.LogError("Error: UtilityTimer got Insufficient input parameters. Not making Timer.");
#endif
        }

        return null;
    }


    void Start()
    {
        if(!(_secondsBetweenTicks > 0) || timerFunctionality == null || multiThreaded < 0)
        {
#if ALL_DEBUG_MODE || MULTITHREAD_DEBUG
            Debug.LogError("Error: UtilityTimer is missing critical components for startup. Please use custom constructor.");
#endif
            Destroy(this);
        }
        else if(multiThreaded == 1)
        {
            _timerThread = new Thread(MultiThreadedUpdate);
            _timerThread.SetApartmentState(ApartmentState.STA);

            _timerThread.Start();
        }
    }


    void Update()
    {
        if(multiThreaded == 0 && _dT > _secondsBetweenTicks)
        {
            timerFunctionality();

            _dT = 0;
        }

        _dT += Time.deltaTime;
    }


    void MultiThreadedUpdate()
    {
        while(!_shouldTerminate)
        {
            timerFunctionality();
            Thread.Sleep((int)(_secondsBetweenTicks * 1000));
        }
    }


    void OnDestroy()
    {
        if (_timerThread != null)
        {
            _shouldTerminate = true;
            _timerThread.Join(1000);
        }
    }
}
