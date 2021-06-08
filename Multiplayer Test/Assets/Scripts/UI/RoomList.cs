using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using Photon.Pun;

public class RoomList : MonoBehaviourPunCallbacks
{
    [Header("For Room List")]
    [SerializeField] private Transform _content;
    [SerializeField] private RoomButton _roomButtonScript;

    public override void OnRoomListUpdate(List<RoomInfo> roomInfoList)
    {
        for (int i = 0; i < roomInfoList.Count; i++)
        {
            RoomButton roomButton = Instantiate(_roomButtonScript, _content);

            if (roomButton != null)
            {
                roomButton.SetRoomInfo(roomInfoList[i]);
            }

            if (roomInfoList[i].RemovedFromList)
            {
                Destroy(roomButton.gameObject);
            }
        }
    }
}
