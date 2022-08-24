using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> grassSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> waterSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> caveSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> cementSteps = new List<AudioClip>();

    private enum Surface { grass, water, cave, cement};
    private Surface surface;

    private List<AudioClip> currentList;

    private AudioSource source;
    private PlayerStateMachine playerStateMachine;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
    }

    public void PlayStep()
    {
        AudioClip clip = currentList[Random.Range(0, currentList.Count)];
        source.PlayOneShot(clip);
    }

    private void SelectStepList()
    {
        switch (surface)
        {
            case Surface.grass:
                currentList = grassSteps;
                break;
            case Surface.water:
                currentList = waterSteps;
                break;
            case Surface.cave:
                currentList = caveSteps;
                break;
            case Surface.cement:
                currentList = cementSteps;
                break;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Grass")
        {
            surface = Surface.grass;
        }

        if (hit.transform.tag == "Water")
        {
            surface = Surface.water;
        }

        if (hit.transform.tag == "Cave")
        {
            surface = Surface.cave;
        }
        if(hit.transform.tag == "Cement")
        {
            surface = Surface.cement;
        }

        SelectStepList();

    }
}
