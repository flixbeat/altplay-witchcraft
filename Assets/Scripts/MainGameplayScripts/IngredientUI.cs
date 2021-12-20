using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientUI : MonoBehaviour
{
    [SerializeField] private int count;
    public IngredientType ingredientType;
    [SerializeField] private GameObject checkMark, countObject, xMark;
	
    [SerializeField] bool isDone;
    public bool IsDone => isDone;
	
	Text countText;
	
    private void Start()
    {
		countText = countObject.GetComponentInChildren<Text>();
        countText.text = count.ToString();
    }
	
    public bool Check(IngredientType type)
    {
        if(isDone || type != ingredientType) return false;
		
        count--;
		
		if(countText)
			countText.text = count.ToString();
		
		if(count <= 0){
			checkMark.SetActive(true);
			countObject.SetActive(false);
			isDone = true;
		}

		return true;
    }

	public void ShowWrongMark()
    {
		xMark.SetActive(true);
		isDone = true;
    }
}