using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    Vector2 _dragOrigin;
    bool _cameraMoving;
    public bool _cameraCanMove;

	// Update is called once per frame
	void Update ()
    {
        if (_cameraCanMove)
        {
            if (Input.GetMouseButtonDown(0))
            {

            }
            else if (Input.GetMouseButton(0))
            {

            }
        }
	}
}
