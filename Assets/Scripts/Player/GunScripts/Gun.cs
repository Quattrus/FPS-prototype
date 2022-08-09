using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("Initialization Variables")]
    [SerializeField] private GameObject Player;
    private Transform cam;

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

    [Header("Ammo")]
    public int availableClips = 5;
    public int currentAmmo;
    private bool manualReload = false;
    public bool isReloading = false;
    [SerializeField] int maxAmmo;

    [Header("Automatic Rifle")]
    [SerializeField] bool automaticRifle = true;

    [Header("Shotgun")]
    [SerializeField] bool shotgun = false;
    [SerializeField] int bulletsPerShot = 6;

    [Header("Laser")]
    [SerializeField] private GameObject laser;
    [SerializeField] Transform muzzle;
    [SerializeField] float fadeDuration = 0.1f;


    private void Awake()
    {
        cam = Camera.main.transform;
        rapidFireWait = new WaitForSeconds(1 / fireRate);
        reloadWait = new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
    }
    private void Update()
    {
        Aiming();
    }

    public void Shoot()
    {
        //remove from current ammo here
        currentAmmo--; 


        if(shotgun)
        {
            ShotGun();
        }
        else if(automaticRifle)
        {
            AutomaticRifle();
        }
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
                if (availableClips >= 0)
                {
                    StartCoroutine(Reload());
                }
            }
        }
        else if(CanShoot() && availableClips >= 0)
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
        if(currentAmmo != maxAmmo && !isReloading)
        {
            manualReload = true;
            StartCoroutine(Reload());
        }
    }



    private bool CanShoot()
    {
        bool enoughAmmo = currentAmmo > 0;
        return enoughAmmo;
    }

    private IEnumerator Reload()
    {
        if (currentAmmo > 0 && !manualReload)
        {
            yield return null;
        }
        else if(currentAmmo == 0 && !isReloading || manualReload)
        {
            isReloading = true;
            yield return reloadWait;
            currentAmmo = maxAmmo;
            AmmoRemoved();
            manualReload = false;
            isReloading = false;
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
        if (Player.GetComponent<PlayerLook>().IsAiming == true)
        {
            inaccuracyDistance = 2f;
        }
        else
        {
            inaccuracyDistance = 5f;
        }
    }

    public void AmmoAdded()
    {
        availableClips += 1;
    }

    public void AmmoRemoved()
    {
        if (availableClips > 0)
        {
            availableClips -= 1;
        }
    }

    private void ShotGun()
    {
        for (int i = 0; i < bulletsPerShot; i++)
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

    private void AutomaticRifle()
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


