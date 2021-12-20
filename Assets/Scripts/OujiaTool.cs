using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OujiaTool : MonoBehaviour
{
    [SerializeField] private string wholeSentence;
    [SerializeField] private string currentLetter;
    [SerializeField] private TalkToSpirit stepScript;
    [SerializeField] private FreeMovementDrag toolTarget;
    [SerializeField] private Transform toolOriginPos;
    [SerializeField] private GameObject hint;
    [SerializeField] private Animator toolAnimator;
    [SerializeField] private SpiritAnswerSentence spiritAnswerScript;
    [SerializeField] private Image skipButton;

    private Transform currentCorrectLetter;
    private float currentDistanceToCorrectLetter;

    private Coroutine setCurrentLetterRoutine;
    private bool isActivated;

    private void Update()
    {
        if (currentCorrectLetter != null)
        {
            currentDistanceToCorrectLetter = Vector3.Distance(currentCorrectLetter.position, transform.position);

            if (currentDistanceToCorrectLetter <= 0.6f)
                toolAnimator.SetFloat("vibrateSpeed", 4);
            else if (currentDistanceToCorrectLetter <= 0.8f)
                toolAnimator.SetFloat("vibrateSpeed", 2);
            else if (currentDistanceToCorrectLetter <= 1.2f)
                toolAnimator.SetFloat("vibrateSpeed", 1);
            else
                toolAnimator.SetFloat("vibrateSpeed", 0);
        }

        if (Input.touchCount > 0)
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                skipButton.raycastTarget = false;

            if (Input.GetMouseButtonUp(0))
                skipButton.raycastTarget = true;
        }    

    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isActivated || !other.CompareTag("OujiaLetters"))
            return;

        if (other.GetComponent<OujiaLetter>().isCorrect && !OujiaLevelConfigurator.instance.levelConfiguration.isTutorial)
            other.GetComponent<Animator>().SetTrigger("on");

        currentLetter = other.name;
        if (setCurrentLetterRoutine != null)
            StopCoroutine(setCurrentLetterRoutine);

        setCurrentLetterRoutine = StartCoroutine(SetCurrentLetter());
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("OujiaLetters"))
            return;

        if (setCurrentLetterRoutine != null)
        {
            StopCoroutine(setCurrentLetterRoutine);
            hint.SetActive(false);
            ResetColliderRoutine();
        }

        if (other.GetComponent<OujiaLetter>().isCorrect && !OujiaLevelConfigurator.instance.levelConfiguration.isTutorial)
            other.GetComponent<Animator>().SetTrigger("idle");
    }

    private IEnumerator SetCurrentLetter()
    {
        yield return new WaitForSeconds(0.5f);
        hint.SetActive(true);
        yield return new WaitForSeconds(1);
        hint.SetActive(false);
        wholeSentence = $"{wholeSentence}{currentLetter}";
        stepScript.CheckLatestLetterIfCorrect(currentLetter);
        stepScript.UpdateCurrentSentence(wholeSentence);
    }

    public void AddSpaceToSentence()
    {
        // add space
        spiritAnswerScript.RemoveBlocker(wholeSentence.Length);
        wholeSentence = $"{wholeSentence} ";
        spiritAnswerScript.RemoveBlocker(wholeSentence.Length);
        stepScript.UpdateCurrentSentence(wholeSentence);
    }

    public void ResetSentence()
    {
        wholeSentence = "";
        currentLetter = "";
    }

    public void SetTrackedLetter(Transform letter)
    {
        currentCorrectLetter = letter;

        if (letter == null)
            toolAnimator.SetFloat("vibrateSpeed", 0);

    }

    public void ToggleActivate(bool isActivated)
    {
        StopAllCoroutines();
        this.isActivated = isActivated;
        toolTarget.enabled = isActivated;

        if (!isActivated)
        {
            toolTarget.transform.position = toolOriginPos.transform.position;
            StartCoroutine(SlowDownTweenerRoutine());
        }
    }

    private IEnumerator SlowDownTweenerRoutine()
    {
        GetComponent<Tweener>().SetSpeed(5);
        yield return new WaitForSeconds(1.5f);
        GetComponent<Tweener>().SetSpeed(20);
    }

    public IEnumerator ResetColliderRoutine()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForEndOfFrame();
        GetComponent<Collider>().enabled = true;
    }
}
