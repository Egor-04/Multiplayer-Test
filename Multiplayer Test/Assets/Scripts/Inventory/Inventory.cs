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
            if (InventoryList[i].ID == 0)
            {
                Debug.Log(currentItem.ID + " - " + ItemsInHand[i].ID);

                if (currentItem.ActivateObjectInHand)
                {
                    if (currentItem.ID == ItemsInHand[i].ID)
                    {
                        InventoryList[i] = currentItem;
                        ItemsInHand[i].HandItem.SetActive(true);
                        ShowIcon();
                        Destroy(currentItem.gameObject);
                        Debug.Log("||||||||||||||||||||||||||||| DESTROYED ||||||||||||||||||||||||||||||||");
                        break;
                    }
                }
                else
                {
                    InventoryList[i] = currentItem;
                    ShowIcon();
                    Destroy(currentItem.gameObject);
                    Debug.Log("||||||||||||||||||||||||||||| DESTROYED ||||||||||||||||||||||||||||||||");
                    break;
                }
            }
        }
    }

    private void AddStackable(Item currentItem)
    {
        for (int i = 0; i < InventoryList.Count; i++)
        {
            if (InventoryList[i].ID == 0)
            {
                InventoryList[i] = currentItem;

                if (InventoryList[i].ActivateObjectInHand)
                {
                    if (InventoryList[i].ID == ItemsInHand[i].ID)
                    {
                        ItemsInHand[i].HandItem.SetActive(true);
                    }
                }

                ShowIcon();
                Destroy(gameObject.gameObject);
                Debug.Log("||||||||||||||||||||||||||||| DESTROYED ||||||||||||||||||||||||||||||||");
                break;
            }
            else if (InventoryList[i].ID == currentItem.ID)
            {
                InventoryList[i] = currentItem;
                InventoryList[i].Quantity += currentItem.Quantity;
                ShowIcon();
                Destroy(currentItem.gameObject);
                Debug.Log("||||||||||||||||||||||||||||| DESTROYED ||||||||||||||||||||||||||||||||");
                break;
            }
        }
    }

    private void ShowIcon()
    {
        for (int i = 0; i < _cellContainer.childCount; i++)
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
                    TextQuantity.enabled = transform;
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
