using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public EntityType playerType = EntityType.RED;
    public int health = 3;
    public float movementSpeed = 10;

    public TouchPanel touchPanel;

    private MeshRenderer MR;
    private Color currentColor;
    private Color targetColor;
    private float currentLerpTime = 0.0f;
    private float lerpTime = 0.1f;
    private bool transitioningColor = false;

    private void Awake()
    {
        MR = GetComponentInChildren<MeshRenderer>();

        SetPlayerType(EntityType.RED);
        currentColor = Color.red;
    }

    private void Start()
    {
        touchPanel.OnSwipe += OnSwipe;
    }

    private void Update()
    {
        if (transitioningColor)
        {
            currentLerpTime += Time.deltaTime;
            float lerpValue = currentLerpTime / lerpTime;

            Color lerpedColor = Color.Lerp(currentColor, targetColor, lerpValue);

            if (currentLerpTime > lerpTime)
            {
                MR.material.SetColor("_EmissionColor", targetColor);
                transitioningColor = false;
            }
            else MR.material.SetColor("_EmissionColor", lerpedColor);
        }
    }

    private void OnSwipe(object sender, SwipeEventArgs e)
    {
        if (e.SwipeDirection == SwipeDirections.LEFT)
        {
            switch (playerType)
            {
                case EntityType.RED: SetPlayerType(EntityType.BLUE); break;
                case EntityType.BLUE: SetPlayerType(EntityType.GREEN); break;
                case EntityType.GREEN: SetPlayerType(EntityType.RED); break;
            }
        }
        else if (e.SwipeDirection == SwipeDirections.RIGHT)
        {
            switch (playerType)
            {
                case EntityType.RED: SetPlayerType(EntityType.GREEN); break;
                case EntityType.BLUE: SetPlayerType(EntityType.RED); break;
                case EntityType.GREEN: SetPlayerType(EntityType.BLUE); break;
            }
        }
    }

    public void SetPlayerType(EntityType newPlayerType)
    {
        playerType = newPlayerType;

        if (MR != null)
        {
            switch (playerType)
            {
                case EntityType.RED: targetColor = Color.red; break;
                case EntityType.BLUE: targetColor = Color.blue; break;
                case EntityType.GREEN: targetColor = Color.green; break;
            }
        }

        currentColor = MR.material.GetColor("_EmissionColor");

        currentLerpTime = 0.0f;

        transitioningColor = true;
    }
}
