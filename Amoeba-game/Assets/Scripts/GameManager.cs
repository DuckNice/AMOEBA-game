using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
    //EmotionStuff
    public Sprite Shape;
    public Vector2 MinMaxSize = new Vector2(1, 2);
    public Color NegativeColor;
    public Color NeutralColor;
    public Color PositiveColor;
    public List<Sprite> Shapes = new List<Sprite>();
    public Material ConnectionMaterial;
	

    void Awake()
    { 
        if (Shape == null || Shapes.Count != 4 ||
            NegativeColor == default(Color) || NeutralColor == default(Color) || PositiveColor == default(Color) ||
            MinMaxSize.x <= 0 || MinMaxSize.x > MinMaxSize.y || MinMaxSize.y <= 0 || ConnectionMaterial == null)
        {
            Debug.LogError("Fatal error: GameManager is not properly setup. Closing Application");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}