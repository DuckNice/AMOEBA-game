using UnityEngine;
using System.Collections;
using System.Threading;

public class UtilityTimer : MonoBehaviour {

    public delegate void TimerFunction();
    TimerFunction _timerFunctionality;

    float _secondsBetweenTicks = 1;
    int _ticksAfterWhichTheTimerEnds = -1;
    int _currentTick = 0;
    bool _timerRunning = true;
    public float SecondsBetweenTicks {
        get { return _secondsBetweenTicks; } set { _secondsBetweenTicks = (value > 0.01f) ? value : 0.01f ; }
    }

    int _multiThreaded = -1;
    Thread _timerThread;
    private volatile bool _shouldTerminate = false;
    float _dT = 0;


    public static UtilityTimer CreateUtilityTimer(GameObject utilityHolder, TimerFunction delegateToExecute, float tickPaceInSeconds = 1, int tickAmountForFiniteDuration = -1, bool useSeperateThread = false)
    {
        if(utilityHolder != null && delegateToExecute != null)
        {
            UtilityTimer timer = utilityHolder.AddComponent<UtilityTimer>();
            timer._secondsBetweenTicks = tickPaceInSeconds;
            timer._timerFunctionality = delegateToExecute;
            timer._multiThreaded = (useSeperateThread) ? 1 : 0;
            timer._ticksAfterWhichTheTimerEnds = tickAmountForFiniteDuration;

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


    #region Timer interaction functionality
    public void PauseTimer()
    {
        _timerRunning = false;
    }


    public void StartTimer()
    {
        _timerRunning = true;
    }
    

    public void ResetTimer(bool stoppedAfterReset = true)
    {
        _timerRunning = false;
        _currentTick = 0;

        if (!stoppedAfterReset)
            StartTimer();
    }

    public int IsTimerRunning()
    {
        if(_timerRunning)
        {
            return 1;
        }
        else if(_currentTick >= _ticksAfterWhichTheTimerEnds)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }

    public void UpdateDelegate(TimerFunction delegateToExecute, int resetTimerWithTicks = -1)
    {
        if (delegateToExecute == null) { Debug.LogError("Error: delegate sent to timer contained null. Not updating."); return; }

        _timerFunctionality = delegateToExecute;

        if(resetTimerWithTicks > 0)
        {
            _ticksAfterWhichTheTimerEnds = resetTimerWithTicks;
            ResetTimer(false);
        }
    }
    #endregion


    #region Timer life functionality.
    void Start()
    {
        if(!(_secondsBetweenTicks > 0) || _timerFunctionality == null || _multiThreaded < 0)
        {
#if ALL_DEBUG_MODE || MULTITHREAD_DEBUG
            Debug.LogError("Error: UtilityTimer is missing critical components for startup. Please use custom constructor.");
#endif
            Destroy(this);
        }
        else if(_multiThreaded == 1)
        {
            _timerThread = new Thread(MultiThreadedUpdate);
            _timerThread.SetApartmentState(ApartmentState.STA);

            _timerThread.Start();
        }
    }


    void Update()
    {
        if (_timerRunning)
        {
            if(_currentTick >= _ticksAfterWhichTheTimerEnds && _ticksAfterWhichTheTimerEnds > 0)
            {
                _timerRunning = false;
                return;
            }


            if (_multiThreaded == 0 && _dT > _secondsBetweenTicks)
            {
                _timerFunctionality();

                _dT = 0;
                _currentTick++;
            }

            _dT += Time.deltaTime;
        }
    }


    void MultiThreadedUpdate()
    {
        while(!_shouldTerminate)
        {
            if (_timerRunning)
            {
                _timerFunctionality();
                Thread.Sleep((int)(_secondsBetweenTicks * 1000));
            }
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
    #endregion
}
