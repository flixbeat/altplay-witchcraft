using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool_PreparingTheBox : MonoBehaviour
{
    public static Tool_PreparingTheBox instance;
    [SerializeField] private bool hasPickedUp;
    [SerializeField] private Transform foamTweenerTarget;
    [SerializeField] private Transform mortar;
    [SerializeField] private ParticleSystem particleFX;
    [SerializeField] private IngredientUI[] ingredientsUI;
    [SerializeField] private Transform ingredientsUIParent;
    [SerializeField] private Recap recapScript;
    [SerializeField] private GameObject loading;
	
	[SerializeField] private Transform foreFingertip, thumbTip;
    [SerializeField] private GameObject feedbackUI;
    [SerializeField] private Transform toolPositionOnDrop;
    [SerializeField] private GameObject nextButton;
    [SerializeField] public Tweener toolTweener;
    [SerializeField] private Transform fxParent;
    public BoxFoamContainer currentContainer;

    private Animator anim;
    public bool canDrop, pickUpAutomaticOnProgress, checkWrongUI;
    private BoxFoamContainer pickedUpObjectOriginContainer;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (anim == null)
            anim = GetComponentInChildren<Animator>();

        toolTweener.SetSpeed(7.5f);
    }

    private void Update()
    {
        //if (Input.GetMouseButtonUp(0))
        //{
        //    if (hasPickedUp && canDrop)
        //        Drop();
        //}
    }
	
	void LateUpdate()
    {
		foamTweenerTarget.position = Vector3.Lerp(foreFingertip.position, thumbTip.position, 0.5f);
	}
	
    public void ToggleLoading(bool isActive)
    {
        if (hasPickedUp && isActive)
            return;

        loading.SetActive(isActive);
        //StartCoroutine(ToggleLoadingRoutine(isActive));
    }

    private IEnumerator ToggleLoadingRoutine(bool isActive)
    {
        loading.SetActive(!isActive);
        yield return new WaitForEndOfFrame();
        loading.SetActive(isActive);
    }

    public void PickupAutomatic(List<Tweener> foamTweeners, BoxFoamContainer container)
    {
        StartCoroutine(PickupAutomaticRoutine(foamTweeners, container));
    }

    public IEnumerator PickupAutomaticRoutine(List<Tweener> foamTweeners, BoxFoamContainer container)
    {
        if (pickUpAutomaticOnProgress)
            yield break;

        pickUpAutomaticOnProgress = true;
        toolTweener.UpdateTarget(container.handTweenerPos);
        yield return new WaitForSeconds(0.3f);
        Pickup(foamTweeners, container);
        yield return new WaitForSeconds(0.3f);
        toolTweener.UpdateTarget(toolPositionOnDrop);
        yield return new WaitForSeconds(0.3f);
        Drop();
        pickUpAutomaticOnProgress = false;
    }


    public void Pickup(List<Tweener> foamTweeners, BoxFoamContainer container)
    {
        if (hasPickedUp)
            return;

        ToggleLoading(false);

        foreach(Tweener t in foamTweeners)
        {
            t.GetComponent<Rigidbody>().isKinematic = true;
            t.GetComponent<Collider>().isTrigger = true;
            t.SetOrigin();
            t.UpdateTarget(foamTweenerTarget);
            t.transform.parent = foamTweenerTarget;
        }

        anim.SetTrigger("pickup");
        pickedUpObjectOriginContainer = container;
        hasPickedUp = true;
    }

    private void Drop()
    {
        hasPickedUp = false;
        anim.SetTrigger("drop");
        
		Transform tempFoam;
		
        tempFoam = foamTweenerTarget.GetChild(0);
            
		bool isCorrect = UpdateUI(tempFoam);

        if (isCorrect)
        {
            //particleFX.Play();
            tempFoam.GetComponent<Rigidbody>().isKinematic = false;
            tempFoam.GetComponent<Rigidbody>().drag = 0.1f;
            tempFoam.GetComponent<Collider>().isTrigger = false;
            tempFoam.GetComponent<Tweener>().UpdateTarget(null);
            tempFoam.parent = mortar;
            fxParent.GetChild(Random.Range(0, fxParent.childCount)).GetComponent<ParticleSystem>().Play();
        }
        else
        {
            tempFoam.GetComponent<Tweener>().ToOrigin();
            tempFoam.parent = pickedUpObjectOriginContainer.foamParent.transform;
            tempFoam.SetAsLastSibling();
            Debug.Log("INCORRECT!!");
        }

        StartCoroutine(ShowFeedbackRoutine(isCorrect));
		UnparentChildren(tempFoam);
        CheckIfDone();
    }
	
    private IEnumerator ShowFeedbackRoutine(bool isCorrect)
    {
        feedbackUI.SetActive(false);
        feedbackUI.transform.GetChild(0).gameObject.SetActive(isCorrect);
        feedbackUI.transform.GetChild(1).gameObject.SetActive(!isCorrect);
        yield return new WaitForEndOfFrame();
        feedbackUI.SetActive(true);

    }

    void UnparentChildren(Transform current){
		for(int i = 0; i < current.childCount; i++){
			var child = current.GetChild(i);
			
			if(child.gameObject.activeSelf){
				child.SetParent(current.parent);
				
				var rb = child.GetComponent<Rigidbody>(); // meh
				if(rb) rb.isKinematic = false;
			}
		}
	}
	
     bool UpdateUI(Transform tempFoam){
		var type = tempFoam.GetComponent<Ingredient>().ingredientType;

        //foreach(var ui in ingredientsUI)
        //	ui.Check(type);
        bool isCorrect = false;
        checkWrongUI = true;

        for (int i = 1; i < ingredientsUIParent.childCount; i++)
        {
            if (ingredientsUIParent.GetChild(i).GetComponent<IngredientUI>().Check(type))
            {
                isCorrect = true;
            }
        }

        //if (!isCorrect)
        //{
        //    for (int i = 1; i < ingredientsUIParent.childCount; i++)
        //    {
        //        if (!ingredientsUIParent.GetChild(i).GetComponent<IngredientUI>().IsDone)
        //        {
        //            ingredientsUIParent.GetChild(i).GetComponent<IngredientUI>().ShowWrongMark();
        //            break;
        //        }
        //    }

        //}

        return isCorrect;
	}

    private void CheckIfDone()
    {
        for (int i = 1; i < ingredientsUIParent.childCount; i++)
        {
            if (!ingredientsUIParent.GetChild(i).GetComponent<IngredientUI>().IsDone)
                return;
        }

        Invoke(nameof(Next), 1);
        //nextButton.SetActive(true);
    }
	
    private void Next()
    {
        StepsManager.instance.NextStep();
    }

	public void CheckWinConditions(){

		bool isWin = true;
        for (int i = 1; i < ingredientsUIParent.childCount; i++)
        {
            var ui = ingredientsUIParent.GetChild(i).GetComponent<IngredientUI>();
            if (!ui.IsDone)
            {
                isWin = false;
                break;
            }
        }

        if (!isWin)
            recapScript.mistakes++;
	}
}
