using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] // 이동 관련 변수들
    public float moveSpeed; // 플레이어 이동 속도
    private Vector2 curMovementInput; // 현재 입력된 이동 방향 (X, Y)
    public float jumpPower; // 점프 힘
    public LayerMask groundLayerMask; // 바닥 판정을 위한 레이어 마스크

    [Header("Look")] // 카메라 회전 관련 변수들
    public Transform cameraContainer; // 카메라의 부모 객체 (카메라 회전을 위한 컨테이너)
    public float minXLook; // 카메라의 최소 X 회전 값 (고개를 너무 아래로 숙이지 않도록 제한)
    public float maxXLook; // 카메라의 최대 X 회전 값 (고개를 너무 위로 들지 않도록 제한)
    private float camCurXRot; // 현재 카메라의 X 회전 값
    public float lookSensitivity; // 마우스 감도

    private Vector2 mouseDelta; // 마우스 이동 입력값

    [HideInInspector] // 인스펙터에서 숨김 (다른 스크립트에서는 접근 가능)
    public bool canLook = true; // 카메라 회전 가능 여부

    private Rigidbody rb; // Rigidbody 컴포넌트

    private void Awake()
    {
        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        // 마우스 커서를 화면 중앙에 고정 (잠금 상태)
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        // 물리 업데이트에서 이동 처리
        Move();
    }

    private void LateUpdate()
    {
        // 카메라 회전 처리 (LateUpdate에서 실행하여 부드럽게 동작하도록 함)
        if (canLook)
        {
            CameraLook();
        }
    }

    // 마우스 움직임 입력 처리
    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    // 이동 입력 처리
    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) // 이동 키를 누르고 있을 때
        {
            curMovementInput = context.ReadValue<Vector2>(); // 입력된 이동 방향 값 저장
        }
        else if (context.phase == InputActionPhase.Canceled) // 이동 키를 떼었을 때
        {
            curMovementInput = Vector2.zero; // 이동 중지
        }
    }

    // 점프 입력 처리
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded()) // 점프 키를 눌렀고, 땅에 있을 때만 실행
        {
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse); // 위쪽 방향으로 힘을 가해 점프 실행
        }
    }

    // 플레이어 이동 처리
    private void Move()
    {
        // 입력된 이동 방향을 기준으로 실제 이동 방향 설정
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= moveSpeed; // 이동 속도 적용
        dir.y = rb.velocity.y; // Y 값은 기존 Rigidbody의 속도를 유지 (중력 영향)

        rb.velocity = dir; // Rigidbody의 속도를 변경하여 이동 적용
    }

    // 카메라 회전 처리
    void CameraLook()
    {
        // 마우스 Y 이동값을 누적하여 X축 회전을 조정 (고개를 위/아래로 움직이는 효과)
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook); // 고개가 너무 많이 숙여지거나 들리지 않도록 제한
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0); // 회전 적용

        // 마우스 X 이동값을 기준으로 플레이어 전체를 회전 (좌/우 회전)
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    // 바닥에 닿아있는지 판정하는 함수
    bool IsGrounded()
    {
        // 플레이어의 네 군데에서 바닥을 향해 레이캐스트 실행
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +(transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            // 레이캐스트를 실행하여 바닥과 충돌하는지 검사 (0.1f 거리 내에서 groundLayerMask에 해당하는 오브젝트가 있는지 확인)
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true; // 바닥이 감지되면 true 반환
            }
        }

        return false; // 네 개의 레이 중 하나도 바닥과 충돌하지 않으면 false 반환
    }

    // 마우스 커서 상태 변경 함수
    public void ToggleCursor(bool toggle)
    {
        // 커서 잠금 상태 변경 (toggle이 true이면 커서를 해제하고, false이면 잠금)
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        
        // canLook 값도 변경하여 카메라 회전 가능 여부를 제어
        canLook = !toggle;
    }
}
