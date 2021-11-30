using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemGrabber : MonoBehaviour
{
    public GameObject numGemsText;

    void Update()
    {
        if (numGemsText != null)
            numGemsText.GetComponent<TMP_Text>().text = GlobalData.Instance.gems.ToString();
    }
}
