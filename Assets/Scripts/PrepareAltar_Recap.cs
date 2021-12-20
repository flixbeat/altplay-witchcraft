using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepareAltar_Recap : Step
{
    [SerializeField] private ParticleSystem smokeBurst;
    [SerializeField] private GameObject frog, altarElements;
    [SerializeField] private Tweener cameraTweener;
    [SerializeField] private Transform winCamPos, loseCamPos;
    [SerializeField] private GameObject winUI, loseUI;
    [SerializeField] private ParticleSystem smokeFXBoyfriend;
    [SerializeField] private GameObject boyfriend;
    public bool isWin;

    public override void OnStepStart()
    {
        base.OnStepStart();
        StartCoroutine(RecapRoutine());
    }

    private IEnumerator RecapRoutine()
    {
        yield return new WaitForSeconds(0f);

        if (isWin)
        {
            cameraTweener.UpdateTarget(winCamPos);
            yield return new WaitForSeconds(1);
            smokeFXBoyfriend.Play();
            boyfriend.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            ClientScript.Instance.Animate("hug");
            yield return new WaitForSeconds(2);
            //winUI.SetActive(true);
        }
        else
        {
            smokeBurst.Play();
            altarElements.SetActive(false);
            frog.SetActive(true);
            cameraTweener.UpdateTarget(loseCamPos);
            yield return new WaitForSeconds(2);
            //loseUI.SetActive(true);
        }

        LevelManager.instance.Recap(isWin);
    }
}
