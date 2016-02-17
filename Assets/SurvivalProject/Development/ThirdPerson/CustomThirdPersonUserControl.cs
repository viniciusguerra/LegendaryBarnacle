using System;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(CustomThirdPersonCharacter))]
    public class CustomThirdPersonUserControl : MonoBehaviour
    {
        private CustomThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;

        float horizontalInput, forwardInput;
        public float horizontalInputLerpStep, forwardInputLerpStep;

        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.");
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<CustomThirdPersonCharacter>();
        }

        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            // read inputs
            horizontalInput = Mathf.Lerp(XboxOneInput.Instance.GetAxis(XboxOneAxis.LeftThumbX), horizontalInput, horizontalInputLerpStep);
            forwardInput = Mathf.Lerp(XboxOneInput.Instance.GetAxis(XboxOneAxis.LeftThumbY), forwardInput, forwardInputLerpStep);
            //bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = forwardInput * m_CamForward + horizontalInput * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = forwardInput * Vector3.forward + horizontalInput * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move);
        }
    }
}
