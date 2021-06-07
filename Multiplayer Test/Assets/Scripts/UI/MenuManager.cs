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

    [Header("Room List")]
    [SerializeField] private Transform _content;
    [SerializeField] private Button _buttonPrefab;

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = (byte)_maxPlayersCount.value;
        PhotonNetwork.CreateRoom(_inputNameToCreate.text, roomOptions);

        //Button buttonPrefab = Instantiate(_buttonPrefab, _content);
        //_content.SetParent(buttonPrefab.transform);

        //Text textRoomName = buttonPrefab.transform.GetChild(0).GetComponent<Text>();
        //textRoomName.text = _inputNameToCreate.text;
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
