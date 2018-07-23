using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

[CreateAssetMenu (fileName = "New Buff", menuName = "Buffs/Basic Buff")]
public class Buff : ScriptableObject, IComponentData {

    public delegate void BuffApplied(Stats stats);
    public static event BuffApplied OnBuffApplied;

    public static void OnBuffAppliedTrigger(Stats stats)
    {
        OnBuffApplied(stats);
    }

    [Header ("Basic info")]
    public uint buffID;
    public string buffName;
    public Sprite buffIcon;
    public float buffDuration;
    [Header ("Type of buff")]
    public bool multiplier;
    public bool actOnTick;
    public int actEvery_X_Tick;
    [HideInInspector]
    public int ticksLeft;
    [HideInInspector]
    public int cumulatedTicks;
    public bool permanentEffects;
    [Header ("Owner")]
    public uint ownerID;
    [Header("Stats this buff will affect")]
    public string statName;
    public float modifierValue;

    public Buff Copy()
    {
        Buff newBuff = Buff.CreateInstance<Buff>();

        newBuff.buffID = this.buffID;
        newBuff.buffName = this.buffName;
        newBuff.buffIcon = this.buffIcon;
        newBuff.buffDuration = this.buffDuration;
        newBuff.multiplier = this.multiplier;
        newBuff.actOnTick = this.actOnTick;
        newBuff.actEvery_X_Tick = this.actEvery_X_Tick;
        newBuff.ticksLeft = this.ticksLeft;
        newBuff.cumulatedTicks = this.cumulatedTicks;
        newBuff.permanentEffects = this.permanentEffects;
        newBuff.ownerID = this.ownerID;
        newBuff.statName = this.statName;
        newBuff.modifierValue = this.modifierValue;

        return newBuff;
    }

}
