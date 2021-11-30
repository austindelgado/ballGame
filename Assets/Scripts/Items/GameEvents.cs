using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// All possible game events should go in here, follow BlockHit() as a template
public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake()
    {
        if (current == null)
        {
            DontDestroyOnLoad(gameObject);
            current = this;
        }
        else if (current != this)
        {
            Destroy(gameObject);
        }
    }

    public event Action onEnemyTurnEnd;
    public void EnemyTurnEnd()
    {
        Debug.Log("OnEnemyTurnEnd activated!");
        if (onEnemyTurnEnd != null)
            onEnemyTurnEnd();
    }

    public event Action<blockObject> onBlockHit;
    public void BlockHit(blockObject block)
    {
        Debug.Log("OnBlockHit activated!");
        if (onBlockHit != null)
            onBlockHit(block);
    }

    // Block break event, primarily used to call Bounce() on all balls
    // Also calls, check gravity on all blocks
    // Potentially add parameter to know which block
    public event Action OnBlockBreak;
    public void BlockBreak(blockObject block)
    {
        Debug.Log("GameEvent: " + block.name + " broke!");
        if (OnBlockBreak != null)
            OnBlockBreak();
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
                collision.GetComponent<blockObject>().BlockHit();
            }
        }
    }
}