using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy;

    private void Update()
    {
        Destroy(gameObject, _timeToDestroy);
    }
}
