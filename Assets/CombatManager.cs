using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager Instance { get; private set; }

    public event Action<DamageInfo> OnDamageReceived = delegate { };

    private void Awake()
    {
        Instance = this;
    }

    public void ReceiveDamageInfo(DamageInfo info)
    {
        OnDamageReceived(info);
    }
}

public class DamageInfo
{
    public uint targetId;
    public int damage;
}