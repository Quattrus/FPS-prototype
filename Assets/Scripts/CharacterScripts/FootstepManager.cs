using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> grassSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> waterSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> caveSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> cementSteps = new List<AudioClip>();
    private Animator _animator;


    private enum Surface { grass, water, cave, cement };
    private Surface surface;

    private List<AudioClip> currentList;
    [SerializeField] int leftStepPlayed = 0;
    [SerializeField] int rightStepPlayed = 0;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }
    private void Update()
    {
        PlayFootstep();
    }

    private void PlayFootstep()
    {

        if (_animator.GetFloat("LeftFootCurve") >= 0.7f && leftStepPlayed == 0)
        {
            leftStepPlayed += 1;
            StepSound();

        }
        else if (_animator.GetFloat("LeftFootCurve") <= 0.5f)
        {
            leftStepPlayed = 0;
        }
        if (_animator.GetFloat("RightFootCurve") >= 0.7f && rightStepPlayed == 0)
        {
            rightStepPlayed += 1;
            StepSound();
        }
        else if (_animator.GetFloat("RightFootCurve") <= 0.5f)
        {
            rightStepPlayed = 0;
        }

    }
    private void StepSound()
    {
        AudioClip clip = currentList[Random.Range(0, currentList.Count)];
        source.PlayOneShot(clip);

    }
    public void PlayLand()
    {
        StepSound();
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
        if (hit.transform.tag == "Cement")
        {
            surface = Surface.cement;
        }
        SelectStepList();
    }
}
