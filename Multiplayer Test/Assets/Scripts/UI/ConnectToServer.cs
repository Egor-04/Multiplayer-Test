using UnityEngine.SceneManagement;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _sceneName = "Menu";
    [SerializeField] private Text _textVersion;
    [SerializeField] private RoomButton _roomButton; 

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = Application.version;
        _textVersion.text = "Current Game Version: " + PhotonNetwork.GameVersion;

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.IsConnected);
        PhotonNetwork.JoinLobby();
        SceneManager.LoadScene(_sceneName);
    }
}