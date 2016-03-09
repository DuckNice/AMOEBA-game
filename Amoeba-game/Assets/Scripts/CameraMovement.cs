using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

    Vector2 _dragOrigin;
    Vector2 _1pixelDistance;
    Vector3 _dragMidOrigin;
    public bool _cameraMoving { get; private set; }
    public bool _cameraCanMove = true;
    public float _heightAboveMap = -10;

	// Update is called once per frame
	void Update ()
    {
        if (_cameraCanMove)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _dragOrigin = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);

                Vector2 go = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
                _dragMidOrigin = Camera.main.ScreenToWorldPoint(new Vector3(go.x, go.y, _heightAboveMap));
                _1pixelDistance = Camera.main.ScreenToWorldPoint(new Vector3((go.x) + 1, (go.y) + 1, _heightAboveMap)) - _dragMidOrigin;
            }
            else if (Input.GetMouseButton(0))
            {
                _cameraMoving = true;
                Vector2 _newposition = new Vector2(Input.mousePosition.x - _dragOrigin.x, Input.mousePosition.y - _dragOrigin.y);

                Camera.main.transform.position = new Vector3(_dragMidOrigin.x - (_newposition.x * _1pixelDistance.x), _dragMidOrigin.y - (_newposition.y * _1pixelDistance.y), _heightAboveMap);
            }
            else
            {
                _cameraMoving = false;
            }
        }
	}
}
