using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public EntityType playerType = EntityType.RED;
    public int health = 3;
    public float movementSpeed = 10;
    public int ammoRed = 20;
    public int ammoGreen = 20;
    public int ammoBlue = 20;

    public TouchPanel touchPanel = null;

    private MeshRenderer MR = null;
    private Color currentColor = Color.white;
    private Color targetColor = Color.white;
    private float currentLerpTime = 0.0f;
    private float lerpTime = 0.1f;
    private bool transitioningColor = false;
    private float invincibilityTime = 0.5f;
    private float currentInvincibilityTime = 0.75f;

    [SerializeField] ObjectPool explosionPool = null;

    [SerializeField] GameHUD gameHUD = null;

    [SerializeField] GameEventsSO onPlayerDeath = null;
    private void Awake()
    {
        MR = GetComponentInChildren<MeshRenderer>();

        SetPlayerType(EntityType.RED);
        currentColor = Color.red;

        AudioManager.Instance.PlayLoop("GameMusic");
    }

    private void Start()
    {
        touchPanel.OnSwipe += OnSwipe;

        SaveState saveState = SaveManager.Instance.state;

        health = saveState.maxHealth;
        movementSpeed = saveState.movementSpeed;
        ammoRed = saveState.ammoCapacity;
        ammoBlue = saveState.ammoCapacity;
        ammoGreen = saveState.ammoCapacity;
    }

    private void Update()
    {
        if (currentInvincibilityTime < invincibilityTime)
            currentInvincibilityTime += Time.deltaTime;

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

    public void TakeDamage(int damage)
    {
        if (SaveManager.Instance.state.invincibilityOn || currentInvincibilityTime < invincibilityTime) return;

        health -= damage;

        gameHUD.RemoveHeart();

        currentInvincibilityTime = 0;

        if (health <= 0)
            Die();
    }

    public void Die()
    {
        GameObject go = explosionPool.RetrieveObject();
        go.transform.position = transform.position;

        gameHUD.DisplayResults();

        onPlayerDeath.Raise();

        gameObject.SetActive(false);
    }
}
