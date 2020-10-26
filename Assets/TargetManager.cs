using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public static TargetManager Instance { get; private set; }

    [SerializeField]
    private Transform targetDecal;

    private Character target;

    public event Action<Character> OnTargetChanged = delegate { };

    private void Awake()
    {
        Instance = this;
    }

    private void LateUpdate()
    {
        UpdateDecalPosition();
    }

    private void UpdateDecalPosition()
    {
        if (target != null)
        {
            targetDecal.transform.position = target.transform.position + new Vector3(0, 2);
        }
        else
        {
            if (targetDecal.gameObject.activeInHierarchy)
            {
                targetDecal.gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetTarget(null);
        }

        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 150))
            {
                var ch = hit.collider.GetComponent<Character>();
                if (ch)
                {
                    SetTarget(ch);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 150))
            {
                var ch = hit.collider.GetComponent<Character>();
                if (ch)
                {
                    SetTarget(ch);
                    OutcomingPackets.SendPacket(ServerPacketType.ClickNpcRequest, ch.id, (byte)1);
                }
            }
        }
    }

    private void SetTarget(Character ch)
    {
        target = ch;

        if (target == null)
        {
            targetDecal.gameObject.SetActive(false);
            OutcomingPackets.SendPacket(ServerPacketType.ClickNpcRequest, (uint)0, (byte)0);
        }
        else
        {
            targetDecal.gameObject.SetActive(true);
        }

        UpdateDecalPosition();
        OnTargetChanged(ch);
    }
}
