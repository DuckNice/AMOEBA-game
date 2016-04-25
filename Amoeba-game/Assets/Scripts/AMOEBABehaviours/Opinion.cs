using UnityEngine;
using System.Collections.Generic;

public class Opinion : MonoBehaviour {
    GameObject emotion;
    GameObject myObject;
    string _otherCharacterName;
    protected LineRenderer line;
    NMoodyMaskSystem.Person me;




    public static Opinion CreateComponent(GameObject thisObject, string thisCharName, string otherCharName, GameObject emotionObj)
    {
        NMoodyMaskSystem.Person me = GameManager.MoodyMask.GetPerson(thisCharName);

        if(me == null)
        {
            Debug.LogError("No person with the name: '" + thisCharName + "' was found. Not making opinion.");
        }

        GameObject traitObject = Instantiate(GameManager.Instance.Spawnable);
        traitObject.name = "Opinion towards " + otherCharName;
        traitObject.transform.parent = thisObject.transform;
        //TODO: Make this not be 1000000000 rigidbodies. Make it into formulas instead.

        Opinion myC = traitObject.AddComponent<Opinion>();
        myC.myObject = thisObject;
        myC.me = me;
        myC.emotion = emotionObj;
        myC._otherCharacterName = otherCharName;
        myC.CreateLineRenderer();

        return myC;
    }


    void Update()
    {
        if(me != null)
        {
            //TODO: hide opinion getting.
            List<NMoodyMaskSystem.Opinion> opinions = me.Opinions.FindAll(x => x.Pers.Name == _otherCharacterName.Trim().ToLower());

            if (opinions.Count == 3)
            {
                UpdateOpinion(opinions.Find(x => x.Trait == NMoodyMaskSystem.TraitTypes.NiceNasty).Value,
                    opinions.Find(x => x.Trait == NMoodyMaskSystem.TraitTypes.HonestFalse).Value,
                    opinions.Find(x => x.Trait == NMoodyMaskSystem.TraitTypes.CharitableGreedy).Value);

                DrawLine();
            }
        }
    }


    public void UpdateOpinion(float nicNas, float honFal, float chaGre)
    {
        nicNas = (nicNas != -1) ? (nicNas + 1) / 2 : 0;
        chaGre = (chaGre != -1) ? (chaGre + 1) / 2 : 0;
        honFal = (honFal != -1) ? (honFal + 1) / 2 : 0;
    }


    void CreateLineRenderer()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.SetColors(Color.black, Color.black);
        line.SetWidth(0.1f, 0.1f);
        line.material = GameManager.Instance.ConnectionMaterial;
    }


    void DrawLine()
    {
        Vector3[] poss = new Vector3[2];
        poss[0] = myObject.transform.position;
        poss[1] = emotion.transform.position;

        line.SetPositions(poss);
    }
}