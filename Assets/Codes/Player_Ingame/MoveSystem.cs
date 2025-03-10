using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveSystem : MonoBehaviour
{
    [Header("카메라")]
    public float mouseSpeed;
    private float _xRot;
    private float _yRot;
    private Vector2 _mouseDelta;
    Camera cam;
    [Header("플레이어 움직임")]
    [SerializeField] public float moveSpeed = 1;
    [HideInInspector] public Vector3 moveVector =  Vector3.zero;
    private Vector3 _inputVector = Vector3.zero;
    [HideInInspector] public Vector3 currentVelocity = Vector3.zero;
    private Rigidbody rb;
    public float maxSpeed;
    public float acceleration;
    public float deceleration;

    private void Awake()
    {
        rb =  GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cam = Camera.main;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector3 targetVelocity = moveVector * maxSpeed;
        moveVector = transform.forward * _inputVector.y + transform.right * _inputVector.x;
        if (moveVector.magnitude > 0.1)
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentVelocity = Vector3.MoveTowards(currentVelocity, Vector3.zero, deceleration * Time.fixedDeltaTime);
        }
        rb.linearVelocity = new Vector3(currentVelocity.x, rb.linearVelocity.y, currentVelocity.z);
    }

    private void Update()
    {
        Rotate();
    }

    public void Move(InputAction.CallbackContext context)
    {
        _inputVector =  context.ReadValue<Vector2>();
    }
    void Rotate()
    {
        _yRot -= _mouseDelta.y * mouseSpeed * Time.deltaTime;
        _xRot += _mouseDelta.x * mouseSpeed * Time.deltaTime;
        _yRot = Mathf.Clamp(_yRot, -90f, 90f);
        cam.transform.rotation = Quaternion.Euler(_yRot,_xRot , 0);
        rb.rotation = Quaternion.Euler(0, _xRot, 0);
    }
    public void MouseMove(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
        Debug.Log(_mouseDelta);
    }
}
