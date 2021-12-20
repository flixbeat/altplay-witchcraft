using UnityEngine;

public class BasketItem : MonoBehaviour
{
	public Sprite icon;
	public float dropDelayDuration = 0.75f;
	
	Tweener tweener;
	static Transform basketPoint;
	
	void Start(){
		tweener = GetComponent<Tweener>();
		tweener.SetOrigin();
	}
	
	public void UpdateTweener(Transform target){
		tweener.UpdateTarget(target);
		Invoke(nameof(Drop), dropDelayDuration);
		
		if(!basketPoint)
			basketPoint = target;
	}
	
	public void Return(){
		if(basketPoint)
			tweener.UpdateTarget(basketPoint);
		
		Invoke(nameof(ToOrigin), dropDelayDuration);
	}
	
	void Drop(){
		tweener.rigidbody.isKinematic = false;
		tweener.enabled = false;
	}
	
	void ToOrigin(){
		tweener.ToOrigin();
	}
}