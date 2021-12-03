using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launcherMover : MonoBehaviour
{
    public GameObject ballLauncher;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Ball")
            Destroy(other.gameObject);
    }
}