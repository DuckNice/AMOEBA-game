using UnityEngine;

public class Emotion : MonoBehaviour {
    SpriteRenderer _renderer;
    string _charName;
    float lastvalue = 0f;
    float lastEmotion = 0.5f;

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }


    public static Emotion CreateComponent(GameObject where, EmotionGenerator owner)
    {
        Emotion myC = where.AddComponent<Emotion>();

        myC._charName = owner.CharacterName;

        return myC;
    }
    public float value = -0.7f;

    void Update()
    {
        float emotion = 1;

        if(Mathf.Abs(emotion - lastEmotion) > 0.01f)
        {
            _renderer.sprite = GameManager.Instance.Shapes[2];
        }

    //    if(Mathf.Abs(value - lastvalue) > 0.01f)
        {
            _renderer.color = (value < 0) ? Color.Lerp(GameManager.Instance.NeutralColor, GameManager.Instance.NegativeColor, Mathf.Abs(value)) : 
                                            Color.Lerp(GameManager.Instance.NeutralColor, GameManager.Instance.PositiveColor, value);
        }
    }
}
