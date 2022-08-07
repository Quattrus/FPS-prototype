using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerSphere : Interactable
{
    private MeshRenderer meshRenderer;
    private int colorIndex;

    [SerializeField] Color[] colors;
    
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = Color.red;
    }

    protected override void Interact()
    {
        colorIndex++;
        if(colorIndex > colors.Length  - 1)
        {
            colorIndex = 0;
        }
        meshRenderer.material.color = colors[colorIndex];
    }
}
