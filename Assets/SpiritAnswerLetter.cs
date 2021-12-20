using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpiritAnswerLetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI letterText;
    [SerializeField] private GameObject blocker;

    [SerializeField] private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    [ContextMenu("Disable Blocker")]
    public void DisableBlocker()
    {
        blocker.SetActive(false);
        anim.SetTrigger("move");
    }

    [ContextMenu("Set Correct")]
    public void SetCorrect()
    {
        SetCorrect(true);
    }

    public void SetCorrect(bool isCorrect)
    {
        letterText.color = isCorrect ? Color.black : Color.red;
        anim.SetTrigger("pop");
    }

    public void Setup(char letter)
    {
        letterText.text = letter.ToString();
    }

}
