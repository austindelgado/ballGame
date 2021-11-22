using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class launcherMover : MonoBehaviour
{
    public GameObject ballLauncher;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (ballLauncher.GetComponent<ballLauncher>().shotFired && !ballLauncher.GetComponent<ballLauncher>().posUpdated)
        {
            ballLauncher.GetComponent<ballLauncher>().nextXPos = other.transform.position.x;
            ballLauncher.GetComponent<ballLauncher>().posUpdated = true;
        }

        Destroy(other.gameObject);
    }
}