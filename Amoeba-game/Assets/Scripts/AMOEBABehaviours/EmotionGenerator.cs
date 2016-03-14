using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    protected List<GameObject> _emotions = new List<GameObject>();
    [SerializeField]
    protected GameObject _emotionPrefab;


    void Awake()
    {
        _instances.Add(this);

        StartCoroutine(UpdateNotableInstancesLoop());
    }


    public void Update()
    {
        foreach(EmotionGenerator gen in _notableInstances)
        {
            if ((gen.transform.position - transform.position).magnitude < _connectionDistance && !(_connectedInstances.ContainsKey(gen)))
            {
                CreateConnection(gen);
            }
        }
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

            thisEmotion.gameObject.name = "EmotionTowards" + other.gameObject.name;

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


    private IEnumerator UpdateNotableInstancesLoop()
    {
        while(true)
        {
            UpdateNotableInstances();

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
}