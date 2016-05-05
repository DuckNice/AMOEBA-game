using UnityEngine;
using System.Collections;

public class TutItemWithTrigger : TutorialItem
{
    [SerializeField]
    TutorialTrigger trigger;
    Vector3 startPos;
    [SerializeField]
    bool ContinueImmediate;

    protected override void OnEnable()
    {
        PlayerMotion.CanMove = false;

        BaseContinueTrigger += () => { PlayerMotion.CanMove = true; };

        if(trigger != null)
        {
            if (!ContinueImmediate)
            {
                trigger.TriggerForActive += () => { base.OnEnable(); GameManager.ToggleGameOn(true); };
            }
            else
            {
                trigger.TriggerForActive += () => { BaseContinueTrigger(); };
            }
        }
    }


    protected override void Update()
    {
        base.Update();
    }
}