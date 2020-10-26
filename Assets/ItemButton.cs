using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    public ItemsContainerId ContainerId { get; private set; }

    public void SetContainerId(ItemsContainerId id)
    {
        this.ContainerId = id;
    }

    public void Fill(ItemData data)
    {
        var grp = icon.GetComponentInChildren<CanvasGroup>();
        if (data != null)
        {
            grp.alpha = 1;
            grp.interactable = true;
        }
        else
        {
            grp.alpha = 0;
            grp.interactable = false;
        }
    }
}
