using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class AnimationCalculator : MonoBehaviour {

	public static Sprite CalculateSpriteBasedForFrame(Sprite spriteSheet, int height, int width, int frameToGet, int animationToGet, UnityAction<Sprite> callback)
    {
        height = (int)Mathf.Clamp(height, 1, spriteSheet.rect.height);
        width = (int)Mathf.Clamp(width, 1, spriteSheet.rect.width);
        animationToGet = (int)Mathf.Clamp(animationToGet, 1, spriteSheet.rect.height / height);
        frameToGet = (int)Mathf.Clamp(frameToGet, 1, spriteSheet.rect.width / width);

        Sprite newSprite = Sprite.Create(spriteSheet.texture, new Rect(width * (frameToGet-1),height * (animationToGet-1),width, height), new Vector2(0.5f, 0.5f));

        if (callback != null)
        {
            callback(newSprite);
        }

        return newSprite;
    }
}
