using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public static TouchPanel Instance;

    public TapProperty _tapProperty;
    public event EventHandler<TapEventArgs> OnTap;

    public SwipeProperty _swipeProperty;
    public event EventHandler<SwipeEventArgs> OnSwipe;

    public DragProperty _dragProperty;
    public event EventHandler<DragEventArgs> OnDrag;

    Touch trackedFinger1;
    bool isTouchingPanel = false;
    bool hasLiftedTouch = false;

    // Point where gesture started
    private Vector2 startPoint = Vector2.zero;
    // Point where gesture ended
    private Vector2 endPoint = Vector2.zero;
    // Time from Began to Ended
    private float gestureTime = 0;

    /*
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }
    */

    // Update is called once per frame
    /*
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            trackedFinger1 = Input.GetTouch(0);

            if (trackedFinger1.phase == TouchPhase.Began)
            {
                startPoint = trackedFinger1.position;
                gestureTime = 0;
            }

            if (trackedFinger1.phase == TouchPhase.Ended)
            {
                endPoint = trackedFinger1.position;
                // If total gesture time is below max and if covered screen distance is below max
                // For allowance in case of shaky fingers, etc.
                if (gestureTime <= _tapProperty.tapTime &&
                    Vector2.Distance(startPoint, endPoint) < (Screen.dpi * _tapProperty.tapMaxDistance))
                    FireTapEvent(startPoint);

                if (gestureTime <= _swipeProperty.swipeTime &&
                    Vector2.Distance(startPoint, endPoint) >= (Screen.dpi * _swipeProperty.swipeMinDistance))
                    FireSwipeEvent();
            }
            else
            {
                gestureTime += Time.deltaTime;

                // If finger has stayed long enough n screen consider it a Drag
                if (gestureTime > -_dragProperty.DragBufferTime)
                    FireDragEvent();
            }
        }
    }
    */

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
            }
            else
            {
                gestureTime += Time.deltaTime;

                // If finger has stayed long enough n screen consider it a Drag
                if (gestureTime > _dragProperty.DragBufferTime)
                    FireDragEvent();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPoint = eventData.position;
        isTouchingPanel = true;
        hasLiftedTouch = false;
        gestureTime = 0.0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        endPoint = eventData.position;
        hasLiftedTouch = true;
    }

    private void FireTapEvent(Vector2 pos)
    {
        Debug.Log("TAP!");

        if (OnTap != null)
        {
            Ray r = Camera.main.ScreenPointToRay(pos);
            RaycastHit h = new RaycastHit();
            GameObject hitObj = null;

            if (Physics.Raycast(r, out h, Mathf.Infinity))
                hitObj = h.collider.gameObject;

            // Create the event args first
            TapEventArgs tapArgs = new TapEventArgs(pos, hitObj);
            // On Tap with THIS as the sender + tapArgs
            OnTap(this, tapArgs);
        }

        isTouchingPanel = false;
    }

    private void FireSwipeEvent()
    {
        Debug.Log("SWIPE");

        /*Vector2 diff = endPoint - startPoint;

        Ray r = Camera.main.ScreenPointToRay(startPoint);
        RaycastHit hit = new RaycastHit();
        GameObject hitObj = null;

        if (Physics.Raycast(r, out hit, Mathf.Infinity))
        {
            hitObj = hit.collider.gameObject;
        }

        SwipeDirections swipeDir;
        if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
        {
            if (diff.x > 0)
            {
                Debug.Log("RIGHT");
                swipeDir = SwipeDirections.RIGHT;
            }
            else
            {
                Debug.Log("LEFT");
                swipeDir = SwipeDirections.LEFT;
            }
        }
        else
        {
            if (diff.y > 0)
            {
                Debug.Log("UP");
                swipeDir = SwipeDirections.UP;
            }
            else
            { 
                Debug.Log("DOWN");
                swipeDir = SwipeDirections.DOWN;
            }
        }*/

        //if (OnSwipe != null)
        //{
        //OnSwipe(this, new SwipeEventArgs(startPoint, diff, swipeDir, hitObj));

        /*if (hitObj != null)
        {
            ISwipped swipe = hitObj.GetComponent<ISwipped>();

            if (swipe != null) swipe.OnSwipe(new SwipeEventArgs(startPoint, diff, swipeDir, hitObj));
        }*/
        //}

        isTouchingPanel = false;
    }

    private void FireDragEvent()
    {
        Debug.Log($"Dragging {trackedFinger1.position.ToString()}");

        /*Ray r = Camera.main.ScreenPointToRay(trackedFinger1.position);
        RaycastHit hit = new RaycastHit();
        GameObject hitObj = null;

        if (Physics.Raycast(r, out hit, Mathf.Infinity))
            hitObj = hit.collider.gameObject;

        DragEventArgs args = new DragEventArgs(trackedFinger1, hitObj);*/
        /*
        if(OnDrag != null)
            OnDrag(this, args);
        */

        /*if (hitObj != null)
        {
            IDragged drag = hitObj.GetComponent<IDragged>();
            if (drag != null)
                drag.OnDrag(args);
        }*/
    }
}