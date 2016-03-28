using UnityEngine;
using System.Collections;
using System.Threading;

public class UtilityTimer : MonoBehaviour {

    delegate void TimerFunction();
    TimerFunction timerFunctionality;

    float secondsBetweenTicks;
    int multiThreaded;
    Thread _timerThread;
    private volatile bool _shouldTerminate = false;

    if()
    {

    }

    void Awake()
    {
        if(!(secondsBetweenTicks > 0) || timerFunctionality == null || multiThreaded < 0)
        {
            Debug.LogError("Error: UtilityTimer is missing critical components for startup. Please use custom constructor.");

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
        if(multiThreaded == 0)
        {

        }
    }


    void MultiThreadedUpdate()
    {
        while(!_shouldTerminate)
        {

        }
    }


    void OnDestroy()
    {
        _shouldTerminate = true;
        _timerThread.Join(1000);
    }
}
