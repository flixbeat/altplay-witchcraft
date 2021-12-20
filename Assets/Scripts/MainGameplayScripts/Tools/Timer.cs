using UnityEngine;
using System;

[System.Serializable]
public class Timer
{
	#region By Reference
		
		public static void Tick(
			float duration,
			ref float current,
			float deltaTime,
			Action onElapse
		){
			current += deltaTime;
			
			if(current >= duration){
				onElapse?.Invoke();
				current = 0f;
			}
		}
		
	#endregion
	
	#region By Instance
		
		#region Properties
			
			public float duration;
			public Action onElapse;
			
			public float current{ get; private set; }
			
		#endregion
		
		#region Constructors
			
			public Timer(){}
			
			public Timer(float duration){ this.duration = duration; }
			
			public Timer(float duration, Action onElapse){
				this.duration = duration;
				this.onElapse = onElapse;
			}
			
		#endregion
		
		#region Call
			
			public void Tick(float deltaTime){
				Tick(deltaTime, onElapse);
			}
			
			public void Tick(float deltaTime, Action onElapse){
				current += deltaTime;
				
				if(current >= duration){
					onElapse?.Invoke();
					current = 0f;
				}
			}
			
		#endregion
		
	#endregion
}