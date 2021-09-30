using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu()]
public class Item : ScriptableObject
{
    // Base attributes
    public int ID;
    public new string name;
    public Image icon;
    public string description;

    // Stats
    public int ballsToLaunchChange;
    public int aimLengthChange;

    // Effects
    public List<OnHitEffect> OnHitEffects = new List<OnHitEffect>();

    public void Activate(GameObject block)
    {
        Debug.Log("Default activate");

        foreach (OnHitEffect effect in OnHitEffects)
        {
            effect.Activate(block);
        }
    }

    public void Initialize()
    {
        // Have 2 more of these for the other effects - OnShot, BallEffect
        foreach (OnHitEffect effect in OnHitEffects)
        {
            GameManager.manager.OnBallHit += effect.Activate;
        }

        // Apply stat changes
        GlobalData.Instance.ballsToLaunch += ballsToLaunchChange;
        GlobalData.Instance.aimLength += aimLengthChange;
    }
}
