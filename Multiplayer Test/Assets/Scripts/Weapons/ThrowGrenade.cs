using UnityEngine;
using Photon.Pun;

public class ThrowGrenade : MonoBehaviourPun
{
    [Header("Values")]
    [SerializeField] private float _throwForce = 1000f;
    [SerializeField] private float _throwInterval = 1f;

    [Header("Button")]
    [SerializeField] private KeyCode _throwButton;

    [Header("Audio")]
    [SerializeField] private AudioSource _sourcePrefab;
    [SerializeField] private AudioClip _throwSound;

    [Header("Prefabs")]
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _grenadePrefab;

    private PhotonView _photonView;
    private float _currentThrowInterval;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {
        _currentThrowInterval -= Time.deltaTime;

        if (_currentThrowInterval <= 0f)
        {
            _currentThrowInterval = 0f;
            Throw();
        }
    }

    private void Throw()
    {
        if (Input.GetKeyDown(_throwButton) && _photonView.IsMine)
        {
            GameObject grenadeObject = PhotonNetwork.Instantiate(_grenadePrefab.name, _spawnPoint.position, _grenadePrefab.transform.rotation);
            grenadeObject.GetComponent<Rigidbody>().AddForce(transform.forward * _throwForce);

            GameObject sourceObject = PhotonNetwork.Instantiate(_sourcePrefab.name, _spawnPoint.position, Quaternion.identity);
            AudioSource source = sourceObject.GetComponent<AudioSource>();
            source.PlayOneShot(_throwSound);

            _currentThrowInterval += _throwInterval;
        }
    }
}