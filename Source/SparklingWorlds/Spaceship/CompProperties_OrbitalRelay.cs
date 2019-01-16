﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k.Spaceship
{
    public class CompProperties_OrbitalRelay : CompProperties
    {
        // File path for OrbitalRelay dish texture.
        public string dishTexture;
        // Size of OrbitalRelay dish texture.
        public Vector3 size = new Vector3(1, 1, 1);
    }
}