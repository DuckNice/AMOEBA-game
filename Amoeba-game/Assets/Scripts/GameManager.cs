using UnityEngine;
using System.Collections.Generic;
using NMoodyMaskSystem;

public class GameManager : Singleton<GameManager> {
    public static float Time { get; protected set; }
    //EmotionStuff
    public Sprite Shape;
    public Vector2 MinMaxSize = new Vector2(1, 2);
    public Color NegativeColor;
    public Color NeutralColor;
    public Color PositiveColor;
    public List<Sprite> Shapes = new List<Sprite>();
    public Material ConnectionMaterial;
    [SerializeField]
    protected AIManager _AIManager;
    public static AIManager AIManager {
        get { return Instance._AIManager; }
    }
    public static MoodyMaskSystem MoodyMask
    {
        get { return Instance._AIManager.MoodyMask; }
    }


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
        if (Shape == null || Shapes.Count != 4 ||
            NegativeColor == default(Color) || NeutralColor == default(Color) || PositiveColor == default(Color) ||
            MinMaxSize.x <= 0 || MinMaxSize.x > MinMaxSize.y || MinMaxSize.y <= 0 || ConnectionMaterial == null)
        {
            Debug.LogError("Fatal error: GameManager is not properly setup. Closing Application");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}