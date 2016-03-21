using UnityEngine;
using NMoodyMaskSystem;

public class AIManager : MonoBehaviour {
    public MoodyMaskSystem MoodyMask { get; private set; }
    
    void Awake()
    {
        MoodyMask = new MoodyMaskSystem();


    }

    void Start()
    {

    }
}