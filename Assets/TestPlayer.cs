using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    public GameObject testBlock;
    public Item itemEquipped;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
            Hit();
    }

    // Pretend this is the ball hit
    public void Hit()
    {
        if (testBlock)
            GameManager.manager.BallHit(testBlock);
    }
}
