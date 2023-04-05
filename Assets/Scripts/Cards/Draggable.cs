using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Transform MyParent;
    public bool isEnabled = true;

    private void OnEnable()
    {
        isEnabled = true;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    private void OnDisable()
    {
        isEnabled = false;
    }
    public void OnBeginDrag(PointerEventData _eventData)
    {
        MyParent = transform.parent;
        transform.SetParent(transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        Debug.Log("start drag");
    }
    public void OnDrag(PointerEventData _eventData)
    {
        transform.position = _eventData.position;
    }
    public void OnEndDrag(PointerEventData _eventData)
    {
        Debug.Log("end drag");
        transform.SetParent(MyParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        Card _card = eventData.pointerDrag.GetComponent<Card>();
        if (_card != null && !_card.isACombinedCard && !gameObject.GetComponent<Card>().isACombinedCard && !CombineManager.Instance.CombinedThisTurn)
        {
            CombineManager.Instance.CombineCards(_card, gameObject.GetComponent<Card>());
        }
    }
}
