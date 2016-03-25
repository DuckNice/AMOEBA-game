using UnityEngine;
using System.Collections.Generic;
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


#region Emotions
    public Sprite ShapeSheet;
    [SerializeField]
    protected float _framesInEmotionSheet = 21;
    public static float FramesInEmotionSheet { get { return Instance._framesInEmotionSheet; } }

    [SerializeField]
    protected Vector2 _minMaxEmotionSize = new Vector2(1, 3);
    public static Vector2 MinMaxEmotionSize { get { return Instance._minMaxEmotionSize; } }

    [SerializeField]
    protected Vector2 _minMaxEmotionSpeed = new Vector2(1, 3);
    public static Vector2 MinMaxEmotionSpeed { get { return Instance._minMaxEmotionSpeed; } }

    public Material ConnectionMaterial;
    public Color NegativeColor;
    public Color NeutralColor;
    public Color PositiveColor;
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
        if (ShapeSheet == null ||
            NegativeColor == default(Color) || NeutralColor == default(Color) || PositiveColor == default(Color) || ConnectionMaterial == null)
        {
            Debug.LogError("Fatal error: GameManager is not properly setup. Closing Application");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}