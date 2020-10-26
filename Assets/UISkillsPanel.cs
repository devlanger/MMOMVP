using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillsPanel : MonoBehaviour
{
    private Dictionary<int, DraggableButton> button;

    private void Awake()
    {
        var bts = transform.GetComponentsInChildren<DraggableButton>();
        foreach (var item in bts)
        {
            item.OnDrop.AddListener((e) => {

            });

            item.OnDrag.AddListener((e) =>
            {
                item.transform.position = Input.mousePosition;
            });

            item.OnRelease.AddListener((e) =>
            {
                item.ReturnToPickupPosition();
            });
        }
    }
}
