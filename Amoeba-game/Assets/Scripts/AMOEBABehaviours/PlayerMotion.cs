using UnityEngine;

public class PlayerMotion : MonoBehaviour {
    public float _speed = 5;
    public static bool CanMove = true;

    private Vector3 _currentTargetPosition;


    void Update()
    {
        if (Input.GetMouseButton(0) && GameManager.GameOn && CanMove)
        {
            _currentTargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _currentTargetPosition = new Vector3(_currentTargetPosition.x, _currentTargetPosition.y, 0);
        }
    }


    void FixedUpdate()
    {
        if (_currentTargetPosition != default(Vector3) && GameManager.GameOn && CanMove)
        {
            Vector3 direction = _currentTargetPosition - transform.position;
            this.transform.Translate((_speed * Time.fixedDeltaTime) * direction.normalized);

            if (direction.magnitude < 0.1f)
            {
                _currentTargetPosition = default(Vector3);
            }
        }
    }
}