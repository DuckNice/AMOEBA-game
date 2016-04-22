using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NMoodyMaskSystem;

public class AMOEBAManager : MonoBehaviour {
    public string CharacterName;
    [SerializeField]
    [Tooltip("The max distance distance to a target connections are shown at")]
    protected float _connectionDistance = 2;
    public float ConnectionDistance { get { return _connectionDistance; } protected set { _connectionDistance = value; } }
    [SerializeField]
    [Tooltip("For optimization: The max distance to a target ")]
    protected float _consideranceRadius = 10;

    protected static List<AMOEBAManager> _instances = new List<AMOEBAManager>();
    protected List<AMOEBAManager> _notableInstances = new List<AMOEBAManager>();
    protected List<AMOEBAManager> _connectedInstances = new List<AMOEBAManager>();

    protected static WaitForSeconds _waitforO5 = new WaitForSeconds(0.5f);
    protected List<GameObject> _opinions = new List<GameObject>();
    protected UtilityTimer animationTimer;

    SpriteRenderer _emotionRenderer;
    
    public Traits Traits{ get; private set; }


    void Awake()
    {
        _instances.Add(this);
    }


    void Start()
    {
        GameObject emotions = new GameObject();
        _emotionRenderer = emotions.AddComponent<SpriteRenderer>();
        emotions.transform.parent = transform;
        emotions.transform.localPosition = new Vector3(0, 1, 0);
        emotions.gameObject.name = "Emotion";

        Traits = Traits.BuildTrait(gameObject);

        StartCoroutine(OpinionsLoop());

        animationTimer = UtilityTimer.CreateUtilityTimer(gameObject, () => { Animator(); }, GameManager.MinMaxEmotionSpeed.x);
    }

    


    public void CreateConnection(AMOEBAManager other)
    {
        
        Opinion.CreateComponent(gameObject, CharacterName, other.CharacterName, other.Traits.gameObject);
        

        _connectedInstances.Add(other);
    }
    

    private IEnumerator OpinionsLoop()
    {
        while(true)
        {
            UpdateNotableInstances();

            foreach (AMOEBAManager gen in _notableInstances)
            {
                if ((gen.transform.position - transform.position).magnitude < _connectionDistance && !(_connectedInstances.Contains(gen)))
                {
                    CreateConnection(gen);
                }
            }

            yield return _waitforO5;
        }
    }


    protected void UpdateNotableInstances()
    {
        _notableInstances.Clear();

        foreach (AMOEBAManager gen in _instances)
        {
            if(gen != this && (gen.transform.position - transform.position).magnitude < _consideranceRadius && !(_connectedInstances.Contains(gen)))
            {
                _notableInstances.Add(gen);
            }
        }
    }


    Person person;
    public void Update()
    {
        //Emotions
        if (person == null)
        {
            person = GameManager.MoodyMask.GetPerson(CharacterName);

            if (person == null)
            {
                Debug.LogError("Error: No person with name: '" + CharacterName + "' in PersonContainer. not updating emotion.");
                return;
            }
        }

        float hapSad = person.Moods[MoodTypes.hapSad], arousDisgus = person.Moods[MoodTypes.arousDisgus], 
            angryFear = person.Moods[MoodTypes.angryFear], energTired = person.Moods[MoodTypes.energTired];

        //Adjust emotions for use (0f to 1f).
        hapSad = (hapSad != -1) ? (hapSad + 1) / 2 : 0;
        arousDisgus = (arousDisgus != -1) ? (arousDisgus + 1) / 2 : 0;
        angryFear = (angryFear != -1) ? (angryFear + 1) / 2 : 0;
        energTired = (energTired != -1) ? (energTired + 1) / 2 : 0;
        
        //Set happy/sad & aroused/disgusted
        Color emotionColor = Color.Lerp(GameManager.Instance.Disgusted, GameManager.Instance.Aroused, arousDisgus);
        emotionColor = Color.Lerp(emotionColor, GameManager.Instance.Sad, hapSad);
        emotionColor.a = 1;
        _emotionRenderer.color = emotionColor;

        //Set anger/fear
        Vector2 sizes = GameManager.MinMaxEmotionSize;
        float range = sizes.y - sizes.x;
        float size = (angryFear * range) + sizes.x;
        _emotionRenderer.transform.localScale = new Vector3(size, size, 0);

        //Set energetic/tired
        if (animationTimer != null)
        {
            float relSpeed = (GameManager.MinMaxEmotionSpeed.y - GameManager.MinMaxEmotionSpeed.x) * energTired;
            animationTimer.SecondsBetweenTicks = GameManager.MinMaxEmotionSpeed.x + relSpeed;
        }


        //Traits
        float charGreed = person.AbsTraits.Traits[TraitTypes.CharitableGreedy].GetTraitValue(),
            honFalse = person.AbsTraits.Traits[TraitTypes.HonestFalse].GetTraitValue(),
            niceNasty = person.AbsTraits.Traits[TraitTypes.NiceNasty].GetTraitValue();

        
        Traits.UpdateTraits(niceNasty, charGreed, honFalse);
    }


    int currFrame = 1;


    void Animator()
    {
        currFrame = (currFrame < GameManager.FramesInEmotionSheet) ? currFrame + 1 : 1;

        AnimationCalculator.CalculateSpriteBasedForFrame(GameManager.Instance.EmotionShapeSheet, 97, 97, currFrame, 1, (sprite) => { Destroy(_emotionRenderer.sprite); _emotionRenderer.sprite = sprite; });
    }
}