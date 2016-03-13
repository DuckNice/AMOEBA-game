using UnityEngine;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager> {
    //EmotionStuff
    public List<Sprite> Shapes = new List<Sprite>();
    public List<Color> Colors = new List<Color>();
    public Vector2 MinMaxSize = new Vector2(1, 2);
    public Material ConnectionMaterial;
	

    void Awake()
    {
        if(Shapes.Count != 4 || Colors.Count != 4 || MinMaxSize.x <= 0 || MinMaxSize.y <= 0 || ConnectionMaterial == null)
        {
            Debug.LogError("Fatal error: GameManager is not properly setup. Closing Application");
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
}