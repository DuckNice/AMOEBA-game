using UnityEngine;

public class Emotion : MonoBehaviour {
    SpriteRenderer _renderer;
#pragma warning disable 0414
    string _charName;
#pragma warning restore 0414
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

    void Update()
    {
        float emotion = 1;
        float value = -0.7f;

        if (Mathf.Abs(emotion - lastEmotion) > 0.01f)
        {
            AnimationCalculator.CalculateSpriteBasedForFrame(GameManager.Instance.Shape, 130, 120, 2, 1, (sprite) => { Destroy(_renderer.sprite); _renderer.sprite = sprite; });

        //    _renderer.sprite = GameManager.Instance.Shapes[2];
        }

        if(Mathf.Abs(value - lastvalue) > 0.01f)
        {
            _renderer.color = (value < 0) ? Color.Lerp(GameManager.Instance.NeutralColor, GameManager.Instance.NegativeColor, Mathf.Abs(value)) : 
                                            Color.Lerp(GameManager.Instance.NeutralColor, GameManager.Instance.PositiveColor, value);
        }
    }
}