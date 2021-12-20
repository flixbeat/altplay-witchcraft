using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Minigame;
using Cinemachine;

namespace Minigame
{
    public class Movement : MonoBehaviour
    {
        [SerializeField]bool isMoving;
        Animator animator;
        CharacterController character;
        [SerializeField] float moveSpeed, walkSpeed, sideSpeed;
        float speed;
        float startTime, endTime;
        ClothesManager clothesManager;
        PlayerCharacter playerCharacter;
        bool isRunning;
        bool isGoodWitch;
        [SerializeField] Recap recapObj;
        // [SerializeField] private Recap _recap;
        [SerializeField] Slider slider,sliderBad,slideGood;
        [SerializeField] Image mainSliderImage;
        [SerializeField] PotionManager potionCount;
        public PotionType potionToCollect;

        int maxDoor = 3;
        public int currentDoor = 0;
        [SerializeField] Color badColor, goodColor,defaultColor;
        [SerializeField] GameObject doorVfx,potionVfx;
        [SerializeField] TextMeshProUGUI feedBackTxt,badTxt,goodTxt;
        [SerializeField]bool startRotate,isGameFinished;
        [SerializeField] List<GameObject> stars;
        [SerializeField] int badPotionCounter, goodPotionCounter;
        private Vector3 _mousePosition;
        Quaternion playerRot;

        [SerializeField] Material defaultMat, goodMat, badMat;
        [SerializeField] List<Renderer> worldrend;
        [SerializeField] private CinemachineVirtualCamera gameCamera;
        // Start is called before the first frame update
        void Start()
        {
           
            foreach (var item in worldrend)
            {
                item.material = defaultMat;
            }
            speed = walkSpeed;
            //_recap = recapObj.GetComponent<Recap>();
            animator = GetComponentInChildren<Animator>();
            character = GetComponent<CharacterController>();
            clothesManager = GetComponentInParent<ClothesManager>();
            playerCharacter = GetComponentInChildren<PlayerCharacter>();
            feedBackTxt.text = "";
            playerRot = playerCharacter.transform.rotation;
            // slider.maxValue = potionCount.potionCount;
            //RenderSettings.skybox.SetColor("_Tint",defaultColor);

        }

        //public override void OnStepStart()
        //{
        //    base.OnStepStart();
        //}

        //public override void OnStepEnd()
        //{
        //    base.OnStepEnd();
            
        //}


        void Update()
        {
            HandleInput();
            HandleRotation();
            // CheckSliderValue();
            GoodAndBadUi();
            HandleTouchInput();
        }
        void GoodAndBadUi()
        {
            var e = Mathf.Abs(slideGood.value * 100);
            goodTxt.text = e.ToString("F0")+"%";

            var a = Mathf.Abs(slideGood.value * 100 - 100);
            badTxt.text = a.ToString("F0") + "%";
        }
        void HandleRotation()
        {
            if (startRotate)
            {
                playerCharacter.transform.Rotate(new Vector3(0, 180, 0) * 3f * Time.deltaTime);
            }
            else
            {
                playerCharacter.transform.rotation = playerRot;
            }

            if (isGameFinished)
            {
                playerCharacter.transform.rotation = Quaternion.Euler(0, 180, 0);
                playerCharacter.transform.position = new Vector3(0, playerCharacter.transform.position.y,playerCharacter.transform.position.z);
            }
        }

        void HandleTouchInput()
        {
            //foreach (var touch in Input.touches)
            //{
            //    if (touch.phase == TouchPhase.Began)
            //    {

            //    }
            //    else if(touch.phase == TouchPhase.Ended || Input.GetMouseButtonUp(0))
            //    {
            //        isMoving = true;
            //    }
           // }
        }
        void HandleInput()
        {
            if (isGameFinished)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                //startTime = Time.time;
                isMoving = true;
            }

            // if (Input.GetMouseButtonUp(0))
            // {
            //endTime = Time.time;
            // isMoving = false;
            //  }

            if (isRunning)
            {
                speed = moveSpeed;

            }
            else
            {
                speed = walkSpeed;
            }

            if (isMoving)
            {
                //character.Move(speed * Time.deltaTime * Vector3.forward);
                transform.Translate(speed * Time.deltaTime * Vector3.forward);


                var move = Vector3.forward;
                if (Mathf.Abs(_mousePosition.x - Input.mousePosition.x) > 1 && Input.GetMouseButton(0))
                {
                    var x = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1);
                    ;
                    //character.Move(sideSpeed * Time.deltaTime * x * Vector3.right);
                    var clampPos = Mathf.Clamp(transform.position.x,-2.5f,2f);
                    transform.Translate(sideSpeed * Time.deltaTime*x * Vector3.right);
                   
                    move *= Mathf.Lerp(1, .1f, Mathf.Abs(x));
                }

                _mousePosition = Input.mousePosition;

