using Photon.Pun;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float _damageCount = 20f;
    [SerializeField] private float _radius = 10f;
    [SerializeField] private float _explosionForce;
    [SerializeField] private float _timerToExplosion = 3f;
    [SerializeField] private float _timerToDestroy = 0.2f;

    [Header("Audio")]
    [SerializeField] private AudioSource _sourcePrefab;
    [SerializeField] private AudioClip _explosionSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _explosionEffect;

    [Header("Gizmos")]
    [SerializeField] private float _r, _g, _b;

    private float _currentTimeToExplosion;
    private float _currentTimeToDestroy;
    private bool _hasExploded;


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
            if (!_hasExploded)
            {
                Explosion();
            }
        }
    }

    private void Explosion()
    {
        _currentTimeToExplosion = 0f;

        PhotonNetwork.Instantiate(_explosionEffect.name, transform.position, Quaternion.identity);

        AudioSource source = PhotonNetwork.Instantiate(_sourcePrefab.name, transform.position, Quaternion.identity).GetComponent<AudioSource>();
        source.PlayOneShot(_explosionSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            Player player = colliders[i].GetComponent<Player>();
            player.GetDamage(_damageCount);

            Rigidbody rigidbody = colliders[i].GetComponent<Rigidbody>();

            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _radius);
                Debug.Log("Boom!!!");
                _hasExploded = true;
            }
        }

        _currentTimeToDestroy += _timerToDestroy;

        if (_currentTimeToDestroy <= 0f)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(_r, _g, _b);
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}