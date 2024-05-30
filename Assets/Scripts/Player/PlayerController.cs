using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float MoveSpeed;
    public float JumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action inventory;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if(canLook)
        {
            CameraLook();
        }
    }

    void Move()
    {
        //방향값을 정해주고
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        //이동속도 값을 곱해주고
        dir *= MoveSpeed;
        //y값 초기화
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);

        // 엔진에서 + 가면 밑을 보고 -를 하면 위로 봄 그래서 -로 반대
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        //delta x값을 y에 넣어야 됨 y축으로 이동하니까
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensivity, 0);
    }

    // 상태에 따라 인풋시스템에 입력한 키를 입력했을 때 값(1,0)등 을 받아옴
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started && IsGrounded()) 
        {
            _rigidbody.AddForce(Vector2.up * JumpPower, ForceMode.Impulse);
        }
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up*0.01f),Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up*0.01f),Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up*0.01f),Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up*0.01f),Vector3.down)
        };

        for(int i = 0; i<rays.Length; i++)
        {
            if (Physics.Raycast(rays[i],0.1f,groundLayerMask))
            {
                return true;
            }
        }
        return false;
        
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }
}
