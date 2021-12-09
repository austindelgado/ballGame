using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int roomDepth;
    public int side;
    private bool doorOpen;

    public Collider2D lDoor;
    public Collider2D rDoor;

    // This was made incredibly confusing for no reason
    // rename to Room

    // Update is called once per frame
    void Update()
    {
        if (roomDepth == GridManager.manager.playerDepth && !doorOpen)
        {
            ToggleDoor(side);
            GridManager.manager.rooms.Remove(this);
        }
        else if (roomDepth != GridManager.manager.playerDepth && doorOpen)
        {
            ToggleDoor(side);
        }
        else if (GridManager.manager.playerDepth - roomDepth > 5)
            Destroy(gameObject);    
    }

    void ToggleDoor(int side)
    {
        doorOpen = !doorOpen;

        Debug.Log("Toggling door");
        if (side == 0)
            lDoor.enabled = !doorOpen;
        else
            rDoor.enabled = !doorOpen;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Player entered room!");
            col.gameObject.GetComponent<PlayerMovement>().ToggleShoot();
            col.gameObject.GetComponent<PlayerMovement>().ToggleMelee();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Player left room!");
            col.gameObject.GetComponent<PlayerMovement>().ToggleShoot();
            col.gameObject.GetComponent<PlayerMovement>().ToggleMelee();
        }
    }
}
