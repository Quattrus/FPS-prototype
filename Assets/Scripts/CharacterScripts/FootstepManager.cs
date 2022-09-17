using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepManager : MonoBehaviour
{

    public static FootstepManager Instance { get; private set; }
    private AudioSource source;
    #region AudioClip Lists
    [SerializeField] List<AudioClip> grassSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> waterSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> caveSteps = new List<AudioClip>();
    [SerializeField] List<AudioClip> cementSteps = new List<AudioClip>();
    private List<AudioClip> currentList;
    #endregion
    
    #region Surface types
    private enum Surface { grass, water, cave, cement };
    private Surface surface;
    #endregion

    #region Animator and animator curves
    [SerializeField] int leftStepPlayed = 0;
    [SerializeField] int rightStepPlayed = 0;
    private Animator _animator;
    #endregion

    private void Start()
    {
        Instance = this;
        source = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        SelectStepList();
    }
    private void Update()
    {
        
        PlayFootstep();
        
    }

    #region Animator footstep checking
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
    #endregion


    #region Selects the type of audioclip to play
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
    #endregion

    #region Choose surface based on controllerColliderHit
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

    #endregion

    #region Play Step AudioClip and Land AudioClip
    private void StepSound()
    {
        AudioClip clip = currentList[Random.Range(0, currentList.Count)];
        source.PlayOneShot(clip);

    }
    public void PlayLand()
    {
        StepSound();
    }
    #endregion
}
