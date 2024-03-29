using UnityEngine;
using System.Collections.Generic;

public class Opinion : MonoBehaviour {
    GameObject emotion;
    GameObject myObject;
    string _otherCharacterName;
    protected LineRenderer line;
    NMoodyMaskSystem.Person me;
    GameObject _trait1;
    GameObject _trait2;
    GameObject _trait3;
    public bool KeepOn = false;



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
        myC.CreateOpinionLine();

        myC.Update();

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

                DrawOpinion();
                _opinionNotFound = false;
            }
            else
            {
#if ALL_DEBUG_MODE || AMOEBA_DEBUG_MODE
                Debug.LogWarning("Warning: " + me.Name.Trim().ToLower() + "'s opinion towards " + _otherCharacterName.Trim().ToLower() + " was not found. Hiding opinion.");
#endif
                HideOpinion();
                _opinionNotFound = true;
            }
        }
    }

    bool _opinionNotFound = false;

    public void HideOpinion()
    {
        if (!KeepOn)
        {
            line.gameObject.SetActive(false);
            _trait1.gameObject.SetActive(false);
            _trait2.gameObject.SetActive(false);
            _trait3.gameObject.SetActive(false);
            _opinionActive = false;
        }
    }

    bool _opinionActive = false;
    public bool OpinionActive { get { return _opinionActive; } }

    public void ShowOpinion()
    {
        Update();
        if (!_opinionNotFound)
        {
            line.gameObject.SetActive(true);
            _trait1.gameObject.SetActive(true);
            _trait2.gameObject.SetActive(true);
            _trait3.gameObject.SetActive(true);
            _opinionActive = true;
        }
    }


    public void UpdateOpinion(float nicNas, float honFal, float chaGre)
    {
        _nicNas = (nicNas != -1) ? (nicNas + 1) / 2 : 0;
        _chaGre = (chaGre != -1) ? (chaGre + 1) / 2 : 0;
        _honFal = (honFal != -1) ? (honFal + 1) / 2 : 0;
    }

    float _nicNas = 0;
    float _chaGre;
    float _honFal;

    void CreateOpinionLine()
    {
        line = gameObject.AddComponent<LineRenderer>();
        line.SetColors(Color.black, Color.black);
        line.SetWidth(0.1f, 0.1f);
        line.material = GameManager.Instance.ConnectionMaterial;
        

        _trait1 = Instantiate(GameManager.Instance.Spawnable);
        _trait1.name = "OpTTrait1";
        _trait1.transform.parent = transform;
        _trait1.transform.localScale = new Vector3(GameManager.OpinionScale, GameManager.OpinionScale, GameManager.OpinionScale);
        SpriteRenderer rend = _trait1.GetComponent<SpriteRenderer>();
        rend.sprite = GameManager.OpinionRepresentation;

        _trait2 = Instantiate(GameManager.Instance.Spawnable);
        _trait2.name = "OpTTrait2";
        _trait2.transform.parent = transform;
        _trait2.transform.localScale = new Vector3(GameManager.OpinionScale, GameManager.OpinionScale, GameManager.OpinionScale);
        rend = _trait2.GetComponent<SpriteRenderer>();
        rend.sprite = GameManager.OpinionRepresentation;

        _trait3 = Instantiate(GameManager.Instance.Spawnable);
        _trait3.name = "OpToTrait3";
        _trait3.transform.parent = transform;
        _trait3.transform.localScale = new Vector3(GameManager.OpinionScale, GameManager.OpinionScale, GameManager.OpinionScale);
        rend = _trait3.GetComponent<SpriteRenderer>();
        rend.sprite = GameManager.OpinionRepresentation;

        DrawOpinion();
    }


    void DrawOpinion()
    {
        if (_opinionActive)
        {
            line.gameObject.SetActive(true);
            _trait1.gameObject.SetActive(true);
            _trait2.gameObject.SetActive(true);
            _trait3.gameObject.SetActive(true);


            Vector3[] poss = new Vector3[2];
            //TODO: make public
            float opinionOrbDistances = 2;
            poss[0] = myObject.transform.position;
            poss[0].z = -7;
            poss[1] = emotion.transform.position;
            poss[1].z = -7;

            Vector3 lineVector = emotion.transform.position - myObject.transform.position;

            float trait2FromStart = lineVector.magnitude / 3;
            float trait1FromStart = trait2FromStart - opinionOrbDistances;
            float trait3FromStart = trait2FromStart + opinionOrbDistances;
            Vector3 unitDirection = lineVector.normalized;

            //TODO: Calculate this instead of just hiding
            poss[0] += unitDirection * 4f;

            line.SetPositions(poss);

            _trait1.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (-Mathf.Rad2Deg * Mathf.Atan2(unitDirection.x, unitDirection.y)) - 90));
            _trait1.transform.position = myObject.transform.position + (unitDirection * trait1FromStart);
            _trait1.transform.position = new Vector3(_trait1.transform.position.x, _trait1.transform.position.y, -8);
            Color opColor = Color.Lerp(GameManager.LikeTrait, GameManager.DislikeTrait, _nicNas);
            opColor.a = 1;
            _trait1.GetComponent<SpriteRenderer>().color = opColor;
            _trait2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (-Mathf.Rad2Deg * Mathf.Atan2(unitDirection.x, unitDirection.y)) - 90));
            _trait2.transform.position = myObject.transform.position + (unitDirection * trait2FromStart);
            _trait2.transform.position = new Vector3(_trait2.transform.position.x, _trait2.transform.position.y, -8);
            opColor = Color.Lerp(GameManager.LikeTrait, GameManager.DislikeTrait, _chaGre);
            opColor.a = 1;
            _trait2.GetComponent<SpriteRenderer>().color = opColor;
            _trait3.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (-Mathf.Rad2Deg * Mathf.Atan2(unitDirection.x, unitDirection.y)) - 90));
            _trait3.transform.position = myObject.transform.position + (unitDirection * trait3FromStart);
            _trait3.transform.position = new Vector3(_trait3.transform.position.x, _trait3.transform.position.y, -8);
            opColor = Color.Lerp(GameManager.LikeTrait, GameManager.DislikeTrait, _honFal);
            opColor.a = 1;
            _trait3.GetComponent<SpriteRenderer>().color = opColor;
        }
    }
}