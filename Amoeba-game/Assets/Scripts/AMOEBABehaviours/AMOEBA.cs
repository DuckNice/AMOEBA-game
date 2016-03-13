using UnityEngine;

public class AMOEBA : MonoBehaviour {
    public float _speed = 2;

    [SerializeField]
    bool _isPlayer;

    private Vector3 _currentTargetPosition;


    void Update()
    {
        if(_isPlayer)
        {
            if (Input.GetMouseButton(0))
            {
                _currentTargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _currentTargetPosition = new Vector3(_currentTargetPosition.x, _currentTargetPosition.y, 0);
            }
        }
    }


    void FixedUpdate()
    {
        if(_currentTargetPosition != default(Vector3))
        {
            Vector3 direction = _currentTargetPosition - transform.position;
            this.transform.Translate((_speed * Time.fixedDeltaTime) * direction.normalized);

            if(direction.magnitude < 0.1f)
            {
                _currentTargetPosition = default(Vector3);
            }
        }
    }
}