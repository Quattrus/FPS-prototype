using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("Initialization Variables")]
    [SerializeField] private GameObject Player;
    private Transform cam;
    private Inventory inventory;

    [Header("General Stats")]
    [SerializeField] float range = 50f;
    [SerializeField] float damage = 10f;
    [SerializeField] float fireRate = 5f;
    [SerializeField] float reloadTime;
    private WaitForSeconds reloadWait;
    [SerializeField] float inaccuracyDistance = 5f;

    [Header("Rapid Fire")]
    [SerializeField] bool rapidFire = false;
    private WaitForSeconds rapidFireWait;

    [Header("Automatic Rifle")]
    [SerializeField] bool automaticRifle;

    [Header("Shotgun")]
    [SerializeField] bool shotgun;
    [SerializeField] int bulletsPerShot = 6;

    [Header("Laser")]
    [SerializeField] private GameObject laser;
    [SerializeField] Transform muzzle;
    [SerializeField] float fadeDuration = 0.1f;

    [Header("Reloading")]
    private bool manualReload = false;
    public bool isReloading = false;

    private enum GunType { shotgun, automaticRifle}
    private GunType gunType;
    


    private void Awake()
    {
        cam = Camera.main.transform;
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        reloadWait = new WaitForSeconds(reloadTime);
        inventory = Player.GetComponent<Inventory>();
        
    }
    private void Update()
    {
       Aiming();
    }

    public IEnumerator RapidFire()
    {
        if(CanShoot() && !isReloading)
        {
            Shoot();
            if(rapidFire)
            {
                while (CanShoot())
                {
                    yield return rapidFireWait;
                    Shoot();
                }
                if (inventory.AvailableClips >= 0)
                {
                    StartCoroutine(Reload());
                }
            }
        }
        else if(CanShoot() && inventory.AvailableClips >= 0)
        {
            StartCoroutine(Reload());
        }
        else
        {
            yield return null;
        }

    }



    public void ReloadGun()
    {
        if(inventory.CurrentAmmo != inventory.MaxAmmo && !isReloading)
        {
            manualReload = true;
            StartCoroutine(Reload());
        }
    }



    private bool CanShoot()
    {
        bool enoughAmmo = inventory.CurrentAmmo > 0;
        return enoughAmmo;
    }

    private IEnumerator Reload()
    {
        if (inventory.CurrentAmmo > 0 && !manualReload)
        {
            yield return null;
        }
        else if(inventory.CurrentAmmo == 0 && !isReloading || manualReload)
        {
            isReloading = true;
            yield return reloadWait;
            inventory.CurrentAmmo = inventory.MaxAmmo;
            inventory.AmmoRemoved();
            manualReload = false;
            isReloading = false;
        }
    }

    public void Shoot()
    {
        //remove from current ammo here
        inventory.CurrentAmmo--;
        if (shotgun)
        {
            ShotGun();
        }
        else
        {
            AutomaticRifle();
        }

    }

    private Vector3 GetShootingDirection()
    { 
        Vector3 targetPos = cam.position + cam.forward * range;
        targetPos = new Vector3(
            targetPos.x + Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.y + Random.Range(-inaccuracyDistance, inaccuracyDistance),
            targetPos.z + Random.Range(-inaccuracyDistance, inaccuracyDistance)
            );

        Vector3 direction = targetPos - cam.position;
        return direction.normalized;

    }

    private void CreateLaser(Vector3 end)
    {
        LineRenderer lr = Instantiate(laser.GetComponent<LineRenderer>());
        lr.SetPositions(new Vector3[2] { muzzle.position, end });
        StartCoroutine(FadeLaser(lr));
    }

    private IEnumerator FadeLaser(LineRenderer lr)
    {
        float alpha = 1;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime / fadeDuration;
            lr.startColor = new Color(lr.startColor.r, lr.startColor.g, lr.startColor.b, alpha);
            lr.endColor = new Color(lr.endColor.r, lr.endColor.g, lr.endColor.b, alpha);
            yield return null;
        }
    }

    private void Aiming()
    {
        if (Player.GetComponent<PlayerStateMachine>().IsAiming == true)
        {
            inaccuracyDistance = 2f;
        }
        else
        {
            inaccuracyDistance = 5f;
        }
    }

    private void ShotGun()
    {
        bulletsPerShot = 6;
        ShootRayCast();
    }

    private void AutomaticRifle()
    {
        bulletsPerShot = 1;
        ShootRayCast();
    }

    private void ShootRayCast()
    {
        for (int bullets = 0; bullets < bulletsPerShot; bullets++)
        {
            RaycastHit hit;
            Vector3 shootingDir = GetShootingDirection();
            if (Physics.Raycast(cam.position, shootingDir, out hit, range))
            {
                if (hit.collider.GetComponent<Damageable>() != null)
                {
                    hit.collider.GetComponent<Damageable>().TakeDamage(damage, hit.point, hit.normal);
                }
                CreateLaser(hit.point);
            }
            else
            {
                CreateLaser(cam.position + shootingDir * range);
            }
        }
    }

    public void Flashlight()
    {
        //place flashlight code here
    }

    public void SwitchGuns(int weaponType)
    {
        if (weaponType == 1)
        {
            gunType = GunType.automaticRifle;
        }
        if(weaponType == 2)
        {
            gunType = GunType.shotgun;
        }
        GunTypeCheck();
    }

    private void GunTypeCheck()
    {
        switch (gunType)
        {
            case GunType.automaticRifle:
                automaticRifle = true;
                shotgun = false;
                break;
            case GunType.shotgun:
                shotgun = true;
                automaticRifle = false;
                break;
        }
    }
}


