using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

public class UIBuffSystem : ComponentSystem
{

    public static void BuffUIUpdateRequest(Stats stats)
    {
        int buffsActive, deBuffsActive;
        Image[] buffPrefabs = stats.buffUIHolder.GetComponentsInChildren<Image>();
        Image[] deBuffPrefabs = stats.deBuffUIHolder.GetComponentsInChildren<Image>();

        List<Buff> buffs = new List<Buff>();
        List<Buff> deBuffs = new List<Buff>();


        foreach (Buff b in stats.buffsActive) //Get buffs and debuffs
        {
            if (b.modifierValue < 0) // Divide buffs and debuffs to show them in the respecting line
            {
                deBuffs.Add(b);
            }
            else
            {
                buffs.Add(b);
            }
        }
        buffsActive = buffs.Count;
        deBuffsActive = deBuffs.Count;

        if (buffsActive <= buffPrefabs.Length) // If we have less buffs actives than prefabs, update the ones we need and delete the ones we dont need
        {
            for (int i = 0; i < buffPrefabs.Length; i++) // For each buff prefab, update image and timer
            {
                if (i < buffsActive)
                {
                    buffPrefabs[i].sprite = buffs[i].buffIcon;
                    buffPrefabs[i].color = new Color(1, 1, 1, 1);
                    buffPrefabs[i].GetComponentInChildren<Text>().text = buffs[i].buffDuration.ToString();
                }
                else if (buffPrefabs[i].sprite != null) // If we have buff prefabs left over, Hide them
                {
                    buffPrefabs[i].sprite = null;
                    buffPrefabs[i].color = new Color(1, 1, 1, 0);
                    buffPrefabs[i].GetComponentInChildren<Text>().text = "";
                }
            }
        }
        else
        {
            for (int i = 0; i < buffsActive; i++) // For each buff prefab, update image and timer
            {
                if (i < buffPrefabs.Length)
                {
                    buffPrefabs[i].sprite = buffs[i].buffIcon;
                    buffPrefabs[i].color = new Color(1, 1, 1, 1);
                    buffPrefabs[i].GetComponentInChildren<Text>().text = buffs[i].buffDuration.ToString();
                }
                else // If we need ran out of buff prefabs to display the active buffs
                {
                    // Do nothing for now
                }
            }
        }

        if (deBuffsActive <= deBuffPrefabs.Length) // If we have less buffs actives than prefabs, update the ones we need and delete the ones we dont need
        {
            for (int i = 0; i < deBuffPrefabs.Length; i++) // For each buff prefab, update image and timer
            {
                if (i < deBuffsActive)
                {
                    deBuffPrefabs[i].sprite = deBuffs[i].buffIcon;
                    deBuffPrefabs[i].color = new Color(1, 1, 1, 1);
                    deBuffPrefabs[i].GetComponentInChildren<Text>().text = deBuffs[i].buffDuration.ToString();
                }
                else if (deBuffPrefabs[i].sprite != null) // If we have buff prefabs left over, Hide them
                {
                    deBuffPrefabs[i].sprite = null;
                    deBuffPrefabs[i].color = new Color(1, 1, 1, 0);
                    deBuffPrefabs[i].GetComponentInChildren<Text>().text = "";
                }
            }
        }
        else
        {
            for (int i = 0; i < buffsActive; i++) // For each buff prefab, update image and timer
            {
                if (i < buffPrefabs.Length)
                {
                    deBuffPrefabs[i].sprite = deBuffs[i].buffIcon;
                    deBuffPrefabs[i].color = new Color(1, 1, 1, 1);
                    deBuffPrefabs[i].GetComponentInChildren<Text>().text = deBuffs[i].buffDuration.ToString();
                }
                else // If we need ran out of buff prefabs to display the active buffs
                {
                    // Do nothing for now
                }
            }
        }
    }

    protected override void OnUpdate()
    {
        //
    }
}
