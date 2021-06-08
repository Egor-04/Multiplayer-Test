using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;
using System;

public class RoomList : MonoBehaviourPunCallbacks
{
    [Header("For Room List")]
    [SerializeField] private Transform _content;
    [SerializeField] private RoomButton _roomButtonScript;

    public override void OnRoomListUpdate(List<RoomInfo> roomInfoList)
    {
        for (int i = 0; i < roomInfoList.Count; i++)
        {
            if (roomInfoList[i].PlayerCount > 0)
            {
                RoomButton roomButton = Instantiate(_roomButtonScript, _content);
                
                if (roomButton != null && roomInfoList[i].PlayerCount >= 1)
                {
                    roomButton.SetRoomInfo(roomInfoList[i]);
                }
            }
        }
    }
}
