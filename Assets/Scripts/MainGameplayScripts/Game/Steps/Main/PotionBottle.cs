using UnityEngine;

public class PotionBottle : MonoBehaviour
{
	public Transform
		cork,
		charmAnchorPoint,
		waxAnchorPoint;
	
	[SerializeField] AlembicSelect _bakedWater;
	[SerializeField] GameObject proceduralWater;
	
	[SerializeField] GameObject proceduralFiller;
	[SerializeField] Renderer[] renderers;
	
	public Bounds bounds; // tool that can define objects if they were inside or outside the bottle
	
	GameObject bakedWater;
	bool isAlembic;
	
	void Start(){
		bakedWater = _bakedWater.GetReference();
		isAlembic = _bakedWater.IsAlembic();
		
		NoWater();
	}
	
	[ContextMenu("No Water")]
	public void NoWater(){
		bakedWater?.SetActive(false);
		proceduralWater?.SetActive(false);
	}
	
	public void ThrowCork()
    {
		cork.gameObject.SetActive(false);
		//transform.Find("Cork").GetComponent<Animator>().enabled = true;
    }

	public void PutbackCork()
    {
		cork.gameObject.SetActive(true);
		//transform.Find("Cork").GetComponent<Animator>().SetTrigger("back");
	}

	public void BakedWater(){
		bakedWater?.SetActive(true);
		proceduralWater?.SetActive(false);
	}
	
	public void ProceduralWater(){
		bakedWater?.SetActive(false);
		proceduralWater?.SetActive(true);
	}
	
	public void SetWaterColor(Color color){		
		foreach(var rend in renderers){
			rend.material.color = color;
			rend.material.SetColor("_colorA", color);
			rend.material.SetColor("_colorB", color);
		}
	}
	
	public void Fill(bool isFilling){
		proceduralFiller.SetActive(isFilling);
	}
	
	public bool HasInsidePoint(Vector3 point){
		bounds.center = transform.position;
		return bounds.Contains(point);
	}
	
	public Collider[] CheckInsideBounds(){
		return Physics.OverlapBox(
			bounds.center + transform.position,
			bounds.extents,
			transform.rotation
		);
	}
	
	public Collider[] CheckInsideBounds(LayerMask layerMask){
		return Physics.OverlapBox(
			bounds.center + transform.position,
			bounds.extents,
			transform.rotation,
			layerMask
		);
	}
	
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireCube(bounds.center + transform.position, bounds.size);
	}
}