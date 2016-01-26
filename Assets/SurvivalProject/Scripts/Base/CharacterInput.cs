using UnityEngine;
using System.Collections;

public class CharacterInput : MonoBehaviour
{
    protected Character character;
    public Character Character { get { return character; } }

    public float walkSpeed;
    public float averageSpeed;
    public float currentSpeed;
    public float maxDegreesDelta = 300;

    protected CharacterController characterController;   

    //used for calculating movement direction
    protected Vector3 lastPosition;
    //used for interpolating rotation
    protected Quaternion targetRotation;
    protected float stickRotationThreshold = 0.05f;
    protected float movementLookThreshold = 0.05f;

    protected void Move(Vector3 movementDirection)
    {
        characterController.Move(movementDirection);
    }

    protected void Aim(Quaternion targetRotation)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxDegreesDelta * Time.deltaTime);
    }

    protected void SetMovementState(MovementState state)
    {
        switch (state)
        {
            case MovementState.Walking:
                {
                    currentSpeed = walkSpeed;
                    break;
                }
            case MovementState.Running:
                {
                    currentSpeed = averageSpeed;
                    break;
                }
            default:
                break;
        }
    }    

    protected virtual void Start()
    {
        characterController = GetComponent<CharacterController>();
        character = GetComponent<Character>();     
    }    
}
