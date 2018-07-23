using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class BuffsSystem : ComponentSystem {

    private struct Filter
    {
        public Stats stats;
    }

    protected override void OnStartRunning()
    {
        base.OnStartRunning();
        Buff.OnBuffApplied += ApplyBuff;
    }

    protected override void OnStopRunning()
    {
        base.OnStopRunning();
        Buff.OnBuffApplied -= ApplyBuff;
    }

    private float globalTickCount = 0.1f;
    private float tickTimer = 0.1f;

    protected override void OnUpdate()
    {
        if (tickTimer <= 0) // Every tick apply update all buffs
        {
            UpdateBuffs(globalTickCount);
            tickTimer = globalTickCount;
        }
        else
        {
            tickTimer -= Time.deltaTime; // In between ticks update the time left for next tick
        }
    }

    protected void UpdateBuffs(float timeSinceLastTick) // Update buff status for all buffs active in the scene
    {
        foreach (var entity in GetEntities<Filter>())
        {
            if (entity.stats.buffsActive.Count > 0) // Only check and update buffs/UI if there is buffs active
            {
                foreach (Buff buff in entity.stats.buffsActive.ToArray()) // For each buff
                {
                    buff.buffDuration -= timeSinceLastTick; // Remove the time passed since the last update
                    float correctedBuffDuration = buff.buffDuration * 10;
                    int tempI = (int)correctedBuffDuration;
                    correctedBuffDuration = (float)tempI / 10;
                    buff.buffDuration = correctedBuffDuration; // Set the buff duration with 2 decimals

                    if (buff.actOnTick) // If the buff is marked to act every tick, apply the effects every tick of the update
                    {
                        if (buff.ticksLeft <= 0) // If it has to update in this tick doso
                        {
                            ApplyBuffEffects(buff, entity.stats);
                            buff.ticksLeft = buff.actEvery_X_Tick - 1;
                        }
                        else
                        {
                            buff.ticksLeft -= 1;
                        }
                    }

                    if (buff.buffDuration <= 0) // If the buff duration is exausted, end the buff effect
                    {
                        BuffEnded(buff, entity.stats);
                    }
                }
                UIBuffSystem.BuffUIUpdateRequest(entity.stats);
                ActiveBuffsToText(entity.stats);
            }
        }
    }

    //For the event trigger
    protected void ApplyBuff(Stats stats)
    {
        if (stats.buffsPending.Count > 0) // Check if these stats had a pending buff to apply
        {
            foreach (Buff buff in stats.buffsPending.ToArray()) // Apply all pending buffs
            {
                if (AddBuffToActiveBuffs(buff, stats))
                {
                    if (buff.actOnTick) // If the buff has to apply every X ticks, mark the amount of ticks left for the next apply
                    {
                        buff.ticksLeft = buff.actEvery_X_Tick - 1;
                        buff.cumulatedTicks = 0;
                    }
                    else // Apply the effect if it doesnt apply every X ticks
                    {
                        ApplyBuffEffects(buff, stats);
                        buff.cumulatedTicks = 1;
                    }
                    stats.buffsPending.Remove(buff);
                }
                ActiveBuffsToText(stats);
            }
        }
        else // These stats didn't have a pending buff to apply
        {

        }
    }

    private void ApplyBuffEffects(Buff buff, Stats stats)
    {
        if (buff.multiplier) // Distinction between multiplier or not depending on how it affects the stat (Additive just adds the value to the stat vs multiplicative adds to the multiplier of the selected stat)
        {
                if (stats.statMultipliers.ContainsKey(buff.statName)) // If these stats contain the stat we are trying to alter
                {
                     stats.statMultipliers[buff.statName] += buff.modifierValue; // Apply the Change
                }
                else // These stats dont contain the desired stat so we cant alter it (Display inmune or smth)
                {

                }
        }
        else // Apply the same as the multiplier but additive
        {
            if (stats.stats.ContainsKey(buff.statName)) // If these stats contain the stat we are trying to alter
            {
                stats.stats[buff.statName] += buff.modifierValue; // Apply the Change
            }
            else // These stats dont contain the desired stat so we cant alter it (Display inmune or smth)
            {

            }
        }
        if (buff.actOnTick) // If the buff has to update every X ticks, update the counter of ticks done
            buff.cumulatedTicks += 1;
    }

    private bool AddBuffToActiveBuffs(Buff buff, Stats stats) // Adds the buff to buff active and returns if it was successfully added
    {
        stats.buffsActive.Add(buff);

        return stats.buffsActive.Contains(buff);
    }

    protected void BuffEnded(Buff buff, Stats stats) // Remove the buff effects and remove from the buffActive list
    {
        if (buff.multiplier)
        {
            if (stats.statMultipliers.ContainsKey(buff.statName)) // If these stats contain the stat we are trying to alter
            {
                if (!buff.permanentEffects)
                    stats.statMultipliers[buff.statName] -= buff.modifierValue * buff.cumulatedTicks; // Revert the modification this buff applied if it not marked as permanent

                stats.buffsActive.Remove(buff);
            }
        }
        else
        {
            if (stats.stats.ContainsKey(buff.statName)) // If these stats contain the stat we are trying to alter
            {
                if (!buff.permanentEffects)
                    stats.stats[buff.statName] -= buff.modifierValue * buff.cumulatedTicks; // Revert the modification this buff applied if it not marked as permanent

                stats.buffsActive.Remove(buff);
            }
        }
    }

    public void ActiveBuffsToText(Stats stats)
    {
        string toPrint = "";

        toPrint += "List of buffs :\n";
        toPrint += "---------------\n";

        foreach (Buff buff in stats.buffsActive)
        {
            toPrint += "Buff ID: " + buff.buffID + " -- " + buff.buffDuration + "s left.\n";
        }

        stats.textToTest.text = toPrint;
    }
}
