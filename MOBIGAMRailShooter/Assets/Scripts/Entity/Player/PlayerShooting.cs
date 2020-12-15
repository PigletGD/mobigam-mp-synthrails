using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public TouchPanel touchPanel = null;

    public PlayerInfo playerInfo = null;

    private Transform ownerTransform = null;
    public Transform planeNuzzle = null;
    public ObjectPool bulletPool = null;

    private GameObject bullet = null;

    private bool currentlyDragging = false;

    [SerializeField] private GameHUD gameHUD = null;

    private bool spentAmmo = false;

    private void Start()
    {
        ownerTransform = transform;

        touchPanel.OnTap += OnTap;
        touchPanel.OnDragging += OnDragging;
        touchPanel.OnDragRelease += OnDragRelease;
    }

    private void OnDisable()
    {
        touchPanel.OnTap -= OnTap;
        touchPanel.OnDragging -= OnDragging;
        touchPanel.OnDragRelease -= OnDragRelease;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Fire();
    }

    private void OnTap(object sender, TapEventArgs e)
    {
        InitializeBullet();
        if (spentAmmo)
        {
            AdjustPosition();
            Fire();
        }
    }

    private void OnDragging(object sender, DragEventArgs e)
    {
        if (!currentlyDragging)
            InitializeBullet();
        if (spentAmmo) AdjustPosition();
    }

    private void OnDragRelease(object sender, DragEventArgs e)
    {
        if (spentAmmo) Fire();
    }

    private void InitializeBullet()
    {
        spentAmmo = false;

        switch (playerInfo.playerType)
        {
            case EntityType.RED:
                if (playerInfo.ammoRed > 0)
                {
                    playerInfo.ammoRed--;
                    spentAmmo = true;

                    if (playerInfo.ammoRed <= 0)
                    {
                        gameHUD.AmmoReload(0);
                        StartCoroutine(Reload(0));
                    }
                }
                break;
            case EntityType.GREEN:
                if (playerInfo.ammoGreen > 0)
                {
                    playerInfo.ammoGreen--;
                    spentAmmo = true;

                    if (playerInfo.ammoGreen <= 0)
                    {
                        gameHUD.AmmoReload(1);
                        StartCoroutine(Reload(1));
                    }
                        
                }
                break;
            case EntityType.BLUE:
                if (playerInfo.ammoBlue > 0)
                {
                    playerInfo.ammoBlue--;
                    spentAmmo = true;

                    if (playerInfo.ammoBlue <= 0)
                    {
                        gameHUD.AmmoReload(2);
                        StartCoroutine(Reload(2));
                    }
                }
                break;
        }

        if (spentAmmo)
        {
            gameHUD.UpdateAmmo();

            bullet = bulletPool.RetrieveObject();

            BulletBehaviour BO = bullet.GetComponent<BulletBehaviour>();
            BO.InitializeType(playerInfo.playerType);
        }

        currentlyDragging = true;
    }

    IEnumerator Reload(int value)
    {
        yield return new WaitForSeconds(5);

        switch (value)
        {
            case 0: playerInfo.ammoRed = SaveManager.Instance.state.ammoCapacity; break;
            case 1: playerInfo.ammoGreen = SaveManager.Instance.state.ammoCapacity; break;
            case 2: playerInfo.ammoBlue = SaveManager.Instance.state.ammoCapacity; break;
        }
    }

    private void AdjustPosition()
    {
        bullet.transform.position = planeNuzzle.position;
        bullet.transform.rotation = ownerTransform.rotation;
    }

    private void Fire()
    {
        AudioManager.Instance.Play("Shot");

        BulletBehaviour BO = bullet.GetComponent<BulletBehaviour>();
        BO.Fire();

        bullet = null;

        currentlyDragging = false;
    }
}