using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _throwForce;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private float _timerToExplosion = 3f;
    [SerializeField] private float _timerToDestroy = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioSource _sourcePrefab;
    [SerializeField] private AudioClip _explosionSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _explosion;

    private float _currentTimeToExplosion;
    private float _currentTimeToDestroy;

    private void Start()
    {
        _currentTimeToExplosion += _timerToExplosion;
    }

    private void Update()
    {
        _currentTimeToExplosion -= Time.deltaTime;
        _currentTimeToDestroy -= Time.deltaTime; 

        if (_currentTimeToExplosion <= 0f)
        {
            _currentTimeToExplosion = 0f;

            PhotonNetwork.Instantiate(_explosion.name, transform.position, Quaternion.identity);
            AudioSource source = PhotonNetwork.Instantiate(_sourcePrefab.name, transform.position, Quaternion.identity).GetComponent<AudioSource>();
            source.PlayOneShot(_explosionSound);

            _currentTimeToDestroy += _timerToDestroy;

            if (_currentTimeToDestroy <= 0f)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<Player>())
        {
            collider.GetComponent<Player>().GetDamage(_explosionForce);
            collider.GetComponent<Rigidbody>().AddExplosionForce(_explosionForce, transform.position, _radius);
        }
    }
}
