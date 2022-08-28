using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> grassSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> waterSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> caveSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> cementSteps = new List<AudioClip>();
    private Animator animator;


    private enum Surface { grass, water, cave, cement};
    private Surface surface;

    private List<AudioClip> currentList;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }

    public void PlayStep(int stepType)
    {
        AudioClip clip = currentList[Random.Range(0, currentList.Count)];
        if(stepType > 1)
        {
            if(animator.GetFloat("MoveY") == 2)
            {
                source.PlayOneShot(clip);
            }
            
        }
        else if(stepType == 1)
        {
            if(animator.GetFloat("MoveY") > 0.6f)
            {
                source.PlayOneShot(clip);
            }
            else if(animator.GetFloat("MoveY") < -0.6f)
            {
                source.PlayOneShot(clip);
            }
            

        }
        else if(stepType < 1)
        {
            if(animator.GetFloat("MoveX") < -0.8f)
            {
                source.PlayOneShot(clip);
            }
            else if(animator.GetFloat("MoveX") > 0.8f)
            {
                source.PlayOneShot(clip);
            }
            
        }

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
