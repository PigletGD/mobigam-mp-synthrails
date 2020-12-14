using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnScreenStick : MonoBehaviour, UnityEngine.EventSystems.IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image JoystickParent;
    public Image Stick;

    public Vector2 JoystickVector;

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos = Vector2.zero;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(JoystickParent.rectTransform, eventData.position, eventData.pressEventCamera, out localPos))
        {
            float half_w = JoystickParent.rectTransform.rect.width / 2.0f;
            float half_h = JoystickParent.rectTransform.rect.height / 2.0f;

            float x = localPos.x / half_w;
            float y = localPos.y / half_h;

            JoystickVector = new Vector2(x, y);
            if (JoystickVector.magnitude > 1)
                JoystickVector.Normalize();

            Stick.rectTransform.localPosition = new Vector2(JoystickVector.x * half_w, JoystickVector.y * half_h);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        JoystickVector = Vector2.zero;
    }
}
