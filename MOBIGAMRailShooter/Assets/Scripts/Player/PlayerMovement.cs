using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform ownerTransform = null;
    public Transform planeModelTransform = null;
    public Transform aimTargetTransform = null;

    public float moveSpeed = 10;
    public float lookSpeed = 340;
    public float targetDepth = 5;

    Vector3 previousVector = Vector3.zero;

    public OnScreenStick joystick = null;

    // Start is called before the first frame update
    void Start()
    {
        ownerTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        float h = joystick.JoystickVector.x;
        float v = joystick.JoystickVector.y;

        LocalMove(h, v);
        ClampPosition();
        RotationLook(h, v);
        HorizontalLean(planeModelTransform, h, 80, 0.1f);
    }

    void LocalMove(float x, float y)
    {
        Vector3 result = Vector3.Lerp(previousVector, new Vector3(x, y, 0), 0.2f);
        ownerTransform.localPosition += result * moveSpeed * Time.deltaTime;
        previousVector = result;
    }

    void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(ownerTransform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);

        ownerTransform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    void RotationLook(float h, float v)
    {
        aimTargetTransform.parent.position = Vector3.zero;
        aimTargetTransform.localPosition = new Vector3(h, v, targetDepth);
        ownerTransform.rotation = Quaternion.RotateTowards(ownerTransform.rotation, Quaternion.LookRotation(aimTargetTransform.position), Mathf.Deg2Rad * lookSpeed);
    }

    void HorizontalLean(Transform target, float axis, float leanLimit, float lerpTime)
    {
        Vector3 targetEulerAngles = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngles.x, targetEulerAngles.y, Mathf.LerpAngle(targetEulerAngles.z, -axis * leanLimit, lerpTime));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(previousVector, 0.5f);
        Gizmos.DrawSphere(previousVector, 0.15f);
    }
}