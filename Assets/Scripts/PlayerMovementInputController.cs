using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class PlayerMovementInputController : MonoBehaviour
{
    [SerializeField] float rotationPower = 3f;
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float sprintSpeed = 3f;
    [SerializeField] Transform followTransform;
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animator;

    Vector2 moveInput;
    Vector2 lookInput;
    float sprintInput;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        moveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        lookInput = new Vector2(Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"));
        sprintInput = Input.GetAxis("Sprint");

        UpdateFollowTargetRotation();

        float speed = Mathf.Lerp(walkSpeed, sprintSpeed, sprintInput);
        Vector3 movement = (transform.forward * moveInput.y * speed * Time.deltaTime) + (transform.right * moveInput.x * speed * Time.deltaTime);
        characterController.Move(movement);

        animator.SetFloat("Horizontal", moveInput.x * speed);
        animator.SetFloat("Vertical", moveInput.y * speed);

        //only rotate the player when moving, allows user to look at the player when idle
        if (moveInput.magnitude > 0.01f)
        {
            //Set the player rotation based on the look transform
            transform.rotation = Quaternion.Euler(0, followTransform.eulerAngles.y, 0);
            //reset the y rotation of the look transform
            followTransform.localEulerAngles = new Vector3(followTransform.localEulerAngles.x, 0, 0);
        }
    }

    private void UpdateFollowTargetRotation()
    {
        //Update follow target rotation
        followTransform.rotation *= Quaternion.AngleAxis(lookInput.x * rotationPower, Vector3.up);
        followTransform.rotation *= Quaternion.AngleAxis(lookInput.y * rotationPower, Vector3.right);

        var angles = followTransform.localEulerAngles;
        angles.z = 0;
        var angle = angles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }
        followTransform.localEulerAngles = angles;
    }
}
