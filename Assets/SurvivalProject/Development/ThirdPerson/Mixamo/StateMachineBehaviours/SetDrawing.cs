using UnityEngine;
using System.Collections;

public class SetDrawing : StateMachineBehaviour
{
    public bool drawing;
    public bool holstering;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(drawing)
            animator.SetBool(CustomCharacterController.a_drawing, true);

        if (holstering)
        {
            animator.SetBool(CustomCharacterController.a_holstering, true);
            SceneManager.Instance.MainCamera.CameraMode = CustomCameraMode.ThirdPerson;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (drawing)
        {
            animator.SetBool(CustomCharacterController.a_drawing, false);
            SceneManager.Instance.MainCamera.CameraMode = CustomCameraMode.FirstPerson;
        }

        if (holstering)
            animator.SetBool(CustomCharacterController.a_holstering, false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
