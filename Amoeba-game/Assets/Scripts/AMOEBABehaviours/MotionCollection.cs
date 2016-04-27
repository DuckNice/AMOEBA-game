using UnityEngine;
using System.Collections;



public class MotionCollection : MonoBehaviour {
    public enum BehaviourTypes
    {
        aristocrat,
        brute,
        girl, 
        king,
    }


    public static IEnumerator Hit(BehaviourTypes personType, MotionManager person, MotionManager otherPerson)
    {
        bool breakSigned = false;

        person.TouchedEvent += () => {
            breakSigned = true;
            otherPerson.Pushed(person);
            person.SetVelocity(Vector3.zero, 0);
        };
        person.AddVelocity((otherPerson.transform.position - person.transform.position).normalized, 20f);

        while(true)
        {
            if (breakSigned)
                break;

            person.Move((otherPerson.transform.position - person.transform.position).normalized, 10f);

            yield return new WaitForEndOfFrame();
        }
    }
    

    public static IEnumerator Walk(BehaviourTypes personType, MotionManager person, Vector2 destination)
    {
        float Speed = 2.0f;

        person.SetVelocity ((destination - new Vector2(person.transform.position.x, person.transform.position.y)).normalized, Speed);

        while((new Vector2(person.transform.position.x, person.transform.position.y) - destination).magnitude > 0.1f)
        {
            person.SetVelocity((destination - new Vector2(person.transform.position.x, person.transform.position.y)).normalized, Speed);

            yield return new WaitForEndOfFrame();
        }

        person.SetVelocity(Vector2.zero, 0);
    }


    public static IEnumerator Run(BehaviourTypes personType, MotionManager person, Vector2 destination)
    {
        float Speed = 15.0f;

        person.SetVelocity((destination - new Vector2(person.transform.position.x, person.transform.position.y)).normalized, Speed);

        while ((new Vector2(person.transform.position.x, person.transform.position.y) - destination).magnitude > 0.3f)
        {
            person.SetVelocity((destination - new Vector2(person.transform.position.x, person.transform.position.y)).normalized, Speed);

            yield return new WaitForEndOfFrame();
        }

        person.SetVelocity(Vector2.zero, 0);
    }


    public static IEnumerator Stumble(BehaviourTypes personType, MotionManager person, Vector3 direction, float Speed)
    {
        
        person.AddVelocity(direction, Speed);

        yield break;
    }


    public static IEnumerator Threaten(BehaviourTypes personType, MotionManager person, MotionManager target)
    {
        float currentScale = 1;

        while (currentScale < 1.1f)
        {
            person.ChangeScale(currentScale);
            target.ChangeScale(2 - currentScale);

            currentScale += 1 * (Time.fixedDeltaTime);

            Vector3 direction = target.transform.position - person.transform.position;

            person.SetVelocity(direction.normalized, 20f);

            yield return new WaitForFixedUpdate();
        }

        person.SetVelocity(Vector3.zero, 0);

        for (int i = 0; i < 10; i++)
        {
            Vector3 direction = target.transform.position - person.transform.position;

            target.SetVelocity(direction.normalized, 14f);

            yield return new WaitForFixedUpdate();
        }

        target.SetVelocity(Vector3.zero, 0);

        yield return new WaitForSeconds(0.3f);

        while (currentScale < 1.2f)
        {
            person.ChangeScale(currentScale);
            target.ChangeScale(2 - currentScale);

            currentScale += 1 * (Time.fixedDeltaTime);

            Vector3 direction = target.transform.position - person.transform.position;

            person.SetVelocity(direction.normalized, 20f);
            target.SetVelocity(direction.normalized, 14f);

            yield return new WaitForFixedUpdate();
        }

        person.SetVelocity(Vector3.zero, 0);
        target.SetVelocity(Vector3.zero, 0);

        yield return new WaitForSeconds(0.2f);


        while (currentScale < 1.3f)
        {
            person.ChangeScale(currentScale);
            target.ChangeScale(2 - currentScale);

            currentScale += 1 * (Time.fixedDeltaTime);

            Vector3 direction = target.transform.position - person.transform.position;

            person.SetVelocity(direction.normalized, 20f);
            target.SetVelocity(direction.normalized, 12f);

            yield return new WaitForFixedUpdate();
        }

        person.SetVelocity(Vector3.zero, 0);
        target.SetVelocity(Vector3.zero, 0);
    }


    public static IEnumerator Console(BehaviourTypes personType, MotionManager person, MotionManager target)
    {
        float currentScale = 1.3f;

        while (currentScale > 1.0f)
        {
            person.ChangeScale(currentScale);
            currentScale -= 1 * (Time.fixedDeltaTime / 3);
            target.ChangeScale(0.7f);


            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        float Speed = 1.0f;
        Vector2 destination = new Vector2(1.8f, 0.7976f);

        person.SetVelocity((destination - new Vector2(person.transform.position.x, person.transform.position.y)).normalized, Speed);

        while ((new Vector2(person.transform.position.x, person.transform.position.y) - destination).magnitude > 0.1f)
        {
            person.SetVelocity((destination - new Vector2(person.transform.position.x, person.transform.position.y)).normalized, Speed);

            yield return new WaitForEndOfFrame();
        }

        person.SetVelocity(Vector2.zero, 0);

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < 5; i++)
        {
            Vector3 direction = target.transform.position - person.transform.position;

            person.SetVelocity(direction.normalized, 7f);

            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 5; i++)
        {
            Vector3 direction = target.transform.position - person.transform.position;

            person.SetVelocity(-direction.normalized, 7f);

            yield return new WaitForFixedUpdate();
        }

        person.SetVelocity(Vector2.zero, 0);

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 5; i++)
        {
            Vector3 direction = target.transform.position - person.transform.position;

            person.SetVelocity(direction.normalized, 7f);

            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 5; i++)
        {
            Vector3 direction = target.transform.position - person.transform.position;

            person.SetVelocity(-direction.normalized, 7f);

            yield return new WaitForFixedUpdate();
        }

        person.SetVelocity(Vector2.zero, 0);
        yield return new WaitForSeconds(0.7f);

        currentScale = 0.7f;

        while (currentScale < 1.0f)
        {
            target.ChangeScale(currentScale);
            currentScale += 1 * (Time.fixedDeltaTime / 3);


            yield return new WaitForFixedUpdate();
        }
    }


    public static IEnumerator Kiss(BehaviourTypes personType, MotionManager person, MotionManager target)
    {
        for (int i = 0; i < 60; i++)
        {
            Vector3 direction = target.transform.position - person.transform.position;

            person.SetVelocity(direction.normalized, 0.5f);
            target.SetVelocity(-direction.normalized, 0.5f);

            yield return new WaitForFixedUpdate();
        }

    }


    ////////////////////////////FOR LATER///////////////////////////77
    public static void Drag()
    {

    }


    public static void Dance()
    {

    }


    public static void Talk()
    {

    }
    

    public static void TurnAway()
    {

    }


    public static void Agree()
    {

    }


    public static void DisAgree()
    {

    }


    public static void Play()
    {

    }


    public static void Sleep()
    {

    }


    public static void Brood()
    {

    }


    public static void Open()
    {

    }


    public static void Close()
    {

    }
}