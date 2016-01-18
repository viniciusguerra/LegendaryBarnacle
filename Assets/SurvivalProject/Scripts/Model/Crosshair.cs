using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour
{
    public float minimalDistance;    

    public float movementSpeed;
    public float fadeTime;
    private bool showing;

    private Vector3 playerRelativePosition;

    [SerializeField]
    private PlayerInput character;
    private bool releasedInput;

    public void Show()
    {
        playerRelativePosition = character.transform.TransformPoint(Vector3.forward * minimalDistance);
        transform.position = playerRelativePosition;

        iTween.FadeTo(gameObject, 1, fadeTime);

        if (XboxOneInput.GetAxis(XboxOneAxis.RightThumbX) != 0 || XboxOneInput.GetAxis(XboxOneAxis.RightThumbY) != 0)
            StartCoroutine(CheckForRelease());
        else
            showing = true;
    }

    public void Hide()
    {
        iTween.FadeTo(gameObject, iTween.Hash("alpha", 0, "time", fadeTime, "oncomplete", "SetVisibility", "oncompleteparams", false));
    }

    private void SetVisibility(bool value)
    {
        showing = value;
    }

    public void Control()
    {        
        Vector3 inputDirection = new Vector3(XboxOneInput.GetAxis(XboxOneAxis.RightThumbX), 0, XboxOneInput.GetAxis(XboxOneAxis.RightThumbY));
        inputDirection *= movementSpeed;
        inputDirection *= Time.deltaTime;

        playerRelativePosition += inputDirection;

        transform.position = playerRelativePosition;
    }

    private IEnumerator CheckForRelease()
    {
        while (XboxOneInput.GetAxis(XboxOneAxis.RightThumbX) != 0 || XboxOneInput.GetAxis(XboxOneAxis.RightThumbY) != 0)
            yield return null;

        showing = true;
    }

    void Start()
    {
        iTween.FadeTo(gameObject, 0, 0);
    }

    void Update()
    {        
        transform.LookAt(transform.position + SceneManager.Instance.MainCamera.transform.rotation * Vector3.forward,
            SceneManager.Instance.MainCamera.transform.rotation * Vector3.up);

        if (showing)        
            Control();        
    }
}
