using UnityEngine;
using System.Collections;

public class MotionManager : MonoBehaviour {
    public delegate void  OnTouchedSomething(Being personHit, Being personHitting, NMoodyMaskSystem.MAction action, Vector3 direction, float speed);
    public static event OnTouchedSomething TouchedEvent;

    public void Awake()
    {
        TouchedEvent += (personHit, personHitting, action, direction, speed) => { };
    }

    public void Move(Vector3 direction, float Speed)
    {
        
    }


    public void Pushed(Being personHit, Being personHitting, NMoodyMaskSystem.MAction action, Vector3 direction, float speed)
    {
        
    }


    void OnCollisionEnter2D(Collision2D coll)
    {
        MotionCollection.Stumble(MotionCollection.BehaviourTypes.brute, this.GetComponent<MotionManager>(), Vector3.one, 1);
    }
}