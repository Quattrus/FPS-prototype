using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    [Header("Ammo")]
    private int availableClips = 5;
   
    private int currentAmmo;
    
    [SerializeField] int maxAmmo; //base this on the gun type
    [SerializeField] bool gunEquipped;
    public int MaxAmmo { get { return maxAmmo; } }
    public int AvailableClips { get { return availableClips; } set { availableClips = value; } }
    public int CurrentAmmo { get { return currentAmmo; } set { currentAmmo = value; } }
    public bool GunEquipped { get { return gunEquipped; } set { gunEquipped = value; } }

    private void Awake()
    {
        Instance = this;
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
