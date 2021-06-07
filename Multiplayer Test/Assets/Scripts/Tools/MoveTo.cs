using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField] private Transform _target;
    private MoveTo _moveTo;

    private void Awake()
    {
        _moveTo = GetComponent<MoveTo>();
    }

    private void Update()
    {
        if (_target)
        {
            transform.position = _target.position;
            transform.eulerAngles = _target.rotation.eulerAngles;
        }
        else
        {
            Destroy(_moveTo);
        }
    }
}