using System;
using UnityEngine;
using UnityEngine.Events;

namespace Moviles.Week1
{
    public class TouchController : MonoBehaviour
    {
        [Header("Tap Parameters")]
        [SerializeField, Range(0.1f, 0.5f)] protected float doubleTapTreshHold = 0.25f;

        [Header("Press & Drag Parameters")]
        [SerializeField, Range(0.1f, 0.5f)] protected float _pressTreshHold = 0.25f;

        [Header("Swipe Parameters")]
        [SerializeField] protected float swipeMinDistance;
        [SerializeField] protected float swipeMaxDistance;
        [SerializeField] protected float swipeDistanceSqr;
        [SerializeField] protected float _minDistance;
        [SerializeField] protected float _maxDistance;
        [SerializeField] protected float _swipeDistance;
        [SerializeField] protected float _swipeTime;

        [Header("Events")]
        [SerializeField] protected UnityEvent<Vector3> OnTap;
        [SerializeField] protected UnityEvent<Vector3> OnDoubleTap;
        [SerializeField] protected UnityEvent<Vector3> OnPress;
        [SerializeField] protected UnityEvent<Vector3> OnDrag;
        [SerializeField] protected UnityEvent OnSwipe;

        protected float _tapTouchTime = 0f;

        private Vector2 _startPosition;
        private Vector2 _endPosition;

        private bool _canDrag;
        private bool _hasDoubletTouched;
        private float _touchTimeElapsed = 0f;

        private void Start()
        {
            UpdateVelocityThreshold();
        }

        [ContextMenu("Update Velocities")]
        private void UpdateVelocityThreshold()
        {
            _minDistance = swipeMinDistance * swipeMinDistance;

            _maxDistance = swipeMaxDistance * swipeMaxDistance;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if(touch.phase == TouchPhase.Began)
                {
                    SimpleTouchLogic(touch.position);
                }

                if (touch.phase == TouchPhase.Stationary)
                {
                    PressTouchLogic(touch.position);
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    DragTouchLogic(touch.position);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    EndTouchLogic(touch.position);
                }
            }
        }

        private void SimpleTouchLogic(Vector3 touchPosition)
        {
            _startPosition = touchPosition;

            float _currentTime = Time.time;

            float _deltaTime = _currentTime - _tapTouchTime;

            if (_deltaTime <= doubleTapTreshHold)
            {
                Debug.Log("Double Tap");

                _hasDoubletTouched = true;

                OnDoubleTap?.Invoke(touchPosition);
            }
            else
            {
                //Debug.Log("Simple Tap");
            }

            _tapTouchTime = Time.time;

            _touchTimeElapsed = 0;

            _canDrag = false;
        }

        private void PressTouchLogic(Vector3 touchPosition)
        {
            if (_hasDoubletTouched) return;

            _touchTimeElapsed += Time.deltaTime;

            if (_touchTimeElapsed > _pressTreshHold)
            {
                Debug.Log("Is Pressed");

                _canDrag = true;

                OnPress?.Invoke(touchPosition);
            }
        }

        private void DragTouchLogic(Vector3 touchPosition)
        {
            if (_hasDoubletTouched) return;

            if (_canDrag)
            {
                Debug.Log("We Moving");

                OnDrag?.Invoke(touchPosition);
            }
        }

        private void EndTouchLogic(Vector3 touchPosition)
        {
            if (_hasDoubletTouched)
            {
                _hasDoubletTouched = false;
                return;
            }

            if (_canDrag)
            {
                return;
            }

            float _currentTime = Time.time;

            float _deltaTime = _currentTime - _tapTouchTime;

            _endPosition = touchPosition;

            swipeDistanceSqr = Vector2.Distance(_endPosition, _startPosition); //NO EFFICIENT
            _swipeDistance = Vector2.SqrMagnitude(_endPosition - _startPosition);
            _swipeTime = _deltaTime;

            if (_minDistance < _swipeDistance && _maxDistance > _swipeDistance)
            {
                Debug.Log("That's a SWIPE!");

                OnSwipe?.Invoke();

            }else if (_deltaTime >= doubleTapTreshHold)
            {
                Debug.Log("Simple Tap");

                OnTap?.Invoke(touchPosition);
            }
        }
    }
}