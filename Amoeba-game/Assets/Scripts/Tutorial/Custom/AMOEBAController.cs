using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using NMoodyMaskSystem;


public class AMOEBAController : TutorialTrigger {
    public Vector3 InitialEntryToPosition = new Vector3(4.2f, -0.5f, 0);
    public float Speed = 5f;
    Person person;
    public Slider hapSad;
    public Slider arousDisgus;
    public Slider angryFear;
    public Slider energTired;
    public Slider niceNasty;
    public Slider honFalse;
    public Slider charGreed;
    public AMOEBAManager myMan;
    public AMOEBAManager targetMan;
    public TutorialTrigger OpinionSelectedTrigger;
    Opinion OpinionToSupervise;

    public TutorialItem traitEmotionHighLight;

    public void Start()
    {
        //
        List<GameObject> go = new List<GameObject>();

        foreach(Transform tr in transform)
        {
            if(tr.gameObject.name == "Traits" || tr.gameObject.name == "Emotion")
            {
                go.Add(tr.gameObject);
            }
        }

        traitEmotionHighLight.ItemsToHighlight.AddRange(go);

        //TraitSliders
        person = GameManager.MoodyMask.GetPerson("Dummy");

        niceNasty.onValueChanged.AddListener((x) => { person.AbsTraits.Traits[TraitTypes.NiceNasty].SetTraitValue(x); });
        honFalse.onValueChanged.AddListener((x) => { person.AbsTraits.Traits[TraitTypes.HonestFalse].SetTraitValue(x); });
        charGreed.onValueChanged.AddListener((x) => { person.AbsTraits.Traits[TraitTypes.CharitableGreedy].SetTraitValue(x); });
        hapSad.onValueChanged.AddListener((x) => { person.Moods[MoodTypes.hapSad] = x; });
        arousDisgus.onValueChanged.AddListener((x) => { person.Moods[MoodTypes.arousDisgus] = x; });
        angryFear.onValueChanged.AddListener((x) => { person.Moods[MoodTypes.angryFear] = x; });
        energTired.onValueChanged.AddListener((x) => { person.Moods[MoodTypes.energTired] = x; });
    }


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


    public void CreateOpinion()
    {
        OpinionToSupervise = myMan.CreateConnection(targetMan);

        StartCoroutine(WaitForOpinionSelected());
    }

    IEnumerator WaitForOpinionSelected()
    {
        WaitForEndOfFrame frameWaiter = new WaitForEndOfFrame();

        while(true)
        {
            if(OpinionToSupervise.OpinionActive)
            {
                OpinionSelectedTrigger.ActivateTrigger();
                OpinionToSupervise.KeepOn = true;
                yield break;
            }

            yield return frameWaiter;
        }
    }
}