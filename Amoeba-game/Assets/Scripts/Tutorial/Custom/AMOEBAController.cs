using UnityEngine;
using System.Collections;

public class AMOEBAController : TutorialTrigger {
    public Vector3 InitialEntryToPosition = new Vector3(4.2f, -0.5f, 0);
    public float Speed = 5f;

	public void StartEntryMovement()
    {
        StartCoroutine(MoveToEntryPosition());       

    }

    IEnumerator MoveToEntryPosition()
    {
        WaitForEndOfFrame go = new WaitForEndOfFrame();

        Vector3 direction = InitialEntryToPosition - transform.position;

        while (Vector3.Distance(transform.position, InitialEntryToPosition) > 0.05f)
        {
            transform.Translate(direction.normalized * Time.deltaTime * Speed);
            yield return go;
        }

        transform.position = InitialEntryToPosition;
        ActivateTrigger();
    }
}