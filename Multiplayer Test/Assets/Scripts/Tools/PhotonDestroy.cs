using Photon.Pun;
using UnityEngine;

public class PhotonDestroy : MonoBehaviour
{
    [SerializeField] float _timeToDestroy = 20f;

    private float _currentTime;

    private void Update()
    {
        _currentTime -= Time.deltaTime;

        if (_currentTime <= 0f)
        {
            _currentTime = 0f;
            PhotonNetwork.Destroy(gameObject);
        }
    }
}