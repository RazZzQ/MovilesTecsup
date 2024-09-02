using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TouchControl : MonoBehaviour
{
    [Header("Tap Parameters")]
    [SerializeField, Range(0.1f, 0.5f)] protected float doubleTapTreshHold = 0.25f;

    [Header("Press & Drag Parameters")]
    [SerializeField, Range(0.1f, 0.5f)] protected float _pressTreshHold = 0.25f;

    [Header("Swipe Parameters")]
    [SerializeField] protected float swipeMinDistance = 50f;
    [SerializeField] protected float swipeMaxDistance = 500f;

    [Header("Events")]
    [SerializeField] protected UnityEvent<Vector3> OnTap;
    [SerializeField] protected UnityEvent<Vector3> OnDoubleTap;
    [SerializeField] protected UnityEvent<Vector3> OnPress;
    [SerializeField] protected UnityEvent<Vector3> OnDrag;
    [SerializeField] protected UnityEvent OnSwipe;

    private float _tapTouchTime = 0f;
    private Vector2 _startPosition;
    private Vector2 _endPosition;
    private bool _canDrag;
    private bool _hasDoubleTapped;
    private float _touchTimeElapsed = 0f;
    private float _swipeDistance;
    private float _minDistance;
    private float _maxDistance;
    private int _tapCount = 0;

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

    // Nuevo Input System - Callbacks directos

    public void OnTapPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector3 touchPosition = GetWorldPositionFromScreen(screenPosition);
        HandleTap(touchPosition);
    }

    public void OnPressPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector3 touchPosition = GetWorldPositionFromScreen(screenPosition);
        HandlePress(touchPosition);
    }

    public void OnDragPerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector3 touchPosition = GetWorldPositionFromScreen(screenPosition);
        HandleDrag(touchPosition);
    }

    public void OnSwipePerformed(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Vector2 screenPosition = context.ReadValue<Vector2>();
        Vector3 touchPosition = GetWorldPositionFromScreen(screenPosition);
        HandleSwipe(touchPosition);
    }

    private void HandleTap(Vector3 touchPosition)
    {
        _tapCount++;

        if (_tapCount == 1)
        {
            // Primer toque registrado
            _startPosition = touchPosition;
            _tapTouchTime = Time.time;
            Invoke("CheckSingleTap", doubleTapTreshHold);
        }
        else if (_tapCount == 2 && (Time.time - _tapTouchTime) <= doubleTapTreshHold)
        {
            // Segundo toque registrado dentro del intervalo permitido para un double tap
            CancelInvoke("CheckSingleTap");
            Debug.Log("Double Tap");
            OnDoubleTap?.Invoke(touchPosition);
            _tapCount = 0; // Resetear el contador de toques
        }
    }

    private void CheckSingleTap()
    {
        if (_tapCount == 1)
        {
            // Si no se detecta un segundo toque, se considera como toque simple
            Debug.Log("Single Tap");
            OnTap?.Invoke(_startPosition);
        }

        _tapCount = 0; // Resetear el contador de toques
    }

    private void HandlePress(Vector3 touchPosition)
    {
        if (_hasDoubleTapped) return;

        _touchTimeElapsed += Time.deltaTime;

        if (_touchTimeElapsed > _pressTreshHold)
        {
            Debug.Log("Press");
            _canDrag = true;
            OnPress?.Invoke(touchPosition);
        }
    }

    private void HandleDrag(Vector3 touchPosition)
    {
        if (_hasDoubleTapped) return;

        if (_canDrag)
        {
            Debug.Log("Drag");
            OnDrag?.Invoke(touchPosition);
        }
    }

    private void HandleSwipe(Vector3 touchPosition)
    {
        if (_hasDoubleTapped) return;

        _endPosition = touchPosition;
        _swipeDistance = Vector2.SqrMagnitude(_endPosition - _startPosition);

        if (_swipeDistance >= _minDistance && _swipeDistance <= _maxDistance)
        {
            Debug.Log("Swipe Detected");
            OnSwipe?.Invoke();
        }
    }

    private Vector3 GetWorldPositionFromScreen(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        Plane plane = new Plane(Vector3.up, Vector3.forward);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance);
        }

        return Vector3.forward;
    }
}