using UnityEngine;
using NMoodyMaskSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIManager : MonoBehaviour {
    public MoodyMaskSystem MoodyMask { get; private set; }
    public List<Being> allBeings = new List<Being>();

    void Awake()
    {
        MoodyMask = new MoodyMaskSystem();
        List<GameObject> characters = GameObject.FindGameObjectsWithTag("Character").ToList();
        
        foreach(GameObject character in characters)
        {
            allBeings.Add(character.GetComponentInChildren<Being>());
        }

    }


    void Start()
    {
        StartCoroutine(NPCUpdater());
    }

    List<Being> ScheduledForNextEvaluation = new List<Being>();


    IEnumerator NPCUpdater()
    {
        WaitForFixedUpdate waiter = new WaitForFixedUpdate();

        while (true)
        {
            foreach (Being being in allBeings)
            {
                if(MoodyMask.GetUpdateList("main") != null && MoodyMask.GetUpdateList("Main").FindIndex(x => x.Name == being.name.ToLower().Trim()) >= 0)
                {
                    being.NPCAction(GameManager.Time, ScheduledForNextEvaluation.Contains(being));
                }

                if (ScheduledForNextEvaluation.Contains(being))
                {
                    ScheduledForNextEvaluation.Remove(being);
                }
            }

            yield return waiter;
        }
    }
}