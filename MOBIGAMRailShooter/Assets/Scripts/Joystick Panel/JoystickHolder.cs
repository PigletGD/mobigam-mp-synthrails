using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class JoystickHolder : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public OnScreenStick Joystick = null;
    public Image hitBox;

    public void OnDrag(PointerEventData eventData)
    {
        Joystick.OnDrag(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 localPos = Vector2.zero;
        RectTransform rectTransform = Joystick.GetComponent<RectTransform>();

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(hitBox.rectTransform, eventData.position, eventData.pressEventCamera, out localPos))
        {
            Joystick.gameObject.SetActive(true);
            rectTransform.localPosition = localPos;
            Joystick.OnPointerDown(eventData);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Joystick.gameObject.SetActive(false);
        Joystick.OnPointerUp(eventData);
    }
}