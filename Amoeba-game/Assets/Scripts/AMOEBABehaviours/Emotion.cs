using UnityEngine;
using System.Collections;

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



    void Update()
    {
        float emotion = 1;
        float value = 0.5f;

        if(Mathf.Abs(emotion - lastEmotion) > 0.01f)
        {
            _renderer.sprite = GameManager.Instance.Shapes[2];
        }

        if(Mathf.Abs(value - lastvalue) > 0.01f)
        {
            _renderer.color = GameManager.Instance.Colors[0] * new Color(0.5f,0.5f,0.5f,1);
        }
    }
}
