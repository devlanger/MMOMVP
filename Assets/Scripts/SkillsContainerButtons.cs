using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillsContainerButtons : MonoBehaviour
{
    public SkillsContainerId id;
    private Dictionary<int, SkillButton> buttons = new Dictionary<int, SkillButton>();

    private void Start()
    {
        var bts = GetComponentsInChildren<SkillButton>();
        for (int i = 0; i < bts.Length; i++)
        {
            buttons.Add(i, bts[i]);
        }

        PlayerController.Instance.OnLocalPlayerChanged += Instance_OnLocalPlayerChanged;
    }

    private void Instance_OnLocalPlayerChanged(Character obj)
    {
        obj.skills[id].InventoryChanged += ItemsContainerButtons_InventoryChanged;
    }

    private void ItemsContainerButtons_InventoryChanged(ItemsContainer<SkillsContainerId, SkillData> obj)
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (obj.Items.ContainsKey(i))
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
