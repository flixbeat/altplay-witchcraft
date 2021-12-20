using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Breakable : MonoBehaviour
{
    [SerializeField] private Transform container;
    
    private bool allowBreak;
    private float timePassed;
    private float allowBreakSec = 0.5f;
    
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Start()
    {
        rb.isKinematic = true;
        StartCoroutine(EnablePhysics());
        IEnumerator EnablePhysics()
        {
            yield return new WaitForSeconds(0.2f);
            rb.isKinematic = false;
        }
    }

    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed > allowBreakSec)
            allowBreak = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Equals("Pestle") && allowBreak)
        {
            allowBreak = false;
            timePassed = 0;
            
            StartCoroutine(Break());
            
            IEnumerator Break()
            {
                yield return new WaitForSeconds(0.01f);
                
                for (int i = 0; i < 4; i++)
                {
                    Transform piece = Instantiate(transform, transform.position, transform.rotation, container);
                    piece.localScale = transform.localScale / 1.5f;
                    piece.localRotation = Random.rotation;
                    
                    float x = transform.position.x + Random.Range(-0.0003f, 0.0003f);
                    float y = transform.position.y + 0.2f;
                    float z = transform.position.z + Random.Range(-0.0003f, 0.0003f);
                    piece.position = new Vector3(x,y,z);
                    
                    // powderize
                    if (piece.localScale.x <= 0.2f)
                        Mortar.powderize.Invoke(transform.position);
                }
                Destroy(gameObject);
            }
        }
    }
}
