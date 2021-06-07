using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

enum ShootingType {SingleFire, AutomaticFire}
public class Gun : MonoBehaviourPun
{
    [Header("Fire Type")]
    [SerializeField] private ShootingType _fireType;

    [Header("Speed")]
    [SerializeField] private float _speed;

    [Header("Interval")]
    [SerializeField] private float _timeShotInterval = 1f;


    [Header("Bullet")]
    [SerializeField] private GameObject _bulletPrefab;

    [SerializeField] private ParticleSystem _muzzleFlash;

    [Header("Spawn Point")]
    [SerializeField] private Transform _spawnPoint;

    [Header("Sounds")]
    [SerializeField] private AudioSource _sourcePrefab;
    [SerializeField] private AudioClip _shotSound;

    [Header("Buttons")]
    [SerializeField] private KeyCode _fire = KeyCode.Mouse0;

    private PhotonView _photonView;
    private float _currentTimeShotInterval;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        //if (!_photonView.IsMine)
        //{
        //    Destroy(gameObject);
        //}
    }

    private void Update()
    {
        _currentTimeShotInterval -= Time.deltaTime;
        CheckInterval();

        if (_fireType == ShootingType.SingleFire)
        {
            SingleFire();
        }
        else if (_fireType == ShootingType.AutomaticFire)
        {
            AutomaticShoot();
        }
    }

    private void CheckInterval()
    {
        if (_currentTimeShotInterval <= 0f)
        {
            _currentTimeShotInterval = 0f;
        }
    }

    private void SingleFire()
    {
        if (Input.GetKeyDown(_fire) && _photonView.IsMine)
        {
            if (_currentTimeShotInterval <= 0f)
            {
                RpcShot();
            }
        }
    }

    private void AutomaticShoot()
    {
        if (Input.GetKey(_fire) && _photonView.IsMine)
        {
            if (_currentTimeShotInterval <= 0f)
            {
                RpcShot();
            }
        }
    }

    [PunRPC]
    public void RpcShot()
    {
        GameObject bullet = PhotonNetwork.Instantiate(_bulletPrefab.name, _spawnPoint.position, _bulletPrefab.transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(transform.forward * _speed);

        GameObject muzzleFlashObject = PhotonNetwork.Instantiate(_muzzleFlash.name, _spawnPoint.position, _spawnPoint.rotation);
        ParticleSystem muzzleFlash = muzzleFlashObject.GetComponent<ParticleSystem>();
        muzzleFlash.Play();

        GameObject sound = PhotonNetwork.Instantiate(_sourcePrefab.name, _spawnPoint.position, _spawnPoint.rotation);
        AudioSource source = sound.GetComponent<AudioSource>();
        source.minDistance = 10f;
        source.PlayOneShot(_shotSound);
        _currentTimeShotInterval += _timeShotInterval;
    }
}