using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class slot : MonoBehaviour, IBeginDragHandler, IDragHandler,  IDropHandler
{
    public bool onHand = false;
    public inventoryAccess invAc;
    public void OnBeginDrag(PointerEventData eventData)
    {
        invAc.Drag(transform.GetSiblingIndex(), transform.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(onHand)
            invAc.releaseOnHand();
    }

    // Start is called before the first frame update
    void Start()
    {
        invAc = GameObject.FindObjectOfType<inventoryAccess>();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(onHand){
            transform.position = Input.mousePosition;
        }
    }
}
