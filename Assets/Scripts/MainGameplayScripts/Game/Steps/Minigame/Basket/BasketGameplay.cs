using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

/*
	Ihssane  11:14 AM
	@Kenn Ysrael here's the next other gameplay  you tap the items  that's shown in the UI to add to your basket,  @Jude Magalan will prepare the ingredients in jars and I will prepare the UI as well .
	we will work on it later after we finish other gameplays and we add it in today's loop that we should send to the publisher
*/

public class BasketGameplay : MonoBehaviour
{
	#region Variables
		
		public BasketItem[] items;
		public BasketItemUI itemUITemplate;
			
			List<BasketItem>
				tasks = new List<BasketItem>(),
				itemsInbasket = new List<BasketItem>();
		
			BasketItemUI[] tasksUI;
			const int maxTaskCount = 3;
		
		Camera cam;
		
		[Space()]
		public LayerMask itemsLayer;
		public Transform basketPoint;
		
		[Space()]
		public GameObject confirmButton;
		public float confirmShowDelay = 5f;
		
		[Space()]
		public GameObject[] winObjects;
		public GameObject[] loseObjects;
		
		public UnityEvent onWinning, onLosing;
		
	#endregion
	
	#region UNITY METHODS
		
		void Awake(){
			cam = Camera.main;
		}
		
		void Start(){
			GenerateTasks();
			
			Invoke(
				nameof(ShowConfirmButton),
				confirmShowDelay
			);
		}
		
		void Update(){
			if(Input.GetMouseButtonDown(0)){
				var ray = cam.ScreenPointToRay(Input.mousePosition);
				bool raycast = Physics.Raycast(ray, out var hit, 100, itemsLayer);
				
				if(raycast){
					var item = hit.collider.GetComponent<BasketItem>();
					
					if(item){
						if(itemsInbasket.Contains(item)){
							item.Return();
							itemsInbasket.Remove(item);
						}
						
						else{
							item.UpdateTweener(basketPoint);
							itemsInbasket.Add(item);
						}
						
						UpdateUI();
					}
				}
			}
		}
		
	#endregion
	
	#region Custom Methods
		
		void GenerateTasks(){
			tasksUI = new BasketItemUI[maxTaskCount];
			
			for(int i = 0; i < maxTaskCount; i++){
				var newTask = items[Random.Range(0, items.Length)];
				
					while(tasks.Contains(newTask)){
						newTask = items[Random.Range(0, items.Length)];
						Debug.Log("Preventing Duplicate tasks");
					}
					
					tasks.Add(newTask);
					
				// Create UI
				var newUI = Instantiate(
					itemUITemplate,
					itemUITemplate.transform.parent,
					false
				);
				
				newUI.Initialize(newTask);
				tasksUI[i] = newUI;
			}
			
			Destroy(itemUITemplate.gameObject);
		}
		
		void UpdateUI(){
			foreach(var ui in tasksUI)
				ui.Check(itemsInbasket.Contains(ui.myItem));
		}
		
		void ShowConfirmButton(){ confirmButton.SetActive(true); }
		
		public void OnConfirmButton(){ // WIN and LOSE
			bool isWin = true;
			
			CheckForTasks();
			if(isWin) CheckForItemsInBasket();
			
			ToggleObjects(winObjects, isWin);
			ToggleObjects(loseObjects, !isWin);
			
			void CheckForTasks(){
				foreach(var task in tasks){
					if(!itemsInbasket.Contains(task)){
						isWin = false;
						break;
					}
				}
			}
			
			void CheckForItemsInBasket(){
				foreach(var item in itemsInbasket){
					if(!tasks.Contains(item)){
						isWin = false;
						break;
					}
				}
			}
			
			void ToggleObjects(GameObject[] objects, bool toggle){
				foreach(var obj in objects)
					obj.SetActive(toggle);
			}
		}
		
	#endregion
}