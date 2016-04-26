using UnityEngine;



public class MotionCollection : MonoBehaviour {
    public enum BehaviourTypes
    {
        aristocrat,
        brute,
        girl, 
        king,
    }


    public static void Hit(BehaviourTypes personType, MotionManager person, MotionManager otherPerson)
    {

    }



    public static void Walk(BehaviourTypes personType, MotionManager person, Vector3 destination)
    {

    }


    public static void Run(BehaviourTypes personType, MotionManager person, Vector3 destination)
    {

    }


    public static void Stumble(BehaviourTypes personType, MotionManager person, Vector3 direction, float Speed)
    {
        person.Move(direction, Speed);
    }


    public static void Threaten(BehaviourTypes personType, MotionManager person)
    {

    }


    public static void Console(BehaviourTypes personType, MotionManager person)
    {

    }


    public static void Kiss(BehaviourTypes personType, MotionManager person)
    {

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