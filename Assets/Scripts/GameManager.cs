using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    // GameManager should be used for managing inGame events and contain the events, such as hit, that things can subscribe to
    // Difference between events and UnityEvents, look into this

    public static GameManager manager;

    void Awake()
    {
        if (manager == null)
        {
            DontDestroyOnLoad(gameObject);
            manager = this;
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }

    // Hit events need to receive what object they are hitting
    public event Action<GameObject> OnBallHit;
    public void BallHit(GameObject collision)
    {
        //Debug.Log(collision.name);
        if (OnBallHit != null)
        {
            OnBallHit(collision);
        }
        else
        {
            // Check if collision is with a block
            if (collision.tag == "Block" || collision.tag == "Enemy")
            {
                // Fill in with player default damage
                collision.GetComponent<blockObject>().BlockHit(1);
            }
        }
    }

    // Block break event, primarily used to call Bounce() on all balls
        // Also calls, check gravity on all blocks
    // Potentially add parameter to know which block
    public event Action OnBlockBreak;
    public void BlockBreak()
    {
        //Debug.Log("Block break event");
        if (OnBlockBreak != null)
            OnBlockBreak();
    }
}
