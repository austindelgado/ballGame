using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopUp : MonoBehaviour
{
    private TextMeshPro textMesh;
    private float disappearTimer = 1f;
    private Color textColor;

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }

    public static DamagePopUp Create(Vector3 position, int damageAmount, Transform prefab)
    {
        Transform damagePopupTransform = Instantiate(prefab, position, Quaternion.identity);

        DamagePopUp damagePopup = damagePopupTransform.GetComponent<DamagePopUp>();
        damagePopup.Setup(damageAmount);

        return damagePopup;
    }

    public void Setup (int damageAmount)
    {
        textMesh.SetText(damageAmount.ToString());
        textColor = textMesh.color;
    }

    private void Update() 
    {
        float moveSpeed = 1f;
        transform.position += new Vector3(0, moveSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disappearTimer = 1f;
            textColor.a -= disappearTimer * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
                Destroy(gameObject);
        }
    }
}
