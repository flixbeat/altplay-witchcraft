using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixPotions : Step
{
    [Header("Configurations")]
    [SerializeField] private Bottle[] bottles;
    [SerializeField] private List<int> correctBottleIndexesToMix;
    [SerializeField] private int totalBottlesInThisLevel = 5;

    [Header("Scene References")]
    [SerializeField] private Transform pouringPosition;
    [SerializeField] private Transform bottlesParent;
    [SerializeField] private Transform waterStartingPosition, jarWatersParent;
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private ParticleSystem pourParticle, dropletParticle, stirParticle, blastParticle;
    [SerializeField] private Color averageColor;
    [SerializeField] private GameObject buttonsUI;
    [SerializeField] private Tweener cameraTweener;
    [SerializeField] private Transform zoomCameraPos, originCameraPos;
    [SerializeField] private GameObject craftTemplate;
    [SerializeField] private Transform craftParent;
    [SerializeField] private Sprite plusSignSprite;
    [SerializeField] private GameObject ingredientsUI;
    [SerializeField] private MixPotionsRecap recapScript;

    private List<Color> waterColors = new List<Color>();
    private List<int> pouredBottlesindexes = new List<int>();

    private int currentIndex, lastIndex;
    private float maxSizeOfWater = 0.009f;
    private GameObject lastWater;
    private bool canCancel, canPour, lerpColors;

    private void Start()
    {
        SetupBottlesAndButtons();
    }
    public override void OnStepStart()
    {
        base.OnStepStart();
        canPour = true;
        lerpColors = false;
    }

    private void SetupBottlesAndButtons()
    {
        // set craft UI
        for (int i = 0; i < correctBottleIndexesToMix.Count; i++)
        {
            GameObject newCraftUI = Instantiate(craftTemplate, craftParent, false);
            newCraftUI.GetComponent<Image>().sprite = bottles[correctBottleIndexesToMix[i]].bottleSprite;
            newCraftUI.SetActive(true);

            //// add plus sign
            //if (i < correctBottleIndexesToMix.Count - 1)
            //{
            //    GameObject plusSign = Instantiate(craftTemplate, craftParent, false);
            //    plusSign.GetComponent<Image>().sprite = plusSignSprite;
            //    plusSign.SetActive(true);
            //}
        }

        // set buttons UI and bottles model
        for (int i = 0; i < bottlesParent.childCount; i++)
        {

            bottlesParent.GetChild(i).gameObject.SetActive(i < totalBottlesInThisLevel);
            buttonsUI.transform.GetChild(i).gameObject.SetActive(i < totalBottlesInThisLevel);

            if (!bottlesParent.GetChild(i).gameObject.activeInHierarchy)
                continue;

            bottlesParent.GetChild(i).Find("Waters/Water/Procedural").GetComponent<Renderer>().material.SetColor("_colorA", bottles[i].bottleColor);
            buttonsUI.transform.GetChild(i).GetComponentInChildren<Image>().sprite = bottles[i].bottleSprite;
        }
    }

    private void Update()
    {
        if (lerpColors)
        {
            for (int i = 0; i < jarWatersParent.childCount; i++)
            {
                jarWatersParent.GetChild(i).GetComponentInChildren<Renderer>().material.SetColor("Color_Top", Color.Lerp(jarWatersParent.GetChild(i).GetComponentInChildren<Renderer>().material.GetColor("Color_Top"), averageColor, Time.deltaTime));
                jarWatersParent.GetChild(i).GetComponentInChildren<Renderer>().material.SetColor("Color_Bottom", Color.Lerp(jarWatersParent.GetChild(i).GetComponentInChildren<Renderer>().material.GetColor("Color_Bottom"), averageColor, Time.deltaTime));
            }
        }
    }


    public void PourPotion(int index)
    {
        if (!canPour)
            return;

        Debug.Log("Pour potion " + index);
        currentIndex = index;

        bottlesParent.GetChild(index).GetComponent<Tweener>().UpdateTarget(pouringPosition);
        StartCoroutine(PourRoutine());
    }

    public void CancelPour()
    {
        if (!canCancel)
            return;

        bottlesParent.GetChild(currentIndex).GetComponent<Tweener>().ToOrigin();
        Debug.Log("Cancel Pour");
        StopAllCoroutines();
    }

    private IEnumerator PourRoutine()
    {
        canCancel = true;
        yield return new WaitForSeconds(1f);
        canCancel = false;
        canPour = false;
        GameObject newWater = Instantiate(waterPrefab, jarWatersParent, false);

        if (lastWater == null)
        {
            newWater.transform.position = waterStartingPosition.transform.position;
            newWater.GetComponentInChildren<Renderer>().material.SetColor("Color_Top", bottles[currentIndex].bottleColor);
            newWater.GetComponentInChildren<Renderer>().material.SetColor("Color_Bottom", bottles[currentIndex].bottleColor);
        }
        else
        {
            newWater.transform.position = lastWater.transform.Find("Top").transform.position + (Vector3.up * 0.0001f);
            newWater.GetComponentInChildren<Renderer>().material.SetColor("Color_Top", bottles[currentIndex].bottleColor);
            newWater.GetComponentInChildren<Renderer>().material.SetColor("Color_Bottom", bottles[lastIndex].bottleColor);
        }

        newWater.transform.localScale = Vector3.zero;
        lastWater = newWater;
        lastIndex = currentIndex;
        waterColors.Add(bottles[currentIndex].bottleColor);
        pouredBottlesindexes.Add(currentIndex);
        var particleEmission = dropletParticle.main;
        particleEmission.startColor = bottles[currentIndex].bottleColor;
        Debug.Log("Pour water");
        pourParticle.Play();
        yield return new WaitForSeconds(0.5f);

        // pour animation
        while (newWater.transform.localScale.x < 0.01663956f)
        {
            newWater.transform.localScale += (Vector3.right + Vector3.forward) * Time.deltaTime * 0.05f;
            yield return new WaitForEndOfFrame();
        }

        // update the 2 based on the number of ingredients
        float waterTargetSize = maxSizeOfWater / correctBottleIndexesToMix.Count; 

        while (newWater.transform.localScale.y < waterTargetSize)
        {
            newWater.transform.localScale += Vector3.up * Time.deltaTime * 0.003f;
            yield return new WaitForEndOfFrame();
        }
        //

        pourParticle.Stop();
        bottlesParent.GetChild(currentIndex).GetComponent<Tweener>().ToOrigin();
        CheckIfDone(newWater.transform.Find("Top").position.y);
    }

    private void CheckIfDone(float lastTopPosition)
    {
        Debug.Log(lastTopPosition);
        if (lastTopPosition >= 1.56f)
        {
            StartCoroutine(DoneRoutine());
        }
        else
        {
            canPour = true;
        }
    }

    private IEnumerator DoneRoutine()
    {
        CheckIfWin();

        float averageR = 0, averageG = 0, averageB = 0;

        foreach (Color c in waterColors)
        {
            averageR += c.r;
            averageG += c.g;
            averageB += c.b;
        }

        averageColor = new Color(averageR / waterColors.Count, averageG / waterColors.Count, averageB / waterColors.Count);

        var stirMain = stirParticle.main;
        stirMain.startColor = averageColor;
        stirParticle.Play();

        lerpColors = true;
        buttonsUI.SetActive(false);
        ingredientsUI.SetActive(false);
        cameraTweener.UpdateTarget(zoomCameraPos);
        yield return new WaitForSeconds(2.5f);
        stirParticle.Stop();
        blastParticle.Play();
        cameraTweener.UpdateTarget(originCameraPos);

        yield return new WaitForSeconds(1);
        StepsManager.instance.NextStep();

    }

    private void CheckIfWin()
    {
        for (int i = 0; i < correctBottleIndexesToMix.Count; i++)
        {
            if (!pouredBottlesindexes.Contains(correctBottleIndexesToMix[i]))
            {
                Debug.Log("LOSE!!");
                recapScript.SetWin(false);
                return;
            }
        }

        Debug.Log("WIN!");
        recapScript.SetWin(true);

    }

}


[System.Serializable]
public class Bottle
{
    public Color bottleColor;
    public Sprite bottleSprite;
}