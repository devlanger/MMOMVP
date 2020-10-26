using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITargetPanel : MonoBehaviour
{
    private UIPanel panel;

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Slider healthBar;

    private Character lastTarget;

    private void Start()
    {
        panel = GetComponent<UIPanel>();
        TargetManager.Instance.OnTargetChanged += Instance_OnTargetChanged;
    }

    private void Instance_OnTargetChanged(Character obj)
    {
        if(lastTarget != null)
        {
            lastTarget.Stats.OnStatChanged -= Stats_OnStatChanged;
        }

        if(obj == null)
        {
            panel.Deactivate();
        }
        else
        {
            obj.Stats.OnStatChanged += Stats_OnStatChanged;
            foreach (var item in obj.Stats.stats)
            {
                Stats_OnStatChanged(0, item.Key, item.Value);
            }
            panel.Activate();
        }

        lastTarget = obj;
    }

    private void Stats_OnStatChanged(uint arg1, StatType arg2, object arg3)
    {
        switch(arg2)
        {
            case StatType.NAME:
                nameText.text = (string)arg3;
                break;
            case StatType.HEALTH:
                healthBar.value = (int)arg3;
                break;
        }
    }
}
