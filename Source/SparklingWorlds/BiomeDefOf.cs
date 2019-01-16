﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using RimWorld;
using Verse;

namespace Rimhammer40k
{
    [DefOf]
    public static class BiomeDefOf
    {
        static BiomeDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(BiomeDefOf));
        }

        public static BiomeDef GaussDesertGreen;
    }
}