                //character.Move(sideSpeed * Time.deltaTime * move);
                //transform.Translate(sideSpeed * Time.deltaTime * move);
                animator.SetBool("isMoving", true);
            }
            else
            {
                //animator.SetBool("isMoving", false);
            }



        }

        public void StartMoving(bool startMove)
        {
            isMoving = startMove;
        }

        void CheckGoodOrBad()
        {
            //slideGood.value
        }
        void CheckSlider()
        {
            if (isGoodWitch)
            {
                if ((slider.value) == 3)
                {
                    // Debug.Log("percent is 20");
                    // Debug.Log(slider.value);
                    playerCharacter.SetClothesGoodWitch();
                }
                else if ((slider.value) == 6)
                {
                    playerCharacter.SetClothesGoodWitch();
                }
                var t = 100 * slider.value / slider.maxValue;
                if (t >= 20f)
                {
                    Debug.Log("val is 20%");
                }
                else if (t >= 40)
                {
                    Debug.Log("val is 40%");
                }
                else if (t >= 100)
                {
                    Debug.Log("val is 100%");
                }

                Debug.Log(t);
            }
            else
            {
                if ((slider.value) == 3)
                {
                    // Debug.Log("percent is 20");
                    // Debug.Log(slider.value);
                    playerCharacter.SetClothesBadWitch();
                }
                else if ((slider.value) == 6)
                {
                    playerCharacter.SetClothesBadWitch();
                }
            }



        }
        void CheckSliderValue()
        {
           
            
                if (slider.value * 100 >= 100)
                {
                    var counter = 3;
                    for (int i = 0; i < counter; i++)
                    {
                        stars[i].SetActive(true);
                    }
                }
                else if (slider.value * 100  >= 50 && slider.value * 100 <= 90 )
                {
                    var counter = 3;
                    for (int i = 0; i < counter; i++)
                    {
                        stars[i].SetActive(true);
                    }
                }
                else 
                {
                    var counter = 1;
                    for (int i = 0; i < counter; i++)
                    {
                        stars[i].SetActive(true);
                    }
                }

            Debug.Log(slider.value * 100);
                     

        }
        //private void OnControllerColliderHit(ControllerColliderHit hit)
        //{


        //    if (hit.gameObject.tag.Equals("Potion"))
        //    {
        //        Debug.Log("Potion");
        //        if (hit.gameObject.TryGetComponent<Potion>(out Potion potion))
        //        {
        //            if (potion.potionType == potionToCollect)
        //            {
        //                slider.value += 0.05f;
        //                Debug.Log("Same Potion");
        //            }
        //            else
        //            {
        //                slider.value -= 0.05f;
        //                Debug.Log("Not same Potion");
        //            }
        //        }

        //        hit.gameObject.SetActive(false);


        //    }



        //}
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag.Equals("Potion"))
            {
                Debug.Log("Potion");
                if (other.TryGetComponent(out Potion potion))
                {
                    potionVfx.SetActive(true);
                    if (potion.potionType == potionToCollect)
                    {
                       
                        slider.value = Mathf.Clamp01(slider.value + 0.1f);
                        feedBackTxt.gameObject.SetActive(true);
                        feedBackTxt.color = Color.green;
                        feedBackTxt.text = "+ 1";

                        if (isGoodWitch)
                        {
                           
                            slideGood.value += 0.1f;
                          
                        }
                        else
                        {
                            slideGood.value -= 0.1f;
                          
                            
                        }
                        Debug.Log("Same Potion");
                    }
                    else
                    {

                        slider.value = Mathf.Clamp01(slider.value - 0.1f);
                        feedBackTxt.gameObject.SetActive(true);
                        feedBackTxt.color = Color.red;
                        feedBackTxt.text = "- 1";
                        Debug.Log("Not same Potion");
                    }

                  
                }

                if (potionToCollect == PotionType.Bad)
                {

                }
                StartCoroutine(DisableObject(potionVfx.transform, .5f));
                StartCoroutine(DisableObject(feedBackTxt.transform, 1f));
                other.gameObject.SetActive(false);
               
               // StartCoroutine(DisableObject(feedBackTxt.transform, .5f));

            }
            #region
            //if (other.gameObject.tag.Equals("Clothes"))
            //{
            //    var cloth = other.gameObject.GetComponent<ClotheItem>();
            //    if (cloth != null)
            //    {
            //        switch (cloth.clotheType)
            //        {
            //            case ClotheType.Correct:
            //                clothesManager.currentCount++;
            //                playerCharacter.SetClothes(cloth.bodyType, cloth.index);
            //                isRunning = true;
            //                other.gameObject.SetActive(false);
            //                break;
            //            case ClotheType.Normal:
            //                playerCharacter.SetClothes(cloth.bodyType, cloth.index);
            //                other.gameObject.SetActive(false);
            //                isRunning = true;
            //                break;
            //            case ClotheType.Wrong:
            //                playerCharacter.SetClothes(cloth.bodyType, cloth.index);
            //                isRunning = false;
            //                other.gameObject.SetActive(false);
            //                break;
            //            default:
            //                break;
            //        }
            //    }
            //}
            #endregion
            if (other.gameObject.tag.Equals("FinishLine"))
            {
                recapObj.isWin = true;
                  StepsManager.instance.NextStep();
                isMoving = false;
                animator.SetBool("isMoving", false);
                animator.SetTrigger("happy");
                gameCamera.LookAt = null;
                gameCamera.Follow = null;
                transform.Rotate(new Vector3(0,180,0));
                isGameFinished = true;
                CheckSliderValue();
                Debug.Log("GAME FINISHED");
            }

            if (other.gameObject.tag.Equals("Door"))
            {
                currentDoor += 1;
                doorVfx.gameObject.SetActive(true);

                //animator.SetTrigger("spin");
                //Quaternion currentRotation = playerCharacter.transform.rotation;
                //Quaternion wantedRotation = Quaternion.Euler(0, 180, 0);
                //transform.rotation = Quaternion.RotateTowards(currentRotation, wantedRotation, Time.deltaTime * 300f);
                startRotate = true;
                if (other.TryGetComponent<Door>(out Door door))
                {
                    if (door.doorType == DoorType.Bad)
                    {
                        //door choose is bad
                       isGoodWitch = false;
                        potionToCollect = PotionType.Bad;
                        SetPlayerClothes(0);
                        // RenderSettings.skybox.SetColor("_Tint",badColor) ;
                        foreach (var item in worldrend)
                        {
                            item.material = badMat;
                        }

                        mainSliderImage.color = badColor;
                        //playerCharacter.SetClothesBadWitch();
                        ;
                       // DeactivaePlayerClothes(1);
                        
                    }
                    else
                    {
                        // door choose is good
                        SetPlayerClothes(1);
                        // playerCharacter.SetClothesGoodWitch();
                        foreach (var item in worldrend)
                        {
                            item.material = goodMat;
                        }
                        //DeactivaePlayerClothes(0);

                        // RenderSettings.skybox.SetColor("_Tint", goodColor);
                        potionToCollect = PotionType.Good;
                        mainSliderImage.color = goodColor;
                        isGoodWitch = true;
                    }
                    StartCoroutine(DisableObject(door.parentGameobject,.5f));
                    StartCoroutine(DisableObject(doorVfx.transform,1f));
                    // doorVfx.SetActive(false);
                    StartCoroutine(StopRotation(.5f));
                }
                
            }



            
        }

        IEnumerator StopRotation(float time)
        {
            yield return new WaitForSeconds(time);
            startRotate = false;
        }

       IEnumerator DisableObject(Transform objToDisable,float time)
       {
            yield return new WaitForSeconds(time);
            objToDisable.gameObject.SetActive(false);
       }

        float mapRange(float a1, float a2, float b1, float b2, float s)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }

        void SetPlayerClothes(int index)
        {
            switch (currentDoor)
            {
                case 1:
                    playerCharacter.SetClothes(BodyType.Shoes,index);
                    //playerCharacter.SetClothes(BodyType.Hair, index);
                    break;
                case 2:
                   // playerCharacter.SetClothes(BodyType.Shoes, index);
                    //playerCharacter.SetClothes(BodyType.Hair, index);
                    playerCharacter.SetClothes(BodyType.Top, index);
                    playerCharacter.SetClothes(BodyType.acce2, index);
                    playerCharacter.SetClothes(BodyType.Bottom, index);
                    playerCharacter.SetClothes(BodyType.acce3, index);

                    playerCharacter.DeactivateDefaultClothes();
                    break;
                case 3:
                   // playerCharacter.SetClothes(BodyType.Shoes, index);
                    //playerCharacter.SetClothes(BodyType.Hair, index);
                 //   playerCharacter.SetClothes(BodyType.Top, index);
                  //  playerCharacter.SetClothes(BodyType.acce2, index);
                    playerCharacter.SetClothes(BodyType.acce1, index);
                   // playerCharacter.SetClothes(BodyType.Bottom, index);
                   
                    break;
               
            }
        }

        void DeactivaePlayerClothes(int index)
        {

            switch (currentDoor)
            {
                case 0:
                    playerCharacter.DeactivateClothes(BodyType.Shoes, index);
                    playerCharacter.DeactivateClothes(BodyType.Hair, index);
                    break;
                case 1:
                    playerCharacter.DeactivateClothes(BodyType.Shoes, index);
                    playerCharacter.DeactivateClothes(BodyType.Hair, index);
                    playerCharacter.DeactivateClothes(BodyType.Top, index);
                    playerCharacter.DeactivateClothes(BodyType.acce2, index);
                    break;
                case 2:
                    playerCharacter.DeactivateClothes(BodyType.Shoes, index);
                    playerCharacter.DeactivateClothes(BodyType.Hair, index);
                    playerCharacter.DeactivateClothes(BodyType.Top, index);
                    playerCharacter.DeactivateClothes(BodyType.acce2, index);
                    playerCharacter.DeactivateClothes(BodyType.acce1, index);
                    playerCharacter.DeactivateClothes(BodyType.Bottom, index);
                    break;

            }

        }


    }
}

