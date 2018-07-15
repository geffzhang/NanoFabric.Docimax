﻿using System.Collections.Generic;
using System.Diagnostics;

namespace NanoFabric.Docimax.Grains.Contracts.Heroes
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Hero
    {
        protected string DebuggerDisplay => $"Key: '{Key}', Name: '{Name}', Role: {Role}, Health: {Health}";
        public string Key { get; set; }
        public string Name { get; set; }
        public int Health { get; set; }
        public HeroRoleType Role { get; set; }
        public HashSet<string> Abilities { get; set; }

        public override string ToString() => DebuggerDisplay;
    }

    public enum HeroRoleType
    {
        Assassin = 1,
        Fighter = 2,
        Mage = 3,
        Support = 4,
        Tank = 5,
        Marksman = 6
    }

    public class HeroState
    {
        public Hero Hero { get; set; }
    }
}