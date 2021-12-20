using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game;
using UnityEngine;
using Random = UnityEngine.Random;


namespace WitchCraftD1
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Vector2 _gravity = new Vector2(1, 9);
        [SerializeField] private ParticleSystem _itemParticles;

        [Header("Movement")] [SerializeField] private float _moveSpeed;
        [SerializeField] private float _sideSpeed;
        [SerializeField] private Vector2 _moveSpeedLimit;
        [SerializeField] private float _moveAcceleration;
        [SerializeField] private float _laneWidth;
        [SerializeField] private float _swipeLimit;
        [SerializeField] private Tweener playerTweener;
        [SerializeField] private Transform normalTarget, backTarget;


        [Header("Attack")] [SerializeField] private float _pushForce;
        [SerializeField] private float _damageForce;
        [SerializeField] private Vector3 _attackForce, _sideAttackForce;
        [SerializeField] private float _attackDuration, _sideAttackDuration;

        [SerializeField] private GameObject _attackParticle;
        [SerializeField] private Transform _attackParticleSpawnPoint;

        [Header("Crouch")] [SerializeField] private float _crouchDuration;

        [Header("Fall")] [SerializeField] private float _impactSpeed = 5;
        [SerializeField] private float _fallLimit;
        [SerializeField] private float _fallDuration = 1;

        [Header("Effects")] [SerializeField] private Transform playerEmojiObject;
        [SerializeField] private Renderer emojiRenderer;
        [SerializeField] private Texture[] emojiPositive, emojiNegative;

        private Animator _animator;
       // private PlayerCharacter _character;
        private CharacterController _characterController;
        private Vector3 _mousePosition;
        private Vector3 _impact;


        private int _lane;
        private bool _isMoving;
        private bool _isDancing;
        private bool _isFalling;
        private bool _isSwiped;
        private bool _isSwiping;
        private bool _isSiding;
        [SerializeField] private bool _isAttacking;
        private bool _isCrouching;
        [SerializeField] private bool _handleInput;
        //private PoleManager _currentPole;

        // Cache
        const string speedParam = "speed";

        private void Start()
        {
            //_character = GetComponentInChildren<PlayerCharacter>();
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponentInChildren<Animator>();
            // _handleInput = true;
        }

        private void Update()
        {
            //if (_currentPole != null)
            //{
            //    return;
            //}

            if (!_isMoving) return;
            if (_handleInput) HandleInput();

        }

        private void FixedUpdate()
        {
            //if (_currentPole != null)
            //{
            //    _currentPole.Move();
            //    return;
            //}

            if (_isMoving)
            {
                _characterController.Move(_moveSpeed * Time.fixedDeltaTime * Vector3.forward);
                CalculateAnimationSpeedPercent(_characterController.velocity, _moveSpeed);

                var position = transform.position;
                var sidePosition = position;
                sidePosition.x = _lane * _laneWidth;
                sidePosition = Vector3.MoveTowards(position, sidePosition, Time.fixedDeltaTime * _sideSpeed) - position;
                _characterController.Move(sidePosition);
                _isSiding = sidePosition.magnitude > .1f;

                if (!_isFalling && transform.position.y < _fallLimit)
                {
                    StartCoroutine(Fall());
                }
            }

            _characterController.Move((_isFalling ? _gravity.x : _gravity.y) * Time.fixedDeltaTime * Vector3.down);

            if (_isFalling) return;
            if (_impact.magnitude > 0.2F) _characterController.Move(_impact * Time.fixedDeltaTime);
            _impact = Vector3.Lerp(_impact, Vector3.zero, _impactSpeed * Time.fixedDeltaTime);
        }

        private void HandleInput()
        {
            _moveSpeed = Mathf.Lerp(_moveSpeed, _moveSpeedLimit.y, _moveAcceleration * Time.deltaTime);
            if (Input.GetMouseButtonDown(0))
            {
                SetMoving(true);
                _isSwiping = false;
                _mousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                _isSwiping = true;
                var delta = Input.mousePosition - _mousePosition;
                var swipe = new Vector2(delta.x, delta.y);
                swipe.Scale(new Vector2(1f / Screen.width, 1f / Screen.height));
                var validSwipe = swipe.magnitude > _swipeLimit;
                swipe.Normalize();

                var isHorizontal = validSwipe && Mathf.Abs(swipe.x) > .5f;
                var isVertical = validSwipe && Mathf.Abs(swipe.y) > .5f;

                if (isVertical && !isHorizontal)
                {
                    if (!_isSwiped)
                    {
                        int direction = Mathf.RoundToInt(swipe.y);
                        ForwardStep(direction);
                    }

                    _isSwiped = true;
                }

                else if (isHorizontal && !isVertical)
                {
                    if (!_isSwiped)
                    {
                        int direction = Mathf.RoundToInt(swipe.x);
                        SideStep(direction);
                    }

                    _isSwiped = true;
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _isSwiped = false;
                _isSwiping = false;
            }
        }

        public void SideStep(int direction)
        {
            direction = Mathf.Clamp(direction, -1, 1);

            var lane = _lane;
            lane += direction;

            SetLane(lane);
            PlayAnimation();
            StartCoroutine(SideAttack());

            void PlayAnimation()
            {
                float facingDirection = playerTweener.transform.forward.normalized.z;
                float localDirection = facingDirection * (float)direction;

                string animationClip = localDirection > 0 ? "Right" : "Left";
                _animator.Play(animationClip);
            }
        }

        public void ForwardStep(int direction)
        {
           // StopAction();

            //if (direction > 0) StartCoroutine(Attack());
           // else StartCoroutine(Crouch());
        }

        private void StopAction()
        {
            StopAllCoroutines();
            if (_isAttacking)
            {
                _isAttacking = false;
                _animator.transform.localEulerAngles = Vector3.zero;
                // Debug.Log("Canceled Attack");
            }
            else if (_isCrouching)
            {
                _isCrouching = false;

                var height = _characterController.height;
                _characterController.height = 2 * height;
                _characterController.center += .5f * height * Vector3.up;
                // Debug.Log("Canceled Crouch");
            }
        }

        private void SetLane(int lane)
        {
            if (lane == _lane) return;
            // if (!_isAttacking && !_isCrouching) SetTrigger(lane > _lane ? "Right" : "Left");
            _lane = lane;
        }

        void SetTrigger(string param)
        {
            StartCoroutine(Routine());

            IEnumerator Routine()
            {
                _animator.SetTrigger(param);
                yield return null;
                _animator.ResetTrigger(param);
            }
        }

        public void AddImpact(Vector3 force)
        {
            _impact = force;
        }

        //private void OnControllerColliderHit(ControllerColliderHit hit)
        //{
        //    if (hit.gameObject.CompareTag("Man"))
        //    {
        //        //var man = hit.gameObject.GetComponent<ManManager>();
        //        if (!man.enabled) return;
        //        var canAttack = _isSiding || _isAttacking;

        //        if (canAttack)
        //        {
        //            var direction = hit.normal;
        //            direction.z = Mathf.Min(direction.z, .1f);
        //            direction -= transform.right * Mathf.Sign(transform.InverseTransformPoint(hit.point).x);
        //            man.Stop(direction.normalized * _pushForce);

        //            Instantiate(_attackParticle, _attackParticleSpawnPoint.position, Quaternion.identity);
        //        }
        //        else
        //        {
        //            Die();
        //        }
        //    }
        //    else if (hit.gameObject.CompareTag("Obstacle"))
        //    {
        //        AddImpact(transform.forward * _damageForce);
        //        _animator.SetTrigger("Stumble");
        //    }
        //    else if (hit.gameObject.CompareTag("Killer"))
        //    {
        //        Die();
        //    }

        //    // else if (hit.gameObject.CompareTag("Jump"))
        //    // {
        //    //     Utils.SetChildrenVisible(hit.transform);
        //    //     Jump(_force);
        //    // }
        //}

        //private void Die()
        //{
        //    enabled = false;
        //    _animator.SetTrigger("Die");
        //    GameManager.Instance.Finish(false);
        //}

        private IEnumerator SideAttack()
        {
            AddImpact(_sideAttackForce);
            _moveSpeed = _moveSpeedLimit.x;
            _isAttacking = true;

            yield return new WaitForSeconds(_sideAttackDuration);

            _isAttacking = false;
            ResetTriggers();
        }

       

        private void ResetTriggers()
        {
            _animator.ResetTrigger("Up");
            _animator.ResetTrigger("Down");
        }

        private IEnumerator Crouch()
        {
            if (_isCrouching) yield break;

            _animator.SetTrigger("Down");
            _moveSpeed = _moveSpeedLimit.x;
            _isCrouching = true;

            var height = _characterController.height * .5f;
            _characterController.height = height;
            _characterController.center += .5f * height * Vector3.down;
            yield return new WaitForSeconds(_crouchDuration);
            _characterController.height = 2 * height;
            _characterController.center += .5f * height * Vector3.up;
            _isCrouching = false;
            ResetTriggers();
        }

        private IEnumerator Fall()
        {
            if (_isFalling) yield break;
            Debug.Log("Fall", this);
            _isFalling = true;
            _isMoving = false;
            _animator.SetTrigger("Fall");

            {
                
                yield return new WaitForSeconds(_fallDuration);
                enabled = false;
            }
        }
        //
        //    private IEnumerator Boost()
        //    {
        //        Debug.Log("Boost", this);
        //
        //        _isBoosted = true;
        //        yield return new WaitForSeconds(_boostDuration);
        //        _isBoosted = false;
        //    }
        //
        //    private IEnumerator Protect()
        //    {
        //        Debug.Log("Protect", this);
        //
        //        _isProtected = true;
        //        yield return new WaitForSeconds(_shieldDuration);
        //        _isProtected = false;
        //    }

        private void Reset()
        {
            Debug.Log("Reset", this);
            _animator.SetTrigger("Stumble");
           // _character.Reset();
            var position = transform.position;
            position.y = 1;
            position.x = 0;
            transform.position = position;
        }

        //     if (!_isMoving) return;
        //
        //     if (other.CompareTag("Floor"))
        //     {
        //         if (_currentPole == null) StartCoroutine(Fall());
        //         return;
        //     }


        //        if (other.CompareTag("Reset"))
        //        {
        //            _character.Reset();
        //            return;
        //        }
        //
        //        if (other.CompareTag("Boost"))
        //        {
        //            StartCoroutine(Boost());
        //            Utils.SetChildVisible(other.transform, false);
        //            return;
        //        }
        //
        //        if (other.CompareTag("Protect"))
        //        {
        //            StartCoroutine(Protect());
        //            Utils.SetChildVisible(other.transform, false);
        //            return;
        //        }

        //     if (other.CompareTag("Slow"))
        //     {
        //         AddDirt();
        //         return;
        //     }
        //
        //
        //     var playerChoice = other.GetComponent<PlayerChoice>();
        //     if (playerChoice != null)
        //     {
        //         StartCoroutine(Choose(playerChoice));
        //         return;
        //     }
        //
        //     var playerCustomChoice = other.GetComponent<PlayerCustomChoice>();
        //     if (playerCustomChoice != null)
        //     {
        //         StartCoroutine(Choose(playerCustomChoice));
        //         return;
        //     }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Finish"))
            {
                if (CompareTag("Player"))
                {
                    //other.GetComponent<FinishController>().Finish(this);
                }

                enabled = false;
            }
            else if (other.CompareTag("CarpetFinish"))
            {
                if (CompareTag("Player"))
                {
                   // other.GetComponent<CarpetFinishController>().Finish(this);
                }

                enabled = false;
            }
            else if (other.CompareTag("Obstacle"))
            {
                _moveSpeed = _moveSpeedLimit.x;
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Stumble")) _animator.SetTrigger("Stumble");
            }
            else if (other.CompareTag("PoleIn"))
            {
                StopAction();
                //_currentPole = other.GetComponentInParent<PoleManager>();
                //_currentPole.SetCharacter(transform);
                //_animator.SetTrigger("PoleIn");
                //_animator.transform.localEulerAngles = 180 * Vector3.up;
                //other.GetComponentInParent<PoleManager>().isActivated = true;
                return;
            }

            if (other.CompareTag("PoleOut"))
            {
                //_currentPole = null;
                //transform.SetParent(null);
                //transform.rotation = Quaternion.identity;
                //AddImpact(_impact * -1);
                //_animator.SetTrigger("PoleOut");
                //_animator.transform.localEulerAngles = Vector3.zero;
                //other.GetComponentInParent<PoleManager>().isActivated = false;

                ////SFXManager.instance.PlaySFX(SoundType.randomPositive);
                //SetMoving(true);
            }
        }

        public void Finish(bool isWin)
        {
            if (isWin) _animator.SetTrigger("Finish");
        }

        public void ShowEmoji(bool isHappy)
        {
            Debug.Log("ShowEmoji");
            StartCoroutine(ShowEmojiRoutine(isHappy));
        }

        private IEnumerator ShowEmojiRoutine(bool isHappy)
        {
            if (isHappy)
                emojiRenderer.material.SetTexture("_BaseMap", emojiPositive[Random.Range(0, emojiPositive.Length)]);
            else
                emojiRenderer.material.SetTexture("_BaseMap", emojiNegative[Random.Range(0, emojiPositive.Length)]);


           
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
           
        }

        public void Play()
        {
            SetMoving(true);
        }

        public void SetMoving(bool isMoving)
        {
            _isMoving = isMoving;
            _animator.SetBool("IsMoving", isMoving);

            if (!isMoving)
                _animator.SetFloat(speedParam, 0f);
        }

        void CalculateAnimationSpeedPercent(Vector3 velocity, float maxSpeed)
        {
            float currentSpeed = new Vector2(velocity.x, velocity.z).magnitude;
            float speedPercent = Mathf.Clamp01(currentSpeed / maxSpeed);

            _animator.SetFloat(speedParam, speedPercent, 0.12f, Time.deltaTime);
        }

        public void SetDancing(bool isDancing, bool isCanceled)
        {
            _isDancing = isDancing;
            _animator.SetBool("IsDancing", isDancing);

            StopAllCoroutines();
            if (isCanceled) StartCoroutine(CancelDance());
        }

        private IEnumerator CancelDance()
        {
            yield return new WaitForSeconds(1);
            SetDancing(false, false);
        }
    }
}




