using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Crush : MonoBehaviour
{
    public Transform mortarContainer;

    private void Start()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name.Equals("Pestle"))
        {
            StartCoroutine(Break());
            
            IEnumerator Break()
            {
                yield return new WaitForSeconds(0.01f);
                
                for (int i = 0; i < 4; i++)
                {
                    Transform randomPiece = transform.GetChild(Random.Range(0, transform.childCount));
                    
                    Transform piece = Instantiate(randomPiece, Vector3.zero, Quaternion.identity, mortarContainer);
                    piece.localScale = randomPiece.localScale / 1.5f;
                    piece.localRotation = Random.rotation;
                    
                    float x = Random.Range(-0.0003f, 0.0003f);
                    float y = Random.Range(-0.0001f, 0.0001f);
                    float z = Random.Range(-0.0003f, 0.0003f);
                    piece.localPosition = new Vector3(x,y,z);
                    piece.gameObject.SetActive(true);

                    // powderize
                    //if (piece.localScale.x <= 0.3f)
                    //    Destroy(piece.gameObject);
                    //else
                    //    VonScripts.Ingredient.addToGroup.Invoke(piece.gameObject);
                }
                Destroy(gameObject);
            }
        }
    }
}
