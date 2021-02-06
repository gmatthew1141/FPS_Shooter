using UnityEngine;
using EZCameraShake;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    private GameSetting gameSetting;
    private Vector3 velocity;
    private PlayerStatus playerStatus;

    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] public LayerMask groundMask;
    [SerializeField] private Transform cam;

    private float walkingSpeed = 15f;
    private float runningSpeed = 20f;
    private float jumpingForce = 5f;

    private bool isGrounded;
    private float xRotation = 0f;

    // Weapon recoil variables
    private float upRecoil;
    private float sideRecoil;
    private float recoilSpeed;


    private void Awake() {
        gameSetting = FindObjectOfType<GameSetting>();
        playerStatus = GetComponent<PlayerStatus>();
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        recoilSpeed = gameSetting.recoilSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        float movement = Input.GetAxis("Vertical"); 
        float strave = Input.GetAxis("Horizontal");

        float mouseX = sideRecoil + Input.GetAxis("Mouse X");
        float mouseY = upRecoil + Input.GetAxis("Mouse Y");
        sideRecoil -= recoilSpeed * Time.deltaTime;             // reset side recoil
        upRecoil -= recoilSpeed * Time.deltaTime;               // reset up recoil

        if (sideRecoil < 0) {
            sideRecoil = 0;
        }

        if (upRecoil < 0) {
            upRecoil = 0;
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.localRotation = Quaternion.Euler(xRotation, 0, 0);
        CameraShaker.Instance.RestRotationOffset = cam.localRotation.eulerAngles;       // update camera rotation for recoil script
        transform.Rotate(Vector3.up * mouseX);

        Vector3 move = transform.right * strave + transform.forward * movement;

        // cannot move while dead
        if (playerStatus.GetPlayerState() == PlayerState.ALIVE) {
            if (Input.GetButton("Run")) {
                controller.Move(move * runningSpeed * Time.deltaTime);
            } else {
                controller.Move(move * walkingSpeed * Time.deltaTime);
            }

            if (Input.GetButtonDown("Jump") && isGrounded) {
                velocity.y = Mathf.Sqrt(jumpingForce * -2f * gravity);    //sqrt(jumpheight * -2f * gravity) -> formula for realistic jump
            }

        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void AddRecoil(float up, float side) {
        Debug.Log("Recoil added. up: " + up + " side: " + side);
        upRecoil = up;
        sideRecoil = side;
    }

}
