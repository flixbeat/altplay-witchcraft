using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] private float idleRotationSpeed = 1;
    [SerializeField] private GameObject effectWhenPopped;
    private Animator collectibleAnim;

    private void Start()
    {
        collectibleAnim = GetComponent<Animator>();
        collectibleAnim.SetFloat("idleSpeed", idleRotationSpeed);
    }

    public void Pop()
    {
        collectibleAnim.SetTrigger("pop");

        if (effectWhenPopped != null)
            Destroy(Instantiate(effectWhenPopped, transform, false), 5);
    }
}
