using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cinemachine;

public class TalkToSpirit : Step
{
    [Header("Configurations")]
    [SerializeField] private OujiaLevelConfigurator configurator;
    [SerializeField] private Conversation[] conversations;
    [SerializeField] private int currentConversationIndex;
    [SerializeField] private string currentCorrectSentence;
    [SerializeField] private float speechSpeed;

    [Header("Scene references")]
    [SerializeField] private TextMeshProUGUI spiritCurrentSentenceText;
    [SerializeField] private SpiritAnswerSentence spiritSentence;
    [SerializeField] private Animator sentenceAnimator;
    [SerializeField] private OujiaTool stepTool;
    [SerializeField] private Transform boardLettersParent;
    [SerializeField] private GameObject gameUI, dialogueUI, failUI, winUI, confettiFX;
    [SerializeField] private TextMeshProUGUI dialogueText, numOfLettersLeftText;
    [SerializeField] private Tweener cameraTweener;
    [SerializeField] private Transform dialogueCamera, gameCamera;
    [SerializeField] private Animator characterAnim;
    [SerializeField] private ParticleSystem positiveFX;
    [SerializeField] private CinemachineVirtualCamera stepCamera, mirrorCamera;
    [SerializeField] private GameObject skipButton;

    [Header("Current data")]
    [SerializeField] private string currentSentence;
    [SerializeField] private Animator lastTipAnimator;

    private bool doneSpeaking;
    private Coroutine speechRoutine;
    private string currentLetterToCheck;

    //private Animator characterAnim;

    public override void OnStepStart()
    {
        base.OnStepStart();
        currentConversationIndex = 0;
        stepTool.ToggleActivate(false);

        StartCoroutine(SetupRoutine());
    }

