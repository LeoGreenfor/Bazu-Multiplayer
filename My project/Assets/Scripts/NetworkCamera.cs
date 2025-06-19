using UnityEngine;

public class NetworkCamera : MonoBehaviour
{
    [SerializeField]
    private Vector3 offset;

    public GameObject _target;
    private Transform _currentTargetTransform;
    private Vector3 _newPosition;

    public void SetTarget(GameObject target)
    {
        Debug.Log("a");
        _target = target;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        _currentTargetTransform = _target.transform;
        _newPosition = _currentTargetTransform.position - offset;

        transform.position = Vector3.MoveTowards(transform.position, _newPosition, 1f);
        transform.LookAt(_currentTargetTransform.position);
    }
}
