using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace VonScripts
{
    public class Pestle : MonoBehaviour
    {
        [SerializeField] private const float rayDistance = 10f;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private Transform powder;
        
        private MeshCollider meshCollider;
        private Camera cam;
        private Animator anim;
        private Queue<GameObject> powders = new Queue<GameObject>();

        private float timePassed = 0f;
        private Vector3 lastPoint = Vector3.positiveInfinity;
        
        
        private void Awake()
        {
            meshCollider = transform.GetChild(0).GetComponent<MeshCollider>();
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            cam = Camera.main;
        }

        public void EnableTrigger()
        {
            meshCollider.isTrigger = true;
        }

        public void DisableTrigger()
        {
            meshCollider.isTrigger = false;
        }
        
        private void Update()
        {
            /*if (Input.GetMouseButtonDown((int) MouseButton.LeftMouse) && !IsMashing())
            {
                int layerMask = 1 << this.layerMask;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, rayDistance, ~layerMask))
                    Mash(raycastHit.point);
            }*/
            
            if (Input.GetMouseButton((int) MouseButton.LeftMouse))
            {
                int layerMask = 1 << this.layerMask;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit raycastHit, rayDistance, ~layerMask))
                {
                    if (timePassed > 0.01f)
                    {
                        if (powders.Count > 150)
                            Destroy(powders.Dequeue());

                        if (Vector3.Distance(raycastHit.point, lastPoint) < 0.05f)
                            return;

                        var val = UnityEngine.Random.Range(0.1f,0.3f);
                        powder.localScale = new Vector3(val,0.001f,val);

                        Transform p = Instantiate(powder, raycastHit.point, Quaternion.identity);
                        powders.Enqueue(p.gameObject);
                        
                        
                        var material = p.GetComponent<Renderer>().material;
                        var col = UnityEngine.Random.Range(0.9f,1f);
                        material.color = new Color(col,col,col,1f);
                        
                        timePassed = 0;
                        lastPoint = raycastHit.point;
                    }
                }
            }

            timePassed += Time.deltaTime;
        }

        private void Mash(Vector3 point)
        {
            transform.position = point;
            anim.SetTrigger("Mash");
        }

        private bool IsMashing()
        {
            return anim.GetCurrentAnimatorStateInfo(0).IsName("Crush");
        }
    }
}
