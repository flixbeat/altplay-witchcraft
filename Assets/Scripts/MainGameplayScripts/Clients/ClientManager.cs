using UnityEngine;

public sealed class ClientManager : MonoBehaviour
{
	public Client[] clients;
	public Client current;
	
	public int currentClientIndex;
	public bool randomClient;
	
	static ClientManager _instance;
	public static ClientManager instance{
		get{
			if(!_instance){
				_instance = FindObjectOfType<ClientManager>();
				_instance.Start();
			}
			
			return _instance;
		}
	}
	
	bool hasStarted;
	
	void Start(){
        if(hasStarted) return;
		
		int count = clients.Length;
			
			if(randomClient)
				currentClientIndex = Random.Range(0, count);
		
		current = clients[currentClientIndex];
		
		for(int i = 0; i < count; i ++){
			bool isActive = i == currentClientIndex;
			var client = clients[i].gameObject;
			
				client.SetActive(isActive);
		}
		
		hasStarted = true;
    }
}