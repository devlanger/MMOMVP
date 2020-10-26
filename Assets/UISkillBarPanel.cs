using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillBarPanel : MonoBehaviour
{
    private Dictionary<int, DraggableButton> button = new Dictionary<int, DraggableButton>();

    private void Awake()
    {
        var bts = transform.GetComponentsInChildren<DraggableButton>();
        for (int i = 0; i < bts.Length; i++)
        {
            button.Add(i, bts[i]);

            int slot = i;

            bts[i].Slot = slot;
            bts[i].OnDrop.AddListener((e) => {
                Debug.Log(e.pointerDrag);
                var skillHandler = e.pointerDrag.GetComponentInParent<SkillButton>();
                if (skillHandler != null)
                {
                    Debug.Log("Set skill bar slot: " + slot + " to " + skillHandler.skillData.name);

                    //var dragged = e.pointerDrag.GetComponent<DraggableButton>();
                    OutcomingPackets.SendPacket(ServerPacketType.MoveEntitySlot, MoveEntityType.SKILLBAR, slot);
                }
                else
                {
                    Debug.Log("There is no skill button component in parent.");
                }
            });
        }
    }

    private void Start()
    {
        PlayerController.Instance.OnLocalPlayerChanged += Instance_OnLocalPlayerChanged;
    }

    private void Instance_OnLocalPlayerChanged(Character obj)
    {
        obj.skills[SkillsContainerId.SKILLBAR].InventoryChanged += ItemsContainerButtons_InventoryChanged;
    }

    private void ItemsContainerButtons_InventoryChanged(ItemsContainer<SkillsContainerId, SkillData> obj)
    {
        for (int i = 0; i < button.Count; i++)
        {
            if (obj.Items.ContainsKey(i))
            {
                button[i].GetComponentInParent<SkillButton>().Fill(obj.Items[i]);
            }
            else
            {
                button[i].GetComponentInParent<SkillButton>().Fill(null);
            }
        }
    }
}
