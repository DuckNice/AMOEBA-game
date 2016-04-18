using UnityEngine;
using NMoodyMaskSystem;

public class GameManager : Singleton<GameManager> {
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

    [Header("Emotion-based variables", order =1)]

#region Emotions
    public Sprite EmotionShapeSheet;
    [SerializeField]
    protected float _framesInEmotionSheet = 21;
    public static float FramesInEmotionSheet { get { return Instance._framesInEmotionSheet; } }

    [SerializeField]
    protected Vector2 _minMaxEmotionSize = new Vector2(1, 3);
    public static Vector2 MinMaxEmotionSize { get { return Instance._minMaxEmotionSize; } }

    [SerializeField]
    protected Vector2 _minMaxEmotionSpeed = new Vector2(1, 3);
    public static Vector2 MinMaxEmotionSpeed { get { return Instance._minMaxEmotionSpeed; } }

    [SerializeField]
    [Range(0f,1f)]
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
    public float TraitScale;
    [SerializeField]
    protected GameObject _spawnable;
    public GameObject Spawnable { get { return _spawnable; } }

    #endregion
    [Header("Opinion values", order = 3)]
#region Opinions
    [SerializeField]
    protected float _framesInOpinionSheet = 21;
    public static float FramesInOpinionSheet { get { return Instance._framesInEmotionSheet; } }


    public Sprite OpinionShapeSheet;
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
        if (EmotionShapeSheet == null ||
            Disgusted == default(Color) || Aroused == default(Color) || ConnectionMaterial == null)
        {
            Debug.LogError("Fatal error: GameManager is not properly setup. Closing Application");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}