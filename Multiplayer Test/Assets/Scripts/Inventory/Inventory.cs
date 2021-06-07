using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using System;

public class Inventory : MonoBehaviour
{
    [Header("Player Camera")]
    [SerializeField] private Camera _playerCamera;

    [Header("Ray Length")]
    [SerializeField] private float _rayLength = 20f;

    [Header("UI Inventory")]
    [SerializeField] private Transform _cellContainer;

    [Header("Buttons")]
    [SerializeField] private KeyCode _takeItem = KeyCode.E;
    [SerializeField] private KeyCode _dropItem = KeyCode.G;

    [Header("Items In Hand")]
    [SerializeField] private HandItems[] ItemsInHand;

    [Header("Inventory List")]
    [SerializeField] private List<Item> InventoryList;

    private PhotonView _photonView;

    [Serializable]
    public class HandItems
    {
        public int ID;
        public GameObject HandItem;
    }

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        InventoryList = new List<Item>();

        for (int i = 0; i < _cellContainer.childCount; i++)
        {
            InventoryList.Add(new Item() {ID = 0, Quantity = 0});
        }
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            if (Input.GetKeyDown(_takeItem))
            {
                Ray ray = new Ray(_playerCamera.transform.position, _playerCamera.transform.forward);
                RaycastHit hit; 

                if (Physics.Raycast(ray, out hit, _rayLength))
                {
                    if (hit.collider.GetComponent<Item>())
                    {
                        Item currentItem = hit.collider.GetComponent<Item>();
                        TakeItem(currentItem);
                    }
                }
            }
        }
    }

    private void TakeItem(Item currentItem)
    {
        if (!currentItem.IsStackable)
        {
            AddUnstackable(currentItem);
        }
        else
        {
            AddStackable(currentItem);
        }
    }

    private void AddUnstackable(Item currentItem)
    {
        for (int i = 0; i < InventoryList.Count; i++)
        {
            for (int j = 0; j < ItemsInHand.Length; j++)
            {
                if (InventoryList[i].ID == 0)
                {
                    if (currentItem.ActivateObjectInHand)
                    {
                        if (currentItem.ID == ItemsInHand[j].ID)
                        {
                            InventoryList[i] = currentItem;
                            ShowIcon();
                            ItemsInHand[j].HandItem.SetActive(true);
                            Debug.Log(ItemsInHand[j].HandItem.activeSelf);
                            PhotonNetwork.Destroy(currentItem.gameObject);
                            Debug.Log("||||||||||||||||||||||||||||| DESTROYED 1 ||||||||||||||||||||||||||||||||");
                            return;
                        }
                    }
                    else
                    {
                        InventoryList[i] = currentItem;
                        ShowIcon();
                        PhotonNetwork.Destroy(currentItem.gameObject);
                        Debug.Log("||||||||||||||||||||||||||||| DESTROYED 2 ||||||||||||||||||||||||||||||||");
                        return;
                    }
                }
            }
        }
    }

    private void AddStackable(Item currentItem)
    {
        for (int i = 0; i < InventoryList.Count; i++)
        {
            for (int j = 0; j < ItemsInHand.Length; j++)
            {
                if (InventoryList[i].ID == 0)
                {
                    if (currentItem.ActivateObjectInHand)
                    {
                        if (currentItem.ID == ItemsInHand[j].ID)
                        {
                            InventoryList[i] = currentItem;
                            ShowIcon();
                            ItemsInHand[j].HandItem.SetActive(true);
                            PhotonNetwork.Destroy(currentItem.gameObject);
                            Debug.Log("||||||||||||||||||||||||||||| DESTROYED ||||||||||||||||||||||||||||||||");
                            return;
                        }
                    }
                    else
                    {
                        InventoryList[i] = currentItem;
                        ShowIcon();
                        PhotonNetwork.Destroy(currentItem.gameObject);
                        Debug.Log("||||||||||||||||||||||||||||| DESTROYED ||||||||||||||||||||||||||||||||");
                        return;
                    }
                }
                else if (InventoryList[i].ID == currentItem.ID)
                {
                    InventoryList[i].Quantity += currentItem.Quantity;
                    ShowIcon();
                    PhotonNetwork.Destroy(currentItem.gameObject);
                    Debug.Log("||||||||||||||||||||||||||||| DESTROYED ||||||||||||||||||||||||||||||||");
                    return;
                }
            }
        }
    }

    private void ShowIcon()
    {
        for (int i = 0; i < InventoryList.Count; i++)
        {
            Transform Cell = _cellContainer.GetChild(i);
            Image Icon = Cell.GetChild(0).GetComponent<Image>();
            Text TextQuantity = Icon.transform.GetChild(0).GetComponent<Text>();

            if (InventoryList[i].ID != 0 && InventoryList[i].Quantity >= 1) 
            {
                Icon.sprite = InventoryList[i].Icon;
                Icon.enabled = true;

                if (!InventoryList[i].IsStackable)
                {
                    TextQuantity.enabled = false;
                }
                else
                {
                    TextQuantity.text = InventoryList[i].Quantity.ToString();
                    TextQuantity.enabled = true;
                }    
            }
            else
            {
                Icon.enabled = false;
                Icon.sprite = null;

                TextQuantity.enabled = false;
                TextQuantity.text = null;
            }
        }
    }
}
