using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> grassSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> waterSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> caveSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> cementSteps = new List<AudioClip>();


    //private enum Surface { grass, water, cave, cement};
    //private Surface surface;

    private List<AudioClip> currentList;

    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void PlayFootstep()
    {
        AudioClip clip = currentList[Random.Range(0, currentList.Count)];
        source.PlayOneShot(clip);
    }

    public void PlaySteps(int stepType)
    {
        if(stepType == 0)
        {
            currentList = grassSteps;
            PlayFootstep();
        }
        else if(stepType == 1)
        {
            currentList = cementSteps;
            PlayFootstep();
        }
        else if(stepType == 2)
        {
            currentList = caveSteps;
            PlayFootstep();
        }
        else if(stepType == 3)
        {
            currentList = waterSteps;
            PlayFootstep();
        }
    }
}
