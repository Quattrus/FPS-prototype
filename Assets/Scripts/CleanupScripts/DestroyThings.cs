using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThings : MonoBehaviour
{

    [SerializeField] float timeOut = 0.5f;
    void Start()
    {
        StartCoroutine(Cleanup());
    }

    void Update()
    {
        StartCoroutine(Cleanup());
    }

    #region Cleanup routine to delete interacted gameObject
    IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(timeOut);
        Destroy(gameObject);
    }
    #endregion
}
