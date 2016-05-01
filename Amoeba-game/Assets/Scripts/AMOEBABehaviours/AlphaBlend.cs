using UnityEngine;

public class AlphaBlend : MonoBehaviour {
    Animator anim;


	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

        if (anim == null)
            anim = gameObject.AddComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        anim.SetFloat("Speed", 1);
	}
}
