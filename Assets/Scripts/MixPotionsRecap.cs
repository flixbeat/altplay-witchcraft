using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixPotionsRecap : Step
{
    [SerializeField] private Tweener jarTweener;
    [SerializeField] private Transform jarTarget;
    [SerializeField] private Animator characterAnimator;
    [SerializeField] private SkinnedMeshRenderer headRenderer;
    [SerializeField] private bool isWin;
    [SerializeField] private GameObject poofFX, confettiFX, failFX;
    [SerializeField] private GameObject hairWin, hairLose;
    [SerializeField] private GameObject winUI, loseUI;

    public override void OnStepStart()
    {
        base.OnStepStart();

        StartCoroutine(RecapRoutine());
    }

    public void SetWin(bool isWin)
    {
        this.isWin = isWin;
    }
    private IEnumerator RecapRoutine()
    {
        jarTweener.transform.parent = jarTarget.transform.parent;
        jarTweener.SetSpeed(1000);
        jarTweener.SetRotationSpeed(1000);
        jarTweener.SetScaleSpeed(1000);
        jarTweener.UpdateTarget(jarTarget);
        characterAnimator.SetTrigger("drink");
        yield return new WaitForSeconds(3);

        hairWin.SetActive(false);
        hairLose.SetActive(false);
        poofFX.SetActive(true);
        jarTweener.gameObject.SetActive(false);

        if (isWin)
        {
            confettiFX.SetActive(true);
            characterAnimator.SetTrigger("win");
            headRenderer.SetBlendShapeWeight(11, 0);
            headRenderer.SetBlendShapeWeight(12, 0);
            hairWin.SetActive(true);
            winUI.SetActive(true);
        }
        else
        {
            headRenderer.SetBlendShapeWeight(13, 100);
            characterAnimator.SetTrigger("angry");
            failFX.SetActive(true);
            hairLose.SetActive(true);
            loseUI.SetActive(true);
        }

    }
}
