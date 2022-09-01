using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Ammo")]
    private int availableClips = 5;
    public int AvailableClips { get { return availableClips; } set { availableClips = value; } }
    private int currentAmmo;
    public int CurrentAmmo { get { return currentAmmo; } set { currentAmmo = value; } }
    [SerializeField] int maxAmmo; //base this on the gun type
    public int MaxAmmo { get { return maxAmmo; } }

    private void Awake()
    {
        currentAmmo = maxAmmo;
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
}
