using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
	new public Renderer renderer;
	
	[HideInInspector]
	public int materialIndex;
	
	static readonly string visibility = "_visibility";
	
	public void Enable(bool enable){
		var material = renderer.materials[materialIndex];
		
		float value = enable? 1f: 0f;
		material.SetFloat(visibility, value);
	}
}