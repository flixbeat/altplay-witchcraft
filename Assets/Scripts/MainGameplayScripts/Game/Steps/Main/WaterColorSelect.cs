using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WaterColorSelect : Step
{
    [SerializeField] Renderer[] waterRenderers;
	[SerializeField] Color[] waterColors;
	[SerializeField] private Sprite[] waterSprites;
	[SerializeField] private Image hint;
	[SerializeField] private SelectCraft selectCraftScript;
	[SerializeField] private Recap recapScript;
	[SerializeField] private SelectCraft craftScript;
	[SerializeField] private ParticleSystem stirringFX;

	private int currentIndex;

	[Space(), SerializeField]
	string[] materialProperties = {
		"_colorA",
		"_colorB"
	};
	
	[Space(), SerializeField] UnityEvent<Color> onColorChange;
	
	public Color Selected{ get; private set; }
	
	Recap recap;
	PotionBottle[] potionBottles;
	
	void Awake(){
		recap = FindObjectOfType<Recap>(true);
		potionBottles = FindObjectsOfType<PotionBottle>(true);
	}

    public override void OnStepStart()
    {
        base.OnStepStart();
		hint.sprite = waterSprites[(int)craftScript.currentCraft.correctWaterColor];
    }

    public void ChangeWaterColor(int index)
    {
		Selected = waterColors[index];
		currentIndex = index;

		foreach (Renderer r in waterRenderers){
			var material = r.material;
				material.color = Selected;
			
			foreach(string property in materialProperties)
				material.SetColor(property, Selected);
			
			foreach(var pb in potionBottles)
				pb.SetWaterColor(Selected);
		}
		
		onColorChange?.Invoke(Selected);


		StepsManager.instance.NextStep();
    }
    public override void OnStepEnd()
    {
		if ((int)selectCraftScript.currentCraft.correctWaterColor != currentIndex)
			recapScript.mistakes++;


		Color waterColorFullTransparency = new Color(waterColors[currentIndex].r, waterColors[currentIndex].g, waterColors[currentIndex].b, 1);
		foreach(ParticleSystem p in stirringFX.GetComponentsInChildren<ParticleSystem>())
        {
			//var emission = p.main;
			//emission.startColor = waterColorFullTransparency;
			p.GetComponent<Renderer>().material.color = waterColorFullTransparency;
		}

		base.OnStepEnd();
	}
}