using UnityEngine;

// Mark Yassine 12:42 For the FX, try to show them progressively while you are crashing the ingredients, it should not appear suddenly at first

public class GrindingSparkle : MonoBehaviour
{
    public ParticleSystem sparkleParticle;
	public AnimationCurve falloff;
	
	Material sparkleMaterial;
	
	void Start(){
		sparkleMaterial = sparkleParticle.GetComponent<ParticleSystemRenderer>().material;
	}
	
	public void FadeInSparkleParticle(float progress){
		var color = sparkleMaterial.color;
			color.a = falloff.Evaluate(progress);
		
		sparkleMaterial.color = color;
	}
}