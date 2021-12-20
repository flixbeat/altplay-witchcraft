using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarSlot : MonoBehaviour
{
    public ElementType type;
    public AltarElement currentElement;
    public void CheckoutSlot(AltarElement element)
    {
        if (currentElement != element)
            return;

        currentElement = null;
        ResetSpriterenderer();
    }

    public void CheckinSlot(AltarElement element)
    {
        currentElement = element;
        element.SetTarget(transform);

        if (currentElement.type == type)
        {
            Debug.Log("Correct!!");
            GetComponentInChildren<ParticleSystem>().Play();
            GetComponent<Collider>().enabled = false;

            foreach (Collider c in currentElement.GetComponentsInChildren<Collider>())
                c.enabled = false;
        }

        PrepareAltar.instance.CheckIfDone();
    }

    public void ResetSpriterenderer(AltarElement element = null)
    {
        if (element != null && element != currentElement && currentElement != null)
            return;

        GetComponentInChildren<SpriteRenderer>().color = Color.white;

        GetComponentInChildren<Animator>().SetTrigger("off");

    }

    public void CompareElement(ElementType elementType)
    {
        GetComponentInChildren<SpriteRenderer>().color = elementType == type ? Color.green : Color.white;

        GetComponentInChildren<Animator>().SetTrigger(elementType == type ? "on" : "off");
    }

    public void CheckIfCorrect(ElementType element)
    {
        if (currentElement != null)
            return;

        GetComponentInChildren<SpriteRenderer>().color = element == type ? Color.green : Color.white;
        GetComponentInChildren<Animator>().SetTrigger(element == type ? "on" : "off");
    }


}
