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
    [SerializeField]
    protected GameObject _spawnable;
    protected UtilityTimer animationTimer;
    public GameObject TraitObj { get; protected set; }
    SpriteRenderer _emotionRenderer;
    Transform _traitHolder;
    SpriteRenderer _traitCoreRenderer;
    List<Transform> _traitSpikes;
    List<SpriteRenderer> _traitSpikeRenderers = new List<SpriteRenderer>();
    SpriteRenderer _glowRenderer;


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

        BuildTrait();
        


        StartCoroutine(OpinionsLoop());

        animationTimer = UtilityTimer.CreateUtilityTimer(gameObject, GameManager.MinMaxEmotionSpeed.x, () => { Animator(); });
    }

    void BuildTrait()
    {
        GameObject traits = new GameObject();
        // _traitRenderer = traits.AddComponent<SpriteRenderer>();
        traits.transform.parent = transform;
        traits.transform.localPosition = new Vector3(1, 0, 0);
        traits.gameObject.name = "Trait";
        _traitCoreRenderer = traits.AddComponent<SpriteRenderer>();


        GameObject halo = new GameObject();
        halo.transform.parent = traits.transform;
        halo.transform.localPosition = new Vector3(0, 0, 0);
        halo.gameObject.name = "Trait halo";
        _glowRenderer = halo.AddComponent<SpriteRenderer>();
        _glowRenderer.sprite = GameManager.Instance.HaloSprite;
    }


    public void CreateConnection(AMOEBAManager other)
    {
        GameObject opinionHolder = Instantiate(_spawnable);
        opinionHolder.name = "Opinion towards " + other.CharacterName;
        opinionHolder.transform.parent = transform;
        Opinion.CreateComponent(opinionHolder, CharacterName, other.CharacterName, other.TraitObj);
        

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
        Color emotionColor = Color.Lerp(GameManager.Instance.Disgusted, GameManager.Instance.Aroused, arousDisgus) * hapSad;
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

        //Adjust emotions for use (0f to 1f).
        niceNasty = (niceNasty != -1) ? (niceNasty + 1) / 2 : 0;
        honFalse = (honFalse != -1) ? (honFalse + 1) / 2 : 0;
        charGreed = (charGreed != -1) ? (charGreed + 1) / 2 : 0;

        //Set Nice/Nasty
        Color halo = Color.Lerp(GameManager.Instance.Nice, GameManager.Instance.Nasty, niceNasty);
        emotionColor.a = 1;
        _glowRenderer.color = halo;

        //Set Greedy/Charitable
        float angle = 180f * charGreed;
        //   _emotionRenderer.transform.localScale = new Vector3(size, size, 0);

        //Set Hon/False
        int honFalFrame = (int)(honFalse * GameManager.Instance.FramesInTraitSpike);

        if (honFalFrame != _lastHonestFalseFrame)
        {

            AnimationCalculator.CalculateSpriteBasedForFrame(GameManager.Instance.TraitCore, 97, 97, honFalFrame, 1, (sprite) => { Destroy(_traitCoreRenderer.sprite); _traitCoreRenderer.sprite = sprite; });
            foreach(SpriteRenderer renderer in _traitSpikeRenderers)
            {
                AnimationCalculator.CalculateSpriteBasedForFrame(GameManager.Instance.TraitSpike, 97, 97, honFalFrame, 1, (sprite) => { Destroy(renderer.sprite); renderer.sprite = sprite; });
            }
            _lastHonestFalseFrame = honFalFrame;
        }
        /*  if (animationTimer != null)
          {
              float relSpeed = (GameManager.MinMaxEmotionSpeed.y - GameManager.MinMaxEmotionSpeed.x) * energTired;
              animationTimer.SecondsBetweenTicks = GameManager.MinMaxEmotionSpeed.x + relSpeed;
          }*/
    }
    int _lastHonestFalseFrame = -10;


    int currFrame = 1;
    void Animator()
    {
        currFrame = (currFrame < GameManager.FramesInEmotionSheet) ? currFrame + 1 : 1;

        AnimationCalculator.CalculateSpriteBasedForFrame(GameManager.Instance.EmotionShapeSheet, 97, 97, currFrame, 1, (sprite) => { Destroy(_emotionRenderer.sprite); _emotionRenderer.sprite = sprite; });
    }
}