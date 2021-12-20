using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minigame
{
    public class SelectInsense : Step
    {
        [SerializeField] private int correctIndex;
        [SerializeField] private int currentIndex;
        [SerializeField] private Transform insenseParent, playerParent, rightHandTarget;
        [SerializeField] private Tweener rightHandTweener;
        [SerializeField] private Smudging smudgingStepScript;
        [SerializeField] private Recap recapStepScript;
        [SerializeField] private IncenseSelectionGuide IncenseSelectionGuideScript;

        public bool isCorrect;

        public override void OnStepStart()
        {
            base.OnStepStart();
            SelectCurrentInsense(0);
            IncenseSelectionGuideScript.SetCorrectIncense(correctIndex);
        }

        public void SelectCurrentInsense(int index)
        {
            Utility.DeactivateChildrenExceptIndex(insenseParent, index);
            currentIndex = index;
        }

        public void Confirm()
        {
            isCorrect = currentIndex == correctIndex;
            //recapStepScript.isWin = isCorrect;
            IncenseSelectionGuideScript.CheckIncenseSelected(currentIndex);
            smudgingStepScript.smokeParticles.Clear();
            for (int i = 0; i < insenseParent.GetChild(currentIndex).childCount; i++)
                smudgingStepScript.smokeParticles.Add(insenseParent.GetChild(currentIndex).GetChild(i).gameObject);

            rightHandTweener.transform.parent = playerParent;
            rightHandTweener.transform.position = rightHandTarget.transform.position;

            StepsManager.instance.NextStep();
        }
    }
}