using UnityEngine;
using System.Collections.Generic;

public class Traits : MonoBehaviour {

    SpriteRenderer _traitCoreRenderer;
    List<Transform> _traitSpikes = new List<Transform>();
    List<SpriteRenderer> _traitSpikeRenderers = new List<SpriteRenderer>();
    SpriteRenderer _glowRenderer;
    UtilityTimer RotationMovement;

    int _lastHonestFalseFrame = -10;

    void Start()
    {
        int ticksForAnimation = Random.Range((int)GameManager.Instance.MinMaxTraitRotationDuration.x, (int)GameManager.Instance.MinMaxTraitRotationDuration.y);
        float rotationSpeed = Random.Range(GameManager.Instance.MinMaxTraitRotationSpeed.x, GameManager.Instance.MinMaxTraitRotationSpeed.y);
        rotationSpeed = (Random.Range(-1,1) > 1) ? rotationSpeed : -rotationSpeed;

        RotationMovement = UtilityTimer.CreateUtilityTimer(gameObject, () => { transform.Rotate(new Vector3(0, 0, rotationSpeed)); }, 0.05f, ticksForAnimation);
    }

#region BuildTraitFunctionality
    public static Traits BuildTrait(GameObject parent)
    {
        GameObject traits = new GameObject();
        Traits traitInst = traits.AddComponent<Traits>();
        traits.transform.parent = parent.transform;
        traits.transform.localPosition = new Vector3(1, 0, 0);
        traits.gameObject.name = "Traits";
        traitInst._traitCoreRenderer = traits.AddComponent<SpriteRenderer>();

        
        GameObject halo = new GameObject();
        halo.transform.parent = traits.transform;
        halo.transform.localPosition = new Vector3(0, 0, 0);
        halo.gameObject.name = "Trait halo";
        traitInst._glowRenderer = halo.AddComponent<SpriteRenderer>();
        traitInst._glowRenderer.sprite = GameManager.Instance.HaloSprite;

        traitInst.UpdateTraits(0,0,0);

        return traitInst;
    }


    public void BuildTraitCircle(float angleRotation)
    {
#region Fitting the right amount of spikes
        if(GameManager.Instance.TraitSpikesInCircle != _traitSpikes.Count)
        {
            if (GameManager.Instance.TraitSpikesInCircle > _traitSpikes.Count)
                for (int i = GameManager.Instance.TraitSpikesInCircle - _traitSpikes.Count; i > 0; i--)
                {
                    GameObject spawned = Instantiate(GameManager.Instance.Spawnable);
                    _traitSpikes.Add(spawned.transform);
                    spawned.name = "Trait spike";
                    spawned.transform.SetParent(transform, false);

                    SpriteRenderer renderer = spawned.GetComponent<SpriteRenderer>();
                    _traitSpikeRenderers.Add((renderer != null) ? renderer : spawned.gameObject.AddComponent<SpriteRenderer>());
                }
            else
                for (int i = _traitSpikes.Count - GameManager.Instance.TraitSpikesInCircle; i >= 0; i--)
                {
                    Destroy(_traitSpikes[_traitSpikes.Count - 1]);
                    _traitSpikes.RemoveAt(_traitSpikes.Count - 1);
                }
        }
#endregion


#region Position and rotation of each spike
        float anglePosEachPiece = 360f / Mathf.Max(GameManager.Instance.TraitSpikesInCircle, 1);
        float diameter = GameManager.Instance.TraitCircleDiameter;


        for(int i = _traitSpikes.Count -1; i >= 0; i--)
        {
            float pieceAnglePos = i * anglePosEachPiece;

            _traitSpikes[i].localPosition = new Vector3((Mathf.Cos(Mathf.Deg2Rad * pieceAnglePos)) * diameter , (Mathf.Sin(Mathf.Deg2Rad * pieceAnglePos)) * diameter, -0.1f);
            _traitSpikes[i].localScale = new Vector3(0.5f, 0.5f, 0.5f);

            _traitSpikes[i].localRotation = Quaternion.Euler(0, 0, (pieceAnglePos - 90) - angleRotation);
        }
#endregion
    }


    public void UpdateTraits(float niceNasty, float charGreed, float honFalse)
    {
        //Adjust traits for use (0f to 1f).
        niceNasty = (niceNasty != -1) ? (niceNasty + 1) / 2 : 0;
        honFalse = (honFalse != -1) ? (honFalse + 1) / 2 : 0;
        charGreed = (charGreed != -1) ? (charGreed + 1) / 2 : 0;
        
        //Set Nice/Nasty
        Color halo = Color.Lerp(GameManager.Instance.Nice, GameManager.Instance.Nasty, niceNasty);
        halo.a = 1;
        _glowRenderer.color = halo;

        //Set Greedy/Charitable
        float angle = 180f * charGreed;
        BuildTraitCircle(angle);

        //Set Hon/False
        int honFalFrame = (int)(honFalse * GameManager.Instance.FramesInTraitSpike);

        if (honFalFrame != _lastHonestFalseFrame)
        {
            AnimationCalculator.CalculateSpriteBasedForFrame(GameManager.Instance.TraitCore, 97, 97, honFalFrame, 1, (sprite) => { Destroy(_traitCoreRenderer.sprite); _traitCoreRenderer.sprite = sprite; });
            _traitCoreRenderer.color = new Color(0, 0, 0);
            
            AnimationCalculator.CalculateSpriteBasedForFrame(GameManager.Instance.TraitSpike, 97, 97, honFalFrame, 1, (sprite) => {
                foreach (SpriteRenderer renderer in _traitSpikeRenderers)
                {
                    Destroy(renderer.sprite);
                    renderer.sprite = sprite;
                    renderer.color = new Color(0, 0, 0);
                }
            });

            _lastHonestFalseFrame = honFalFrame;
        }
    }
    #endregion

    void Update()
    {
        Rotation();
    }

    #region TraitMovement

    void Rotation()
    {
        if(RotationMovement.IsTimerRunning() == -1)
        {
            int ticksForAnimation = Random.Range((int)GameManager.Instance.MinMaxTraitRotationDuration.x, (int)GameManager.Instance.MinMaxTraitRotationDuration.y);
            float rotationSpeed = Random.Range(GameManager.Instance.MinMaxTraitRotationSpeed.x, GameManager.Instance.MinMaxTraitRotationSpeed.y);
            rotationSpeed = (Random.Range(0f, 2f) > 1f) ? rotationSpeed : -rotationSpeed;

            RotationMovement.UpdateDelegate(() => { transform.Rotate(new Vector3(0, 0, rotationSpeed)); }, ticksForAnimation);
        }
    }
    
    #endregion
}