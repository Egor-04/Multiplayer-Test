using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

public class RoomButton : MonoBehaviourPunCallbacks
{
    [Header("Room Info")]
    public Text RoomName;
    public Text PlayerCount;
    public Text MaxPlayers;
    public Toggle IsOpen;

    public void EnterInThisRoom()
    {
        PhotonNetwork.JoinRoom(RoomName.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        RoomName.text = "Room Name: " + roomInfo.Name;
        PlayerCount.text = "Current Player Count: " + roomInfo.PlayerCount.ToString();
        MaxPlayers.text = "Max Players Count: " + roomInfo.MaxPlayers.ToString();
        IsOpen.isOn = roomInfo.IsOpen;
    }
}
