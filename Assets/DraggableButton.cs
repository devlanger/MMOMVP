using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDropHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int Slot { get; set; }

    private CanvasGroup group;
    private LayoutElement layout;

    public UnityEvent<PointerEventData> OnDrag;
    public UnityEvent<PointerEventData> OnDrop;
    public UnityEvent<PointerEventData> OnPickup;
    public UnityEvent<PointerEventData> OnRelease;
    public UnityEvent<PointerEventData> OnHover;
    public UnityEvent<PointerEventData> OnExitHover;
    public UnityEvent<PointerEventData> OnClick;

    public Vector3 PickupPosition { get; private set; }

    [SerializeField]
    private bool makeDraggable = false;

    public void ReturnToPickupPosition()
    {
        transform.position = PickupPosition;
    }

    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
        if (group == null)
        {
            group = gameObject.AddComponent<CanvasGroup>();
        }

        layout = GetComponent<LayoutElement>();
        if (layout == null)
        {
            layout = gameObject.AddComponent<LayoutElement>();
        }

        if (makeDraggable)
        {
            OnPickup.AddListener((ev) =>
            {
                SetInteractable(false);
            });

            OnRelease.AddListener((ev) =>
            {
                SetInteractable(true);
                ReturnToPickupPosition();
            });

            OnDrag.AddListener((ev) =>
            {
                transform.position = Input.mousePosition;
            });
        }
    }

    public void SetInteractable(bool interactable)
    {
        group.blocksRaycasts = interactable;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        PickupPosition = transform.position;
        OnPickup.Invoke(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        OnRelease.Invoke(eventData);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        OnDrag.Invoke(eventData);
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        OnDrop.Invoke(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover.Invoke(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnExitHover.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke(eventData);
    }
}
