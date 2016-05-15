using UnityEngine;
using System.Collections;

public class TutorialTrigger : MonoBehaviour {

    public delegate void Trigger();
    public event Trigger TriggerForActive;

    public void ActivateTrigger()
    {
        if(TriggerForActive != null)
            TriggerForActive();
    }
}
