using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiritAnswerSentence : MonoBehaviour
{
    [SerializeField] private GameObject template;

    private GameObject newLetter;


    [ContextMenu("TEST")]
    public void Test()
    {
        SetupSentence("Sean");
    }

    public void SetupSentence(string sentence)
    {
        for (int i = 0; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        foreach(char c in sentence.ToUpper())
        {
            newLetter = Instantiate(template, transform, false);

            newLetter.GetComponent<SpiritAnswerLetter>().Setup(c);

            if (c == ' ')
            {
                foreach (Image i in newLetter.GetComponentsInChildren<Image>())
                    i.enabled = false;
            }
        }
    }

    public void RemoveBlocker(int characterIndex)
    {
        transform.GetChild(characterIndex).GetComponent<SpiritAnswerLetter>().DisableBlocker();
    }

    public void SetCorrect(int characterIndex, bool isCorrect)
    {
        transform.GetChild(characterIndex).GetComponent<SpiritAnswerLetter>().SetCorrect(isCorrect);
    }
}
