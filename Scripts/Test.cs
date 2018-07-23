using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour {

    [Header ("Target Stats")]
    public Stats targetStats;
    public Text statsText;
    [Header("Add Buffs")]
    public List<Buff> buffList = new List<Buff>();
    [Header("Add Debffs")]
    public List<Buff> deBuffList = new List<Buff>();

    private void Start()
    {

    }

    public void SetupStats()
    {
        targetStats.stats.Add("Health", 400);
        targetStats.stats.Add("Speed", 1);
        targetStats.stats.Add("Strength", 1);
        targetStats.stats.Add("DP", 150);
        targetStats.statMultipliers.Add("Health", 1);
        targetStats.statMultipliers.Add("Speed", 1);
        targetStats.statMultipliers.Add("Strength", 1);
        targetStats.statMultipliers.Add("DP", 1);
    }

    private void Update()
    {
        string toPrint = "";
        toPrint += "Current stats:\n";
        toPrint += "-------------\n";
        foreach (KeyValuePair<string, float> values in targetStats.stats)
        {
            toPrint += values.Key + " : " + values.Value + "\n";
        }
        toPrint += "\n";
        toPrint += "-------------\n";
        toPrint += "\n";
        toPrint += "Current multipliers:\n";
        toPrint += "--------------\n";
        foreach (KeyValuePair<string, float> values in targetStats.statMultipliers)
        {
            toPrint += values.Key + " : " + values.Value + "\n";
        }
        statsText.text = toPrint;
    }

    public void AddRandomBuff()
    {
        Buff newBuff = buffList[Random.Range(0, buffList.Count)].Copy();

        targetStats.buffsPending.Add(newBuff);
        Buff.OnBuffAppliedTrigger(targetStats);
    }
    public void AddRandomDeBuff()
    {
        Buff newBuff = deBuffList[Random.Range(0, deBuffList.Count)].Copy();

        targetStats.buffsPending.Add(newBuff);
        Buff.OnBuffAppliedTrigger(targetStats);
    }

}
