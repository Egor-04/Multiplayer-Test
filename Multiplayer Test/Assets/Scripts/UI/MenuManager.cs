using System.Collections.Generic;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;
using System;

public class MenuManager : MonoBehaviourPunCallbacks
{
    [Header("Create Room & Join Room")]
    [SerializeField] private string NameOfGameScene = "Game";
    [SerializeField] private InputField _inputNameToCreate;
    [SerializeField] private InputField _inputNameToAvailableRoom;
    [SerializeField] private Slider _maxPlayersCount;
    [SerializeField] private Toggle _isOpen;
    [SerializeField] private Toggle _isVisible;

    [Header("Room List")]
    [SerializeField] private RoomList _roomList;

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)_maxPlayersCount.value;
        roomOptions.IsOpen = _isOpen.isOn;
        roomOptions.IsVisible = _isVisible.isOn;
        PhotonNetwork.CreateRoom(_inputNameToCreate.text, roomOptions);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_inputNameToAvailableRoom.text);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(NameOfGameScene);
    }
}
