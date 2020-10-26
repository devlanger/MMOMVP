using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDamageLabels : MonoBehaviour
{
    [SerializeField]
    private Text damageLabelPrefab;

    private List<DamageLabel> labels = new List<DamageLabel>();

    private void Start()
    {
        CombatManager.Instance.OnDamageReceived += Instance_OnDamageReceived;
    }

    private void Instance_OnDamageReceived(DamageInfo info)
    {
        var character = MobsManager.Instance.GetCharacter(info.targetId);
        if(character == null)
        {
            return;
        }
        DamageLabel dl = new DamageLabel();

        Text inst = Text.Instantiate(damageLabelPrefab, transform);
        inst.text = info.damage.ToString();

        dl.target = character;
        dl.label = inst;

        dl.UpdateTarget();

        Destroy(inst, 1);

        labels.Add(dl);
    }

    public class DamageLabel
    {
        public Character target;
        public Text label;

        public void UpdateTarget()
        {
            label.transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        }
    }
}