    public IEnumerator SetupRoutine()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        conversations = configurator.levelConfiguration.conversations;
        characterAnim = configurator.clientScript.GetComponentInChildren<Animator>();
        CheckIfDone();
        yield return new WaitForSeconds(5);
        skipButton.SetActive(true);
    }

    public void UpdateCurrentSentence(string sentence, bool checkLetterTarget = true)
    {
        spiritCurrentSentenceText.text = sentence;


        sentenceAnimator.SetTrigger("bump");
        currentSentence = sentence;
        
        if (checkLetterTarget)
            CheckCurrentLetterTarget();

        if (sentence.Length > 0)
            gameUI.SetActive(true);
    }

    private void CheckIfDone()
    {
        if (currentConversationIndex > conversations.Length - 1)
        {
            Done();
            return;
        }
        else
            StartCoroutine(StartNewConversation());
    }

    private IEnumerator StartNewConversation()
    {
        yield return new WaitForSeconds(0.5f);
        UpdateCurrentSentence("", false);
        lastTipAnimator = null;
        dialogueText.text = "";
        dialogueUI.SetActive(true);

        doneSpeaking = false;
        characterAnim.SetTrigger("ask");
        PopulateSpeechBubble(conversations[currentConversationIndex].question, 1, 0);
        yield return new WaitUntil(() => doneSpeaking);
        yield return new WaitForSeconds(0.5f);
        characterAnim.SetTrigger("idle");

        cameraTweener.UpdateTarget(gameCamera);
        yield return new WaitForSeconds(1);

        currentCorrectSentence = conversations[currentConversationIndex].correctAnswer;

        spiritSentence.SetupSentence(currentCorrectSentence);

        stepTool.ToggleActivate(true);
        stepTool.ResetSentence();
        Utility.SetCinemachineCameraSolo(mirrorCamera);
        CheckCurrentLetterTarget();
    }

    public void CheckCurrentLetterTarget()
    {
        if (currentSentence.Length < currentCorrectSentence.Length)
        {
            if (currentCorrectSentence == "Goodbye")
                ToggleHint(currentCorrectSentence);
            else
            {
                string currentLetterToFind = currentCorrectSentence[currentSentence.Length].ToString();

                // check if space
                if (currentLetterToFind == " ") 
                {
                    stepTool.AddSpaceToSentence();
                    Debug.Log("SPACE!!!");
                    return;
                }
                else
                    ToggleHint(currentLetterToFind);
            }

            spiritSentence.RemoveBlocker(currentSentence.Length);
        }
        else
        {
            lastTipAnimator.GetComponent<OujiaLetter>().isCorrect = false;
            lastTipAnimator.SetTrigger("idle");
            stepTool.SetTrackedLetter(null);
            Debug.Log("All done for current sentence");
            skipButton.SetActive(false);
            Confirm();
        }

        CountRemainingLetters();
    }

    private void CountRemainingLetters()
    {
        int subtrahend = 0;
        int minuend = 0;

        for (int i = 0; i < currentSentence.Length; i++)
            if (currentSentence[i].ToString() != " ")
                subtrahend++;

        for (int i = 0; i < currentCorrectSentence.Length; i++)
            if (currentCorrectSentence[i].ToString() != " ")
                minuend++;

        int difference = minuend - subtrahend;

        numOfLettersLeftText.text = $"{difference} / {minuend}";
    }

    public void Confirm(bool autoLose = false)
    {
        StartCoroutine(ConfirmRoutine(autoLose));
    }

    private IEnumerator ConfirmRoutine(bool autoLose = false)
    {
        spiritCurrentSentenceText.text = "";
        stepTool.ToggleActivate(false);
        gameUI.SetActive(false);
        Utility.SetCinemachineCameraSolo(stepCamera);
        cameraTweener.UpdateTarget(dialogueCamera);

        // lose
        if ((currentSentence.ToUpper() != currentCorrectSentence.ToUpper()) || autoLose)
        {
            StartCoroutine(LoseRoutine());
            yield break;
        }
        else
        {
            characterAnim.SetTrigger($"{(int) conversations[currentConversationIndex].characterAnimationWhenCorrect}");
            yield return new WaitForSeconds(1.5f);
            currentConversationIndex++;
            CheckIfDone();
        }
    }

    private IEnumerator LoseRoutine()
    {
        skipButton.SetActive(false);
        Debug.Log("LOSE");
        PopulateSpeechBubble("QUACK DOCTOR!!", 1, 0);
        characterAnim.SetTrigger("angry");
        spiritSentence.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        LevelManager.instance.Recap(false);
    }

    public void ResetOujia()    
    {
        StartCoroutine(ResetRoutine());
    }

    private IEnumerator ResetRoutine()
    {
        if (lastTipAnimator != null)
            lastTipAnimator.GetComponent<OujiaLetter>().isCorrect = false;


        stepTool.ToggleActivate(false);
        gameUI.SetActive(false);
        stepTool.ResetSentence();
        stepTool.ToggleActivate(false);
        UpdateCurrentSentence("", false);
        lastTipAnimator = null;

        yield return new WaitForSeconds(1);
        stepTool.ToggleActivate(true);
        CheckCurrentLetterTarget();
    }

    public void ToggleHint(string letterToCheck)
    {
        Debug.Log($"Check {letterToCheck}");

        if (currentSentence.EndsWith(letterToCheck))
        {
            Debug.Log("Same letter");
            StartCoroutine(stepTool.ResetColliderRoutine());
        }

        if (lastTipAnimator != null)
        {
            lastTipAnimator.GetComponent<OujiaLetter>().isCorrect = false;
            lastTipAnimator.SetTrigger("idle");
        }


        for (int i = 0; i < boardLettersParent.childCount; i++)
        {
            if (boardLettersParent.GetChild(i).name.ToUpper().CompareTo(letterToCheck.ToUpper()) == 0)
            {
                lastTipAnimator = boardLettersParent.GetChild(i).GetComponent<Animator>();

                if (OujiaLevelConfigurator.instance.levelConfiguration.isTutorial)
                    lastTipAnimator.SetTrigger("on");

                lastTipAnimator.GetComponent<OujiaLetter>().isCorrect = true;

                stepTool.SetTrackedLetter(lastTipAnimator.transform);
                Debug.Log($"ON : {lastTipAnimator}");
            }
            else
                boardLettersParent.GetChild(i).GetComponent<OujiaLetter>().isCorrect = false;

        }

        currentLetterToCheck = letterToCheck;
    }

    private void Done()
    {
        StartCoroutine(DoneRoutine());
    }

    private IEnumerator DoneRoutine()
    {
        Debug.Log("Done");
        skipButton.SetActive(false);
        spiritSentence.gameObject.SetActive(false);
        confettiFX.SetActive(true);
        dialogueUI.SetActive(false);
        yield return new WaitForSeconds(1);
        //winUI.SetActive(true);
        LevelManager.instance.Recap(true);
    }

    public void PopulateSpeechBubble(string dialogue, float speechAfterDelay, int animCondition, TextMeshProUGUI text = null)
    {
        doneSpeaking = false;

        if (speechRoutine != null)
            StopCoroutine(speechRoutine);

        speechRoutine = StartCoroutine(PopulateDialogueSpeechBubbleRoutine(dialogue, speechAfterDelay, animCondition, text));
    }

    private IEnumerator PopulateDialogueSpeechBubbleRoutine(string dialogue, float speechAfterDelay, int animCondition, TextMeshProUGUI text = null)
    {
        //characterAnim.SetInteger("condition", animCondition);

        var textToAdjust = text == null ? dialogueText : text;
        textToAdjust.text = "";

        foreach (char c in dialogue)
        {
            textToAdjust.text = $"{textToAdjust.text}{c}";
            yield return new WaitForSeconds(speechSpeed);
        }
        yield return new WaitForSeconds(speechAfterDelay);
        doneSpeaking = true;
    }

    public void CheckLatestLetterIfCorrect(string latestLetter)
    {
        Debug.Log($"Latest letter : {latestLetter} | Letter to check : {currentLetterToCheck}");
        if (currentLetterToCheck.ToUpper() == latestLetter.ToUpper())
        {
            positiveFX.Play();
            spiritSentence.SetCorrect(currentSentence.Length, true);
        }
        else
        {
            spiritSentence.SetCorrect(currentSentence.Length, false);
        }
    }
}


[System.Serializable]
public class Conversation
{
    public string question;
    public string correctAnswer;
    public CharacterAnims characterAnimationWhenCorrect;
}

public enum CharacterAnims
{
    happy,
    angry,
    crying,
    dissapointed,
    shocked,
    happyPose,
    angryPose,
    headlessIntroAnimation
}
