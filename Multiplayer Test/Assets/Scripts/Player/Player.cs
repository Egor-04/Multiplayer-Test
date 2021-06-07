using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviourPunCallbacks
{
    //[SerializeField] private string _menuSceneName = "Menu";
    [SerializeField] private LocalPlayer _localPlayer;
    private PhotonView _photonView;

    private void Awake()
    {
        _localPlayer = GetComponent<LocalPlayer>();
        _photonView = GetComponent<PhotonView>();


        //Благодаря этой строчке стало работать чуть лучше. Теперь можно видеть игрока, но повороты и движение не синхронизированны до конца.
        if (!_photonView.IsMine)
        {
            Destroy(_localPlayer.PlayerCanvas);
            Destroy(_localPlayer.PlayerCamera.gameObject);
        }
    }

    private void Update()
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (_photonView.IsMine)
        {
            if (_localPlayer.PlayerHP <= 0f)
            {
                _localPlayer.PlayerHP = 0f;
                _localPlayer.IsActive = false;

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                PhotonNetwork.Destroy(_localPlayer.gameObject);
            }
            else if (_localPlayer.PlayerHP >= 100f)
            {
                _localPlayer.PlayerHP = 100f;
            }
        }
    }

    public void GetHeal(float healCount)
    {
        if (_photonView.IsMine)
        {
            _localPlayer.PlayerHP += healCount;
        }
    }

    [PunRPC]
    public void GetDamage(float damageCount)
    {
        if (_photonView.IsMine)
        {
            _localPlayer.PlayerHP -= damageCount;
        }
    }
}
