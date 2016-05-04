using UnityEngine;
using System.Collections;

public class TutItemMoving : TutorialItem
{
    bool StartedMoving;
    bool SetButton;

    Vector3 startPos;
    [SerializeField]
    GameObject player;
    protected new void OnEnable()
    {
        GameManager.ToggleGameOn (true);
        startPos = player.transform.position;
    }


    protected override void Update()
    {
        if (!StartedMoving && startPos != player.transform.position)
        {
            StartedMoving = true;
        }

        if (StartedMoving && !SetButton)
        {
            base.OnEnable();

            SetButton = true;
        }

        base.Update();
    }
}