using UnityEngine;
using Photon.Pun;

public class FirstAid : MonoBehaviour
{
    [Header("Heal Count")]
    [SerializeField] private float _healCount = 10f;

    [Header("Sounds")]
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _takeSound;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (collider.GetComponent<LocalPlayer>().PlayerHP < 100f)
            {
                collider.GetComponent<Player>().GetHeal(_healCount);
                GameObject instantiatedObject = PhotonNetwork.Instantiate(_source.name, collider.transform.position, Quaternion.identity);
                AudioSource instantiatedSource = instantiatedObject.GetComponent<AudioSource>();
                instantiatedSource.PlayOneShot(_takeSound);
                PhotonNetwork.Destroy(gameObject);
            }
        }  
    }
}
