using UnityEngine;

public class StaticFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector2 _offset;

    private void Start()
    {
        if (_target == null)
        {
            _target = Camera.main.transform;
        }
    }

    private void LateUpdate() {
        if (_target == null) return;

        Vector3 targetPostion = new Vector3(
            _target.position.x + _offset.x,
            _target.position.y + _offset.y,
            0.0f);

        transform.position = targetPostion;
    }
}
