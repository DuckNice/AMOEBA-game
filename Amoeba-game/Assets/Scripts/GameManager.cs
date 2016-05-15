using UnityEngine;
using System.Collections.Generic;
using NMoodyMaskSystem;

public class GameManager : Singleton<GameManager> {

    public PlayerActionSelection playerActionSelection;

    public GameObject pauseScreen;
    [SerializeField]
    bool _gameOn = true;
    public static bool GameOn { get { return Instance._gameOn; } private set { Instance._gameOn = value; } }
    public static void ToggleGameOn(bool on)
    {
        GameOn = on;
    }

    [SerializeField]
    bool _UIAccessible = true;
    public static bool UIAccessible { get { return Instance._UIAccessible; } private set { Instance._UIAccessible = value; } }
    public static void ToggleUIAccessible(bool on)
    {
        UIAccessible = on;
    }

    [SerializeField]
    public KeyHoldEnable KeyHoldManager { get; protected set;}
    public static float Time { get; protected set; }
    [SerializeField]
    protected AIManager _AIManager;
    public static AIManager AIManager
    {
        get { return Instance._AIManager; }
    }
    public static MoodyMaskSystem MoodyMask
    {
        get { return Instance._AIManager.MoodyMask; }
    }

    [Header("Emotion-based variables", order = 1)]

    #region Emotions

    public bool UseUnityAlphaBlending = false;

    [SerializeField]
    protected List<Sprite> _emotionShapesNoAnimations = new List<Sprite>();
    public static List<Sprite> EmotionShapesNoAnimations { get { return Instance._emotionShapesNoAnimations; } }

    [SerializeField]
    protected List<Sprite> _emotionShapeAnimations = new List<Sprite>();
    public static List<Sprite> EmotionShapeAnimations { get { return Instance._emotionShapeAnimations; } }

    [SerializeField]
    protected int _activeEmotions = 2;
    public static int ActiveEmotions { get { return Instance._activeEmotions; } }
    
    [SerializeField]
    protected int _framesInEmotionSheet = 60;
    public static int FramesInEmotionAnimation { get { return Instance._framesInEmotionSheet; } }

    [SerializeField]
    protected Vector2 _minMaxEmotionSize = new Vector2(1, 3);
    public static Vector2 MinMaxEmotionSize { get { return Instance._minMaxEmotionSize; } }

    [SerializeField]
    protected Vector2 _minMaxEmotionSpeed = new Vector2(1, 3);
    public static Vector2 MinMaxEmotionSpeed { get { return Instance._minMaxEmotionSpeed; } }

    [SerializeField]
    protected Vector2 _minMaxBrightness = new Vector2(0, 1);
    public static Vector2 MinMaxBrightness { get { return Instance._minMaxBrightness; } }

    public Material ConnectionMaterial;
    [SerializeField]
    private Color _disgusted;
    [SerializeField]
    private Color _aroused;
    [SerializeField]
    private Color _sad;
    public Color Disgusted { get { return _disgusted; } }
    public Color Aroused { get { return _aroused; } }
    public Color Sad { get { return _sad; } }
    #endregion

    [Header("Traits values", order = 2)]
    #region Traits
    [SerializeField]
    private Color _nice;
    [SerializeField]
    private Color _nasty;
    public Color Nice { get { return _nice; } }
    public Color Nasty { get { return _nasty; } }
    public float traitSpikes;
    public Sprite TraitCore;
    public Sprite TraitSpike;
    public Sprite HaloSprite;
    [SerializeField]
    public float FramesInTraitSpike = 21;
    public int TraitSpikesInCircle = 10;
    public float TraitCircleDiameter = 0.5f;
    public Vector2 MinMaxTraitRotationSpeed;
    public Vector2 MinMaxTraitRotationDuration;
    public float TraitScale;
    [SerializeField]
    protected GameObject _spawnable;
    public GameObject Spawnable { get { return _spawnable; } }

    #endregion
       [Header("Opinion values", order = 3)]
    #region Opinions

    [SerializeField]
    protected Sprite _opinionRepresentation;
    public static Sprite OpinionRepresentation { get { return Instance._opinionRepresentation; } }

    [SerializeField]
    private Color _dislikeTrait;
    public static Color DislikeTrait { get { return Instance._dislikeTrait; } }

    [SerializeField]
    private Color _likeTrait;
    public static Color LikeTrait { get { return Instance._likeTrait; } }

    [SerializeField]
    private float _opinionScale;
    public static float OpinionScale { get { return Instance._opinionScale; } }

    #endregion


    void Awake()
    { 
        if(_AIManager == null)
        {
            _AIManager = GetComponent<AIManager>();
            if (_AIManager == null)
            {
                _AIManager = gameObject.AddComponent<AIManager>();
            }
        }
    }


    void Update()
    {
        if(GameOn)
        {
            Time += UnityEngine.Time.deltaTime;
        }
    }

    public void TogglePauseScreen()
    {
        if(pauseScreen.activeSelf)
        {
            pauseScreen.SetActive(false);
            GameOn = true;
        }
        else
        {
            pauseScreen.SetActive(true);
            GameOn = false;
        }
    }
}