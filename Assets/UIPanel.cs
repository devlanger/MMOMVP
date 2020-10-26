using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIPanel : MonoBehaviour
{
    private CanvasGroup grp;
    public bool IsActive { get; private set; }

    private void Awake()
    {
        EnsureCanvasGroupExists();
    }

    public void EnsureCanvasGroupExists()
    {
        grp = GetComponent<CanvasGroup>();
    }

    [ContextMenu("Activate")]
    public void Activate() { EnsureCanvasGroupExists(); grp.alpha = 1; grp.blocksRaycasts = true; IsActive = true; }
    [ContextMenu("Deactivate")]
    public void Deactivate() { EnsureCanvasGroupExists(); grp.alpha = 0; grp.blocksRaycasts = false; IsActive = false; }
    public void Toggle() { if (IsActive) Deactivate(); else Activate(); }
}
