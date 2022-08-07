using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyThings : MonoBehaviour
{

    [SerializeField] float timeOut = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cleanup());
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Cleanup());
    }

    IEnumerator Cleanup()
    {
        yield return new WaitForSeconds(timeOut);
        Destroy(gameObject);
    }
}
