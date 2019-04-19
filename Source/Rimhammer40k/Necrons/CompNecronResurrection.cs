using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace Rimhammer40k.Necrons
{
    public class CompNecronResurrection : ThingComp
    {
        public CompProperties_NecronResurrection Props
        {
            get
            {
                return (CompProperties_NecronResurrection)this.props;
            }
        }

        public bool IsResurrectable;

        public int resurrectTime = -1;

        public int ticksToResurrection = 60000;

        public void AttemptResurrection()
        {
            Corpse corpse = this.parent as Corpse;
            System.Random rnd = new System.Random();
            int flag = rnd.Next(1, 100);
            if (corpse.InnerPawn.IsColonist)
            {
                if (flag < 75)
                {
                    this.IsResurrectable = true;
                }
                else
                {
                    return;
                }
            }
            else
            {
                if (flag < 25)
                {
                    this.IsResurrectable = true;
                }
                else
                {
                    return;
                }
            }
        }

        public bool ShouldResurrect()
        {
            Corpse corpse = this.parent as Corpse;
            if (resurrectTime > 0 && Current.Game.tickManager.TicksGame >= this.resurrectTime)
            {
                return true;
            }
            return false;
        }

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            if (!respawningAfterLoad)
            {
                Corpse corpse = this.parent as Corpse;
                AttemptResurrection();
            }
        }

        public override void CompTickRare()
        {
            base.CompTickRare();
            Corpse corpse = this.parent as Corpse;
            if (this.IsResurrectable == true && resurrectTime < 0)
            {
                resurrectTime = Current.Game.tickManager.TicksGame + ticksToResurrection;
            }
            if (this.ShouldResurrect())
            {
                ResurrectionUtility.Resurrect(corpse.InnerPawn);
            }
        }
    }
}
