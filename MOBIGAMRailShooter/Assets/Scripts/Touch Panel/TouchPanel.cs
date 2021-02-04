using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchPanel : MonoBehaviour, UnityEngine.EventSystems.IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public static TouchPanel Instance;
    public Image hitBox = null;
    public OnScreenStick Joystick = null;

    public TapProperty _tapProperty = new TapProperty();
    public event EventHandler<TapEventArgs> OnTap;

    public SwipeProperty _swipeProperty = new SwipeProperty();
    public event EventHandler<SwipeEventArgs> OnSwipe;

    public DragProperty _dragProperty = new DragProperty();
    public event EventHandler<DragEventArgs> OnDragging;
    public event EventHandler<DragEventArgs> OnDragRelease;


    Touch trackedFinger1;
    bool isTouchingPanel = false;
    bool hasLiftedTouch = false;

    // Point where gesture started
    private Vector2 startPoint = Vector2.zero;
    // Point where gesture ended
    private Vector2 endPoint = Vector2.zero;
    // point where gesture currently is
    private Vector2 currentPoint = Vector2.zero;
    // Time from Began to Ended
    private float gestureTime = 0;

    private void Update()
    {
        if (isTouchingPanel)
        {
            if (hasLiftedTouch)
            {
                // If total gesture time is below max and if covered screen distance is below max
                // For allowance in case of shaky fingers, etc.
                if (gestureTime <= _tapProperty.tapTime &&
                    Vector2.Distance(startPoint, endPoint) < (Screen.dpi * _tapProperty.tapMaxDistance))
                    FireTapEvent(startPoint);

                if (gestureTime <= _swipeProperty.swipeTime &&
                    Vector2.Distance(startPoint, endPoint) >= (Screen.dpi * _swipeProperty.swipeMinDistance))
                    FireSwipeEvent();

                if (gestureTime > _dragProperty.DragBufferTime)
                    FireDragReleaseEvent(startPoint, endPoint);
            }
            else
            {
                gestureTime += Time.deltaTime;

                // If finger has stayed long enough n screen consider it a Drag
                if (gestureTime > _dragProperty.DragBufferTime)
                    FireDraggingEvent(startPoint, currentPoint);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPoint = eventData.position;
        isTouchingPanel = true;
        hasLiftedTouch = false;
        gestureTime = 0.0f;

        Vector2 localPos = Vector2.zero;
        RectTransform rectTransform = Joystick.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hitBox.rectTransform, eventData.position, eventData.pressEventCamera, out localPos))
        {
            rectTransform.localPosition = localPos;
            Joystick.OnPointerDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        endPoint = eventData.position;
        hasLiftedTouch = true;

        Joystick.gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentPoint = eventData.position;

        if (gestureTime > _dragProperty.DragBufferTime) {
            Joystick.OnDrag(eventData);

            if (!Joystick.gameObject.activeSelf)
                Joystick.gameObject.SetActive(true);
        }
    }

    private void FireTapEvent(Vector2 pos)
    {
        if (OnTap != null)
        {
            // Create the event args first
            TapEventArgs tapArgs = new TapEventArgs(pos);
            // On Tap with THIS as the sender + tapArgs
            OnTap(this, tapArgs);
        }

        isTouchingPanel = false;
    }

    private void FireSwipeEvent()
    {
        Vector2 diff = endPoint - startPoint;

        SwipeDirections swipeDir;
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            if (diff.x > 0) swipeDir = SwipeDirections.RIGHT;
            else swipeDir = SwipeDirections.LEFT;

        }
        else
        {
            if (diff.y > 0) swipeDir = SwipeDirections.UP;
            else swipeDir = SwipeDirections.DOWN;
        }

        if (OnSwipe != null)
            OnSwipe(this, new SwipeEventArgs(startPoint, diff, swipeDir));

        isTouchingPanel = false;
    }

    private void FireDraggingEvent(Vector2 startPos, Vector2 curPos)
    {
        if (OnDragging != null)
        {
            DragEventArgs args = new DragEventArgs(startPos, curPos);
            OnDragging(this, args);
        }
    }

    private void FireDragReleaseEvent(Vector2 startPos, Vector2 curPos)
    {
        if (OnDragRelease != null)
        {
            DragEventArgs args = new DragEventArgs(startPos, curPos);
            OnDragRelease(this, args);
        }

        isTouchingPanel = false;
    }
}
