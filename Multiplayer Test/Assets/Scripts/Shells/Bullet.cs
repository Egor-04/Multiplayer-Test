using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    [SerializeField] private PhotonView _photonView;
    [SerializeField] private float _damageCount;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().GetDamage(_damageCount);
        }

        if (collision.collider)
        {
            Destroy(gameObject);
        }
    }
}