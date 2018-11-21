﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

using UnityEngine;
using RimWorld;
using Verse;
using Verse.AI;

namespace Rimhammer40k.AIRobot
{
    public class X2_JobGiver_Return2BaseRoom : ThinkNode
    {
        // When the robot is idle, check if it is inside the room of the recharge station. If not, return there.
        public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
        {
            X2_AIRobot robot = pawn as X2_AIRobot;
            if (robot.DestroyedOrNull()) return ThinkResult.NoJob;
            if (!robot.Spawned) return ThinkResult.NoJob;

            X2_Building_AIRobotRechargeStation rechargeStation = robot.rechargeStation;
            if (rechargeStation.DestroyedOrNull()) return ThinkResult.NoJob;
            if (!rechargeStation.Spawned) return ThinkResult.NoJob;

            Room roomRecharge = rechargeStation.Position.GetRoom(rechargeStation.Map);
            Room roomRobot = robot.Position.GetRoom(robot.Map);

            if (roomRecharge == roomRobot)
                return ThinkResult.NoJob;

            Map mapRecharge = rechargeStation.Map;
            IntVec3 cell = roomRecharge.Cells.Where(c => 
                                c.Standable(mapRecharge) && !c.IsForbidden(pawn) && 
                                pawn.CanReach(c, PathEndMode.OnCell, Danger.Some, false, TraverseMode.ByPawn))
                            .FirstOrDefault();

            if (cell == null || cell == IntVec3.Invalid)
                return ThinkResult.NoJob;
            
            Job jobGoto = new Job(JobDefOf.Goto, cell);
            jobGoto.locomotionUrgency = LocomotionUrgency.Amble;

            return new ThinkResult(jobGoto, this, JobTag.Misc, false);

        }
        
    }
}
