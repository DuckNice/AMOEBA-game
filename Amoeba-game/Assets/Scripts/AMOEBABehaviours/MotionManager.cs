using UnityEngine;
using System.Collections;

public class MotionManager : MonoBehaviour {
    public delegate void  OnTouchedSomething();
    public event OnTouchedSomething TouchedEvent;
    [SerializeField]
    Rigidbody2D rigid;

    public void Move(Vector3 direction, float Speed)
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
        if(rigid == null)
        {
            Debug.LogError("No rigidbody attached to player. Aborting move.");
            return;
        }

        rigid.AddForce(new Vector2(direction.x, direction.y) * Speed);
    }


    public void AddVelocity(Vector3 direction, float Speed)
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
        if (rigid == null)
        {
            Debug.LogError("No rigidbody attached to player. Aborting move.");
            return;
        }

        rigid.velocity += new Vector2(direction.x, direction.y) * Speed;
    }


    public void SetVelocity(Vector3 direction, float Speed)
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
        if (rigid == null)
        {
            Debug.LogError("No rigidbody attached to player. Aborting move.");
            return;
        }

        rigid.velocity = new Vector2(direction.x, direction.y) * Speed;
    }


    public Vector2 GetVelocity()
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
        if (rigid == null)
        {
            Debug.LogError("No rigidbody attached to player. Aborting get vel.");
            return default(Vector2);
        }

        return rigid.velocity;
    }


    public void Pushed(MotionManager other)
    {
        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
        if (rigid == null)
        {
            Debug.LogError("No rigidbody attached to player. Aborting move.");
            return;
        }

        if (other.GetVelocity().magnitude > 7f)
        {
            StartCoroutine(MotionCollection.Stumble(MotionCollection.BehaviourTypes.brute, GetComponent<MotionManager>(), 
                transform.position - other.transform.position, 1f));
            return;
        }
    }


    public void ChangeScale(float scaleTo)
    {
        transform.localScale = new Vector3(scaleTo, scaleTo, scaleTo);
    }
    

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(TouchedEvent != null)
            TouchedEvent();

        if (rigid == null)
            rigid = GetComponent<Rigidbody2D>();
        if (rigid == null)
        {
            Debug.LogError("No rigidbody attached to player. Aborting move.");
            return;
        }

   /*     if(coll.GetComponent<Rigidbody2D>().velocity.magnitude > 7f)
        { 
            StartCoroutine(MotionCollection.Stumble(MotionCollection.BehaviourTypes.brute, GetComponent<MotionManager>(),
                coll.transform.position - transform.position, 5f));
            return;
        }*/
    }
}