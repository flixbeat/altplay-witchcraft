using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionManager : MonoBehaviour
{
    public int potionCount;
    [SerializeField] GameObject potionPrefab;
    [SerializeField][Range(0,10)] int maxSpawn;
    [SerializeField] Renderer rend;
    [SerializeField] float yRotation;
    float offset = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        rend.enabled = false;
        //potionCount = transform.childCount;

        SpawnObj();
       
        
       
    }

   public void SpawnObj()
    {
        var min = transform.position.z - 1.3f;
        var max = transform.position.z + transform.localScale.z / 2;
       // var pos = transform.position.x + transform.localScale.z / 2;
        var yPos = Random.Range(min, max);
        for (int i = 0; i < maxSpawn; i++)
        {
            GameObject obj = Instantiate(potionPrefab);
            obj.transform.position = new Vector3(transform.position.x,transform.position.y,min +i * offset);
           // obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, min + i * offset);
           // obj.transform.rotation = transform.rotation;
            obj.SetActive(true);
            obj.transform.parent = transform;
        }
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
