using Photon.Pun;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int ID;
    public string NameItem;
    public int Quantity = 1;
    public Sprite Icon;
    public AudioClip TakeSound;
    public bool IsStackable;

    [Header("Drop Item Prefab")]
    public GameObject DropItem;

    [Header("Object In Hand")]
    public bool ActivateObjectInHand;

    [Header("For Bullet")]
    public bool IsBullet;
    public float Calliber;
    [TextArea] public string Description;

    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }
}
