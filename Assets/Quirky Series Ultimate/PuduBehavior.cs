using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    public float walkingSpeed;
    public float walkDuration;
    public float stopDuration;
    public float turnAngleRange;
    public Animator animator;

    private CharacterController controller;
    private float walkTimer;
    private float stopTimer;
    private bool isWalking = true;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        walkTimer = walkDuration;
    }

    private void Update()
    {
        if (isWalking)
        {
            Vector3 movement = new Vector3(transform.forward.x * walkingSpeed, 0f, transform.forward.z * walkingSpeed);
            controller.SimpleMove(movement);
            walkTimer -= Time.deltaTime;

            if (walkTimer <= 0f)
            {
                isWalking = false;
                animator.SetBool("IsWalking", false);
                stopTimer = stopDuration;
            }
        }
        else
        {
            stopTimer -= Time.deltaTime;

            if (stopTimer <= 0f)
            {
                isWalking = true;
                animator.SetBool("IsWalking", true);
                RandomTurn();
                walkTimer = walkDuration;
                controller.Move(Vector3.zero); // Reset the character's position in the y-axis
            }
        }
    }

    private void RandomTurn()
    {
        float randomAngle = Random.Range(-turnAngleRange, turnAngleRange);
        transform.Rotate(0f, randomAngle, 0f);
    }
}
