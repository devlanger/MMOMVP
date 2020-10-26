using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemsContainerButtons : MonoBehaviour
{
    public ItemsContainerId containerId;

    private Dictionary<int, ItemButton> buttons = new Dictionary<int, ItemButton>();

    private void Awake()
    {
        var bts = GetComponentsInChildren<ItemButton>();
        for (int i = 0; i < bts.Length; i++)
        {
            var slot = i;
            bts[i].SetContainerId(containerId);
            buttons.Add(i, bts[i]);

            var draggable = bts[i].GetComponentInChildren<DraggableButton>();
            draggable.Slot = slot;

            draggable.OnDrag.AddListener((e) =>
            {
                draggable.transform.position = Input.mousePosition;
            });

            draggable.OnPickup.AddListener((e) =>
            {
                draggable.SetInteractable(false);
            });

            draggable.OnRelease.AddListener((e) =>
            {
                draggable.SetInteractable(true);
                draggable.ReturnToPickupPosition();
            });

            draggable.OnClick.AddListener((e) =>
            {
                if(e.button == PointerEventData.InputButton.Right)
                {
                    OutcomingPackets.SendPacket(ServerPacketType.UseItem, containerId, (byte)1, slot);
                }
            });

            draggable.OnDrop.AddListener((e) =>
            {
                var dragged = e.pointerDrag.GetComponent<DraggableButton>();
                OutcomingPackets.SendPacket(ServerPacketType.MoveEntitySlot, MoveEntityType.ITEM, slot, containerId, draggable.GetComponentInParent<ItemsContainerButtons>().containerId, dragged.Slot);
            });
        }
    }

    private void Start()
    {
        PlayerController.Instance.OnLocalPlayerChanged += Instance_OnLocalPlayerChanged;
    }

    private void Instance_OnLocalPlayerChanged(Character obj)
    {
        obj.containers[containerId].InventoryChanged += ItemsContainerButtons_InventoryChanged;   
    }

    private void ItemsContainerButtons_InventoryChanged(ItemsContainer<ItemsContainerId, ItemData> obj)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if(obj.Items.ContainsKey(i))
            {
                buttons[i].Fill(obj.Items[i]);
            }
            else
            {
                buttons[i].Fill(null);
            }
        }
    }
}
