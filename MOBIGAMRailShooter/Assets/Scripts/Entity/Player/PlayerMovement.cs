using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Transform ownerTransform = null;
    public Transform planeModelTransform = null;
    public Transform aimTargetTransform = null;

    public PlayerInfo playerInfo;
    public float moveSpeed = 10;
    public float lookSpeed = 340;
    public float targetDepth = 5;

    Vector3 previousVector = Vector3.zero;

    public OnScreenStick moveJoystick = null;
    
    public TouchPanel touchPanel = null;
    public OnScreenStick aimJoystick = null;

    private bool currentlyDragging = false;

    private bool isRolling = false;
    private float rollTick = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        ownerTransform = transform;

        if (SystemInfo.supportsGyroscope) Input.gyro.enabled = true;
        else Debug.LogError("No Gyroscope in Device");

        touchPanel.OnDragging += OnDragging;
        touchPanel.OnDragRelease += OnDragRelease;
    }

    private void OnDisable()
    {
        touchPanel.OnDragging -= OnDragging;
        touchPanel.OnDragRelease -= OnDragRelease;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float h = moveJoystick.JoystickVector.x;
        float v = moveJoystick.JoystickVector.y;

        if (!isRolling)
        {
            LocalMove(h, v);
            ClampPosition();

            if (SystemInfo.supportsGyroscope)
            {
                Vector3 rot = Input.gyro.rotationRate;

                if (rot.z <= -2.0f) StartCoroutine(BarrelRoll(new Vector3(1.0f, 0.0f, 0.0f)));
                else if (rot.z >= 2.0f) StartCoroutine(BarrelRoll(new Vector3(-1.0f, 0.0f, 0.0f)));
            }
        }

        if (!currentlyDragging)
        {
            RotationLook(h, v);
            HorizontalLean(planeModelTransform, h, 80, 0.1f);
        }
    }

    private void OnDragging(object sender, DragEventArgs e)
    {
        if (!currentlyDragging) currentlyDragging = true;

        float h = aimJoystick.JoystickVector.x;
        float v = aimJoystick.JoystickVector.y;

        RotationLook(h, v);
        HorizontalLean(planeModelTransform, h, 80, 0.1f);
    }

    private void OnDragRelease(object sender, DragEventArgs e) => currentlyDragging = false;

    private void LocalMove(float x, float y)
    {
        Vector3 result = Vector3.Lerp(previousVector, new Vector3(x, y, 0), 0.2f);
        ownerTransform.localPosition += result * playerInfo.movementSpeed * Time.deltaTime;
        previousVector = result;
    }

    private void ClampPosition()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(ownerTransform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);

        ownerTransform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    private void RotationLook(float h, float v)
    {
        aimTargetTransform.parent.position = Vector3.zero;
        aimTargetTransform.localPosition = new Vector3(h, v, targetDepth);
        ownerTransform.rotation = Quaternion.RotateTowards(ownerTransform.rotation, Quaternion.LookRotation(aimTargetTransform.position), Mathf.Deg2Rad * lookSpeed);
    }

    private void HorizontalLean(Transform target, float axis, float leanLimit, float lerpTime)
    {
        Vector3 targetEulerAngles = target.localEulerAngles;
        target.localEulerAngles = new Vector3(targetEulerAngles.x, targetEulerAngles.y, Mathf.LerpAngle(targetEulerAngles.z, -axis * leanLimit, lerpTime));
    }

    private IEnumerator BarrelRoll(Vector3 direction)
    {
        float tick = 0.0f;
        float rollSpeed = playerInfo.movementSpeed * 5;

        isRolling = true;

        do
        {
            tick += Time.deltaTime;

            ownerTransform.position += direction * rollSpeed * Time.deltaTime;

            rollSpeed = Mathf.Lerp(rollSpeed, playerInfo.movementSpeed, tick / rollTick);

            ClampPosition();

            yield return null;

        } while (tick < rollTick);

        isRolling = false;

        previousVector = direction;
    } 
}