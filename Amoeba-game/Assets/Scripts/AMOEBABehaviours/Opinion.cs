using UnityEngine;
using System.Collections.Generic;

public class Opinion : MonoBehaviour {
    SpriteRenderer _renderer;
    GameObject emotion;
    string _thisCharacterName;
    string _otherCharacterName;
    protected LineRenderer line;


    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }


    public static Opinion CreateComponent(GameObject thisObject, string thisCharName, string otherCharName, GameObject emotionObj)
    {
        Opinion myC = thisObject.AddComponent<Opinion>();

        myC.emotion = emotionObj;
        myC._thisCharacterName = thisCharName;
        myC._otherCharacterName = otherCharName;

        return myC;
    }


    void Update()
    {
        NMoodyMaskSystem.Person me = GameManager.MoodyMask.GetPerson(_thisCharacterName);

        if(me != null)
        {
            List<NMoodyMaskSystem.Opinion> opinions = me.Opinions.FindAll(x => x.Pers.Name == _otherCharacterName);

            if (opinions.Count == 3)
            {
                UpdateOpinion(opinions.Find(x => x.Trait == NMoodyMaskSystem.TraitTypes.NiceNasty).Value,
                    opinions.Find(x => x.Trait == NMoodyMaskSystem.TraitTypes.HonestFalse).Value,
                    opinions.Find(x => x.Trait == NMoodyMaskSystem.TraitTypes.CharitableGreedy).Value);
            }
        }
    }


    public void UpdateOpinion(float nicNas, float honFal, float chaGre)
    {
        nicNas = (nicNas != -1) ? (nicNas + 1) / 2 : 0;
        chaGre = (chaGre != -1) ? (chaGre + 1) / 2 : 0;
        honFal = (honFal != -1) ? (honFal + 1) / 2 : 0;

        

        _renderer.sprite =
        AnimationCalculator.CalculateSpriteBasedForFrame(GameManager.Instance.EmotionShapeSheet, 97, 97, (int)((GameManager.FramesInOpinionSheet) * honFal), 1, (sprite) => { Destroy(_renderer.sprite); _renderer.sprite = sprite; });

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
        poss[0] = transform.position;
        poss[1] = emotion.transform.position;

        line.SetPositions(poss);
    }
}