using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

public class RoomList : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform _content;
    [SerializeField] private RoomButton _roomButtonScript;

    [SerializeField] private List<RoomInfo> _roomInfo;

    public override void OnLeftRoom()
    {
        Debug.Log("Player left");
        for (int i = 0; i < _roomInfo.Count; i++)
        {
            if (_roomInfo[i].Name == _roomButtonScript.RoomName.text)
            {
                Destroy(_roomButtonScript.gameObject);
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        _roomInfo = roomList;

        for (int i = 0; i < roomList.Count; i++)
        {
            RoomButton roomButton = Instantiate(_roomButtonScript, _content);

            if (roomButton != null)
            {
                roomButton.SetRoomInfo(roomList[i]);
            }
        }
    }
}
