using UnityEngine;
using UnityEngine.UI;

public class BasketItemUI : MonoBehaviour
{
	public Image icon;
	public GameObject check;
	
	public BasketItem myItem{ get; private set; }
	
	public void Initialize(BasketItem item){
		myItem = item;
		
		icon.sprite = item.icon;
		name = item.name;
		
		gameObject.SetActive(true);
	}
	
	public void Check(bool check){
		this.check.SetActive(check);
	}
}