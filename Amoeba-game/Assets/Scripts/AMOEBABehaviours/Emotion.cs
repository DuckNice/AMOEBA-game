using UnityEngine;

public class Emotion : MonoBehaviour {
    SpriteRenderer _renderer;
#pragma warning disable 0414
    string _charName;
#pragma warning restore 0414
    float lastvalue = 0f;
    float _hapSad, _arousDisgus, _angryFear, _energTired = 0.0f;

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

    public void UpdateEmotion(float hapSad, float arousDisgus, float angryFear, float energTired)
    {
        float value = -0.7f;

        if (Mathf.Abs(hapSad - _hapSad) > 0.05f)
        {
            AnimationCalculator.CalculateSpriteBasedForFrame(GameManager.Instance.ShapeSheet, 97, 97, (int)(hapSad * GameManager.FramesInEmotionSheet), 1, (sprite) => { Destroy(_renderer.sprite); _renderer.sprite = sprite; });
        }

        if(Mathf.Abs(value - lastvalue) > 0.01f)
        {
            _renderer.color = (value < 0) ? Color.Lerp(GameManager.Instance.NeutralColor, GameManager.Instance.NegativeColor, Mathf.Abs(value)) : 
                                            Color.Lerp(GameManager.Instance.NeutralColor, GameManager.Instance.PositiveColor, value);
        }
    }
}