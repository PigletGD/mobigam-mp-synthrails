using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public TouchPanel touchPanel;

    public PlayerInfo playerInfo;

    private Transform ownerTransform;
    public Transform planeNuzzle;
    public ObjectPool bulletPool;

    private GameObject bullet;

    private bool currentlyDragging = false;

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
        AdjustPosition();
        Fire();
    }

    private void OnDragging(object sender, DragEventArgs e)
    {
        if (!currentlyDragging)
            InitializeBullet();
        AdjustPosition();
    }

    private void OnDragRelease(object sender, DragEventArgs e)
    {
        Fire();
    }

    private void InitializeBullet()
    {
        bullet = bulletPool.RetrieveObject();

        BulletBehaviour BO = bullet.GetComponent<BulletBehaviour>();
        BO.InitializeType(playerInfo.playerType);

        currentlyDragging = true;
    }

    private void AdjustPosition()
    {
        bullet.transform.position = planeNuzzle.position;
        bullet.transform.rotation = ownerTransform.rotation;
    }

    private void Fire()
    {
        BulletBehaviour BO = bullet.GetComponent<BulletBehaviour>();
        BO.Fire();

        bullet = null;

        currentlyDragging = false;
    }
}