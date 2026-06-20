using UnityEngine;

public class SimpleFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _target;
    public Transform Target => _target;

    [Header("Follow params")]
    [SerializeField] private Vector3 _offset;


    private void Update()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector3 newPos = new Vector3(
            Target.position.x + _offset.x,
            Target.position.y + _offset.y,
            Target.position.z + _offset.z
            );

        transform.position = newPos;
    }
}
