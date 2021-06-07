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

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<Player>().GetDamage(_damageCount);
            Debug.Log("Blyyyyaaaaaaaat");
            //PhotonNetwork.Destroy(gameObject);
        }

        //if (collider)
        //{
        //    PhotonNetwork.Destroy(gameObject);
        //}
    }
}