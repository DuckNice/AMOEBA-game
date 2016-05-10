using UnityEngine;

public class PlayerMotion : MonoBehaviour {
    public static Vector2 MouseClicked { get; private set; }
    public static bool RightClick { get; private set; }
    public float _speed = 5;
    public static bool CanMove = true;

    private Vector3 _currentTargetPosition;


    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            MouseClicked = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameManager.Instance.playerActionSelection.ToggleActionSelection("", false);
            /*
            _currentTargetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _currentTargetPosition = new Vector3(_currentTargetPosition.x, _currentTargetPosition.y, 0);
            */
        }

        RightClick = Input.GetMouseButtonDown(1);
    }


    void FixedUpdate()
    {
        /*
        if (_currentTargetPosition != default(Vector3) && GameManager.GameOn && CanMove)
        {
            Vector3 direction = _currentTargetPosition - transform.position;
            this.transform.Translate((_speed * Time.fixedDeltaTime) * direction.normalized);

            if (direction.magnitude < 0.1f)
            {
                _currentTargetPosition = default(Vector3);
            }
        }*/
    }
}