using UnityEngine;
using System.Collections;

public class MainCamera : Singleton<MainCamera>
{
    public const int outlineLayer = 8;

    public Transform target;
    public bool follow = true;

    public Vector3 relativePosition;
    public Vector3 relativeRotation;

    public float cameraFollowSpeed = 1;
    public float setTransformTime = 1.5f;
    public float horizontalOffset;

    Vector3 targetPosition;
    Vector3 positionDelta;
    Vector3 positionStep;

    private void FollowTarget()
    {        
        if(transform.position != targetPosition)
            transform.position = positionStep;

        transform.rotation = Quaternion.Euler(relativeRotation);
    }

    public void SetTransform(Transform targetTransform)
    {
        iTween.StopByName(GetInstanceID() + "Offset");
        iTween.StopByName(GetInstanceID() + "PositionReset");
        iTween.StopByName(GetInstanceID() + "RotationReset");

        follow = false;

        iTween.MoveTo(gameObject, iTween.Hash("name", GetInstanceID() + "Position", "position", targetTransform.position, "time", setTransformTime));
        iTween.RotateTo(gameObject, iTween.Hash("name", GetInstanceID() + "Rotation", "rotation", targetTransform, "time", setTransformTime));        
    }

    public void ResetTransform()
    {
        iTween.StopByName(GetInstanceID() + "Position");
        iTween.StopByName(GetInstanceID() + "Rotation");

        iTween.MoveTo(gameObject, iTween.Hash("name", GetInstanceID() + "PositionReset", "position", target.position + relativePosition, "time", setTransformTime));
        iTween.RotateTo(gameObject, iTween.Hash("name", GetInstanceID() + "RotationReset", "rotation", relativeRotation, "time", setTransformTime, "oncomplete", "SetFollow", "oncompleteparams", true));
    }

    private void SetFollow(bool value)
    {
        follow = value;
    }

    void LateUpdate()
    {
        targetPosition = target.position + relativePosition;
        positionDelta = (targetPosition - transform.position) * Time.deltaTime * cameraFollowSpeed;
        positionStep = transform.position + positionDelta;

        if (follow)
            FollowTarget();
    }

    void Start()
    {
        transform.position = target.position + relativePosition;
        transform.rotation = Quaternion.Euler(relativeRotation);
    }
}
