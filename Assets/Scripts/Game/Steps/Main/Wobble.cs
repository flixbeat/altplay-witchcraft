using UnityEngine;

public class Wobble : MonoBehaviour
{
    public float
		MaxWobble = 0.03f,
		WobbleSpeed = 1f,
		Recovery = 1f;
	
    float
		wobbleAmountX, wobbleAmountZ,
		wobbleAmountToAddX, wobbleAmountToAddZ,
		
		pulse, time = 0.5f;
	
    Vector3
		lastPos, velocity,
		lastRot, angularVelocity;
	
	Renderer rend;
	
    void Start(){
        rend = GetComponent<Renderer>();
    }
	
    void Update(){
        time += Time.deltaTime;
		
        // decrease wobble over time
			float deltaRecovery = Time.deltaTime * (Recovery);
				
				wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, deltaRecovery);
				wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, deltaRecovery);

        // make a sine wave of the decreasing wobble
			pulse = 2 * Mathf.PI * WobbleSpeed;
			
			float pulseSine = Mathf.Sin(pulse * time);
				
				wobbleAmountX = wobbleAmountToAddX * pulseSine;
				wobbleAmountZ = wobbleAmountToAddZ * pulseSine;

        // send it to the shader
			rend.material.SetFloat("_wobbleX", wobbleAmountX);
			rend.material.SetFloat("_wobbleZ", wobbleAmountZ);

        // velocity
			velocity = (lastPos - transform.position) / Time.deltaTime;
			angularVelocity = transform.rotation.eulerAngles - lastRot;


        // add clamped velocity to wobble
			float angularVelocityX = (velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble;
			float angularVelocityZ = (velocity.z + (angularVelocity.x * 0.2f))* MaxWobble;

				wobbleAmountToAddX += Mathf.Clamp(angularVelocityX, -MaxWobble, MaxWobble);
				wobbleAmountToAddZ += Mathf.Clamp(angularVelocityZ, -MaxWobble, MaxWobble);

        // keep last position
			lastPos = transform.position;
			lastRot = transform.rotation.eulerAngles;
    }
}