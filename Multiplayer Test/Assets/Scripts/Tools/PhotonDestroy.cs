using Photon.Pun;
using UnityEngine;

public class PhotonDestroy : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy = 20f;

    private float _currentTime;

    private void Start()
    {
        _currentTime += _timeToDestroy;
    }

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0f)
        {
            _currentTime = 0f;
            Destroy(gameObject);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}