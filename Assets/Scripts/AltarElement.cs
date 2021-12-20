using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class AltarElement : MonoBehaviour, IPointerDownHandler
{
    public ElementType type;
    public AltarSlot currentAltarSlot;

    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponent<Tweener>().SetOrigin();
        PrepareAltar.instance.HoldElement(this);
    }


    public void SetTarget(Transform newTarget)
    {
        GetComponent<Tweener>().UpdateTarget(newTarget);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("AltarSlot"))
        {
            Debug.Log("Slot in");

            //if (other.GetComponent<AltarSlot>().currentElement != null)
            //    return;

            currentAltarSlot = other.GetComponent<AltarSlot>();
            //currentAltarSlot.CheckIfCorrect(type);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("AltarSlot"))
        {
            Debug.Log("Slot out");
            
            
            if (currentAltarSlot != null)
            {
                //other.GetComponent<AltarSlot>().ResetSpriterenderer(this);
                currentAltarSlot.CheckoutSlot(this);
                currentAltarSlot = null;
            }

            PrepareAltar.instance.CheckIfDone();
        }

    }

    public void TryLockin(bool force = false)
    {
        Debug.Log("Try lockin");

        if (currentAltarSlot == null)
            return;

        if (currentAltarSlot.currentElement != null && currentAltarSlot.currentElement != this && !force)
        {
            Debug.Log("Altar slot taken");
            GetComponent<Tweener>().ToOrigin();
            return;
        }

        currentAltarSlot.CheckinSlot(this);

    }

    public void ForceLockin(AltarSlot slot)
    {
        currentAltarSlot = slot;
        TryLockin(true);
    }
    public bool CheckIfHasSlot()
    {
        Debug.Log(currentAltarSlot);
        return currentAltarSlot != null;
    }

    public bool CheckIfCorrect()
    {
        return type == currentAltarSlot.type;
    }
}

public enum ElementType
{
    candleCenter,
    candleBlue,
    candleRed,
    candleGreen,
    candleYellow,
    statue
}
