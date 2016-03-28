using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NMoodyMaskSystem;

public class EmotionGenerator : MonoBehaviour {
    public string CharacterName;
    [SerializeField]
    [Tooltip("The max distance distance to a target connections are shown at")]
    protected float _connectionDistance = 2;
    public float ConnectionDistance { get { return _connectionDistance; } protected set { _connectionDistance = value; } }
    [SerializeField]
    [Tooltip("For optimization: The max distance to a target ")]
    protected float _consideranceRadius = 10;

    protected static List<EmotionGenerator> _instances = new List<EmotionGenerator>();
    protected List<EmotionGenerator> _notableInstances = new List<EmotionGenerator>();
    protected Dictionary<EmotionGenerator, GameObject> _connectedInstances = new Dictionary<EmotionGenerator, GameObject>();

    protected static WaitForSeconds _waitforO5 = new WaitForSeconds(0.5f);
    protected List<GameObject> _opinions = new List<GameObject>();
    [SerializeField]
    protected GameObject _emotionPrefab;

    SpriteRenderer _renderer;
    void Awake()
    {
        _instances.Add(this);
    }

    void Start()
    {
        GameObject emotions = new GameObject();
        _renderer = emotions.AddComponent<SpriteRenderer>();
        emotions.transform.parent = transform;
        emotions.transform.localPosition = new Vector3(0, 0, 1);
        emotions.gameObject.name = "Emotion";

        StartCoroutine(UpdateEmotionsLoop());
        StartCoroutine(EmotionAnimator());
    }


    public void CreateConnection(EmotionGenerator other)
    {
        GameObject thisEmotion = Instantiate(_emotionPrefab);
        GameObject othersEmotion = Instantiate(_emotionPrefab);

        if(other.ConnectionRequest(this, othersEmotion))
        {
            thisEmotion.transform.parent = transform;
            thisEmotion.transform.localPosition = new Vector3(0, 0, 1);

            ConnectionManager.CreateComponent(thisEmotion, othersEmotion, this, other);

            thisEmotion.gameObject.name = "OpinionTowards" + other.gameObject.name;

            _connectedInstances.Add(other, thisEmotion);
        }
        else
        {
            Destroy(thisEmotion);
            Destroy(othersEmotion);
        }
    }


    public bool ConnectionRequest(EmotionGenerator other, GameObject thisEmotion)
    {
        if(!_connectedInstances.ContainsKey(other))
        {
            _connectedInstances.Add(other, thisEmotion);
            thisEmotion.transform.parent = transform;
            thisEmotion.transform.localPosition = new Vector3(0, 0, 1);
            thisEmotion.gameObject.name = "EmotionTowards" + other.gameObject.name;
            //TODO: Write a fallback method at which we check if the connection occupying the space is still intact.
            return true;
        }        

        return false;
    }


    public bool TerminateConnection(EmotionGenerator other)
    {
        if(other.TerminateConnectionRequest(this))
        {
            _connectedInstances.Remove(other);

            return true;
        }

        return false;
    }


    public bool TerminateConnectionRequest(EmotionGenerator other)
    {
        if(_connectedInstances.ContainsKey(other))
        {
            _connectedInstances.Remove(other);
        }

        return true;
    }


    public static void ForceUpdateNotableInstances()
    {
        foreach(EmotionGenerator generator in _instances)
        {
            generator.UpdateNotableInstances();
        }
    }


    private IEnumerator UpdateEmotionsLoop()
    {
        while(true)
        {
            UpdateNotableInstances();

            foreach (EmotionGenerator gen in _notableInstances)
            {
                if ((gen.transform.position - transform.position).magnitude < _connectionDistance && !(_connectedInstances.ContainsKey(gen)))
                {
                    CreateConnection(gen);
                }
            }

            Person person = GameManager.MoodyMask.GetPerson(CharacterName);
            if (person != null)
            {
                UpdateEmotion(person.Moods[MoodTypes.hapSad], person.Moods[MoodTypes.arousDisgus], person.Moods[MoodTypes.angryFear], person.Moods[MoodTypes.energTired]);
            }
            else
            {
                Debug.LogError("Error: No person with name: '" + CharacterName + "' in PersonContainer. not updating emotion.");
            }

            yield return _waitforO5;
        }
    }


    protected void UpdateNotableInstances()
    {
        foreach(EmotionGenerator gen in _instances)
        {
            _notableInstances.Clear();

            if(gen != this && (gen.transform.position - transform.position).magnitude < _consideranceRadius && !(_connectedInstances.ContainsKey(gen)))
            {
                _notableInstances.Add(gen);
            }
        }
    }

    float _hapSad = 2.0f, _arousDisgus = 2.0f, _angryFear = 2.0f, _energTired = 2.0f;
    public void UpdateEmotion(float hapSad, float arousDisgus, float angryFear, float energTired)
    {
        hapSad = (hapSad != -1) ? (hapSad + 1) / 2 : 0;
        arousDisgus = (arousDisgus != -1) ? (arousDisgus + 1) / 2 : 0;
        angryFear = (angryFear != -1) ? (angryFear + 1) / 2 : 0;
        energTired = (energTired != -1) ? (energTired + 1) / 2 : 0;
        
        if (Mathf.Abs(hapSad - _hapSad) > 0.01f)
        {
            _hapSad = hapSad;
        }

        if (Mathf.Abs(arousDisgus - _arousDisgus) > 0.01f)
        {
            _renderer.color = Color.Lerp(GameManager.Instance.Disgusted, GameManager.Instance.Aroused, arousDisgus);
            _arousDisgus = arousDisgus;
        }

        if (Mathf.Abs(angryFear - _angryFear) > 0.01f)
        {
            Vector2 sizes = GameManager.MinMaxEmotionSize;
            float range = sizes.y - sizes.x;
            float size = (angryFear * range) + sizes.x;
            _renderer.transform.localScale = new Vector3(size, size, 0);
            _angryFear = angryFear;
        }
    }

    

    IEnumerator EmotionAnimator()
    {
        while (true)
        {
            AnimationCalculator.CalculateSpriteBasedForFrame(GameManager.Instance.ShapeSheet, 97, 97, (int)(hapSad * GameManager.FramesInEmotionSheet), 1, (sprite) => { Destroy(_renderer.sprite); _renderer.sprite = sprite; });
        }

        yield break;
    }
}