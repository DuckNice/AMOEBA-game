using UnityEngine;
//using UnityEditor.Animations;
using System.Collections;
using System.Collections.Generic;
using NMoodyMaskSystem;

public class AMOEBAManager : MonoBehaviour {
    public string CharacterName;
    [SerializeField]
    [Tooltip("The max distance distance to a target connections are shown at")]
    protected float _connectionDistance = 80;
    public float ConnectionDistance { get { return _connectionDistance; } protected set { _connectionDistance = value; } }
    [SerializeField]
    [Tooltip("For optimization: The max distance to a target ")]
    protected float _consideranceRadius = 800;
    [SerializeField]
    float ClickRadius = 2f;
    [SerializeField]
    bool _ignoreRange;

    protected static List<AMOEBAManager> _instances = new List<AMOEBAManager>();
    protected List<AMOEBAManager> _notableInstances = new List<AMOEBAManager>();
    protected List<AMOEBAManager> _connectedInstances = new List<AMOEBAManager>();

    protected static WaitForSeconds _waitforO5 = new WaitForSeconds(0.5f);
    protected List<Opinion> _opinions = new List<Opinion>();
    protected UtilityTimer animationTimer;
    [SerializeField]
    //AnimatorController _emotionAnimator;

    SpriteRenderer _emotionRenderer;
    
    public Traits Traits{ get; private set; }
    Animator _emotionAnim;
    bool _opinionForcedVisible = false;
    bool _opinionForcedNotVisible = false;

    void Awake()
    {
        _instances.Add(this);
    }


    void OnLevelWasLoaded(int lvl)
    {
        _instances.Clear();
    }


    public void ToggleForceOpinionVisible(bool forced)
    {
        _opinionForcedVisible = forced;
    }

    public void ToggleForceOpinionNotVisible(bool forced)
    {
        _opinionForcedNotVisible = forced;
    }


    public static void KillCharacter(string name)
    {
        int index = _instances.FindIndex(x => x.CharacterName.Trim().ToLower() == name.Trim().ToLower());

        if(index > -1)
        {
            _instances[index].Kill();
        }
        else
        {
            Debug.LogWarning("Warning: Character with name '" + name + "' not killing character.");
        }
    }


    public void Kill()
    {
        GameManager.MoodyMask.AddPersonToUpdateList("Dead", GameManager.MoodyMask.GetPerson(CharacterName));
        GameManager.MoodyMask.RemovePersonFromUpdateList("Main", GameManager.MoodyMask.GetPerson(CharacterName));
    }


    void Start()
    {
        Being being = GetComponent<Being>();

        if(being != null)
        {
            CharacterName = being.Name;
        }

        GameObject emotions = new GameObject();
        _emotionRenderer = emotions.AddComponent<SpriteRenderer>();
        emotions.transform.parent = transform;
        emotions.transform.localPosition = new Vector3(0, 1, 0);
        emotions.gameObject.name = "Emotion";
        _emotionAnim = emotions.gameObject.AddComponent<Animator>();
       // _emotionAnim.runtimeAnimatorController = _emotionAnimator;

        Traits = Traits.BuildTrait(gameObject);

        StartCoroutine(OpinionsLoop());


        animationTimer = UtilityTimer.CreateUtilityTimer(gameObject, () => { EmotionAnimator(); }, GameManager.MinMaxEmotionSpeed.x);
    }


    public Opinion CreateConnection(AMOEBAManager other)
    {
        //TODO: Make connection only appear if there is an opinion. LONGER FIX: make an opinion appear which is blank.
        //   if ()
        //   {
        if (other.CharacterName != null && other.Traits != null)
        {
            Opinion opp = Opinion.CreateComponent(gameObject, CharacterName, other.CharacterName, other.Traits.gameObject);
            _opinions.Add(opp);
            opp.HideOpinion();
            _connectedInstances.Add(other);

            return opp;
        }

        return null;
     //   }
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
        emotionColor = Color.Lerp(GameManager.Instance.Sad, emotionColor, hapSad);
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
            float relSpeed = ((GameManager.MinMaxEmotionSpeed.y - GameManager.MinMaxEmotionSpeed.x) * energTired);
            animationTimer.SecondsBetweenTicks = GameManager.MinMaxEmotionSpeed.y - relSpeed;

            if (GameManager.Instance.UseUnityAlphaBlending)
            {
                _emotionAnim.SetFloat("Speed", (GameManager.MinMaxEmotionSpeed.x + relSpeed) * GameManager.FramesInEmotionAnimation);
            }
        }


        //Traits
        float charGreed = person.AbsTraits.Traits[TraitTypes.CharitableGreedy].GetTraitValue(),
            honFalse = person.AbsTraits.Traits[TraitTypes.HonestFalse].GetTraitValue(),
            niceNasty = person.AbsTraits.Traits[TraitTypes.NiceNasty].GetTraitValue();

        
        Traits.UpdateTraits(niceNasty, charGreed, honFalse);



        ToggleIsSelected();
        ShouldShowActionBar();
    }

    void ShouldShowActionBar()
    {
        if (Mathf.Pow((PlayerMotion.MouseClicked.x - transform.position.x), 2) + Mathf.Pow((PlayerMotion.MouseClicked.y - transform.position.y), 2) < Mathf.Pow(ClickRadius, 2) && PlayerMotion.RightClick)
        {
            GameManager.Instance.playerActionSelection.ToggleActionSelection(CharacterName, true);
        }
    }


    public void ToggleIsSelected()
    {
        if( (_ignoreRange || (Mathf.Pow((PlayerMotion.MouseClicked.x - transform.position.x), 2) + Mathf.Pow((PlayerMotion.MouseClicked.y - transform.position.y), 2) < Mathf.Pow(ClickRadius, 2))))
        {
            ToggleActiveOpinions(true);
        }
        else
        {
            ToggleActiveOpinions(false);
        }
    }


    private bool _isActive;

    public void ToggleActiveOpinions(bool active)
    {
        for(int i = _opinions.Count - 2; i >= 0; i--)
        {
            if(_opinions[i] == null)
            {
                _opinions.RemoveAt(i);
            }
        }

        if(_opinionForcedVisible || (!_opinionForcedNotVisible && active && !_isActive))
        {
            foreach(Opinion opinion in _opinions)
            {
                opinion.ShowOpinion();
            }
            _isActive = true;
        }
        else if(_opinionForcedNotVisible || (!_opinionForcedVisible && !active && _isActive))
        {
            foreach (Opinion opinion in _opinions)
            {
                opinion.HideOpinion();
            }
            _isActive = false;
        }
    }


    int _currFrame = 1000000;
    int _animationPlaying = 1;
    int _animationChangingTo = 2;

    void EmotionAnimator()
    {
        if (!GameManager.Instance.UseUnityAlphaBlending)
        {
            if (_currFrame < GameManager.FramesInEmotionAnimation)
                _currFrame++;
            else
            {
                _currFrame = 1;
                _animationPlaying = _animationChangingTo;
                do
                {
                    _animationChangingTo = Random.Range(1, GameManager.ActiveEmotions + 1);
                } while (_animationChangingTo == _animationPlaying);
            }

            int rowToTakeFrom = (_animationPlaying < _animationChangingTo) ? _animationChangingTo - 2 : _animationChangingTo - 1;

            AnimationCalculator.CalculateSpriteBasedForFrame(GameManager.EmotionShapeAnimations[_animationPlaying - 1], 100, 100, _currFrame, rowToTakeFrom, (sprite) => { Destroy(_emotionRenderer.sprite); _emotionRenderer.sprite = sprite; });
        }
    }
}