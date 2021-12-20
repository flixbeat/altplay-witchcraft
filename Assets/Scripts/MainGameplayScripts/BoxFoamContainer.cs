using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxFoamContainer : MonoBehaviour, IPointerClickHandler
{
    public Transform foamParent;
    public Transform handTweenerPos;
    [SerializeField] private int foamNumberPerPickup = 10;

    private Tool_PreparingTheBox toolScript;
    private List<Tweener> pickedUpFoamTweeners = new List<Tweener>();
    Coroutine grabRoutine;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"Pickup {gameObject.name}");


        pickedUpFoamTweeners.Clear();
        for (int i = 0; i < foamNumberPerPickup && i < foamParent.childCount; i++)
        {
            Debug.Log(i);
            if (foamParent.GetChild(i).TryGetComponent(out Tweener t))
            {
                Debug.Log("add");
                Debug.Log(t);
                Debug.Log(pickedUpFoamTweeners);
                pickedUpFoamTweeners.Add(t);
            }

        }

        Tool_PreparingTheBox.instance.PickupAutomatic(pickedUpFoamTweeners, this);
        Debug.Log(Tool_PreparingTheBox.instance);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.TryGetComponent(out Tool_PreparingTheBox toolScript))
    //    {
    //        this.toolScript = toolScript;
    //        toolScript.currentContainer = this;

    //        if (grabRoutine != null)
    //            StopCoroutine(grabRoutine);

    //        grabRoutine = StartCoroutine(GrabProducts());
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.TryGetComponent(out Tool_PreparingTheBox toolScript))
    //    {

    //        if (grabRoutine != null)
    //            StopCoroutine(grabRoutine);

    //        if (toolScript.currentContainer == this)
    //            toolScript.ToggleLoading(false);
    //    }
    //}

    IEnumerator GrabProducts()
    {

        if (toolScript.currentContainer != this)
            yield break;

        toolScript.ToggleLoading(false);
        yield return new WaitForEndOfFrame();
        toolScript.ToggleLoading(true);

        yield return new WaitForSeconds(0.75f);

        if (toolScript.currentContainer != this)
        {
            //toolScript.ToggleLoading(false);
            yield break;
        }

        pickedUpFoamTweeners.Clear();
        for (int i = 0; i < foamNumberPerPickup && i < foamParent.childCount; i++)
        {
            pickedUpFoamTweeners.Add(foamParent.GetChild(i).GetComponent<Tweener>());
        }

        toolScript.Pickup(pickedUpFoamTweeners, this);
    }
}
