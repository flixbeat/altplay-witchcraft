using UnityEngine;

public class Ingredient : MonoBehaviour
{
	public IngredientType ingredientType;
	public GameObject[] groundVersion;
	
	[Tooltip("Ground Object Spawn Offset")]
	public Vector3 offset;

	public void Grind(){}
	
	[ContextMenu("Grind")]
	public void Grind(Vector3 position, float spawnRadius){
		foreach(var gv in groundVersion){
			var gvT = gv.transform;
				gvT.position = position + GetRandomPosition() - offset;
				gvT.rotation = Random.rotation;
				gvT.parent = transform.parent;
			
			gv.SetActive(true);
		}
		
		Destroy(gameObject);
		
		Vector3 GetRandomPosition(){
			return Random.insideUnitSphere * spawnRadius;
		}
	}
}

[System.Serializable]
public enum IngredientType
{
	Basil,
	Chamomile,
	Cinnamon,
	Gemstone,
	Lavender,
	Rosemary,
	Teeth,
	Rose,
	Crystal,
	Penny,
	BatWing
}