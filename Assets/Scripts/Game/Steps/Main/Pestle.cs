using UnityEngine;

public class Pestle : MonoBehaviour
{
	public Animator animator;
	
	public bool isReady{ get; private set; }
	public bool	isGrinding{ get; private set; }
	
	void OnMouseDown(){
		isReady = !isReady;
		animator.SetBool("isReady", isReady);
	}
	
	void Update(){
		//isGrinding = isReady && Input.GetMouseButton(0);
		//animator.SetBool("isGrinding", isGrinding);
	}
	
	void OnTriggerEnter(Collider col){
		//var ingredient = col.GetComponent<Ingredient>();
		
		//if(ingredient)
		//	ingredient.Grind();
	}
}