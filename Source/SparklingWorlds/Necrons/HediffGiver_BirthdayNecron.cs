﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace Rimhammer40k.Necrons
{
    class HediffGiver_BirthdayNecron : HediffGiver
    {
        public void TryApplyAndSimulateSeverityChange(Pawn pawn, float gotAtAge, bool tryNotToKillPawn)
        {
            HediffGiver_BirthdayNecron.addedHediffs.Clear();
            if(!base.TryApply(pawn, HediffGiver_BirthdayNecron.addedHediffs))
            {
                return;
            }
            if(this.averageSeverityPerDayBeforeGeneration != 0f)
            {
                float num = (pawn.ageTracker.AgeBiologicalYearsFloat - gotAtAge) * 60f;
                if(num < 0f)
                {
                    Log.Error(string.Concat(new object[]
                    {
                        "daysPassed < 0, pawn=", pawn, ", gotAtAge=", gotAtAge
                    }));
                    return;
                }
                for(int i = 0; i < HediffGiver_BirthdayNecron.addedHediffs.Count; i++)
                {
                    this.SimulateSeverityChange(pawn, HediffGiver_BirthdayNecron.addedHediffs[i], num, tryNotToKillPawn);
                }
            }
            HediffGiver_BirthdayNecron.addedHediffs.Clear();
        }

        private void SimulateSeverityChange(Pawn pawn, Hediff hediff, float daysPassed, bool tryNotToKillPawn)
        {
            float num = this.averageSeverityPerDayBeforeGeneration * daysPassed;
            num *= Rand.Range(0.5f, 1.4f);
            num += hediff.def.initialSeverity;
            if (tryNotToKillPawn)
            {
                this.AvoidLifeThreateningStages(ref num, hediff.def.stages);
            }
            hediff.Severity = num;
            pawn.health.Notify_HediffChanged(hediff);
        }

        private void AvoidLifeThreateningStages(ref float severity, List<HediffStage> stages)
        {
            if (stages.NullOrEmpty<HediffStage>())
            {
                return;
            }
            int num = -1;
            for (int i = 0; i < stages.Count; i++)
            {
                if (stages[i].lifeThreatening)
                {
                    num = i;
                    break;
                }
            }
            if (num >= 0)
            {
                if (num == 0)
                {
                    severity = Mathf.Min(severity, stages[num].minSeverity);
                }
                else
                {
                    severity = Mathf.Min(severity, (stages[num].minSeverity + stages[num - 1].minSeverity) / 2f);
                }
            }
        }

        public float DebugChanceToHaveAtAge(Pawn pawn, int age)
        {
            float num = 1f;
            for (int i = 1; i <= age; i++)
            {
                float x = (float)i / pawn.RaceProps.lifeExpectancy;
                num *= 1f - this.ageFractionChanceCurve.Evaluate(x);
            }
            return 1f - num;
        }
        
        public override bool OnHediffAdded(Pawn pawn, Hediff hediff)
        {
            //Remove any disease from affecting.
            if (hediff.def.makesSickThought)
            {
                pawn.health.RemoveHediff(hediff);
                return false;
            }

            return true;
        }

        public SimpleCurve ageFractionChanceCurve;

        public float averageSeverityPerDayBeforeGeneration;

        private static List<Hediff> addedHediffs = new List<Hediff>();
    }
}