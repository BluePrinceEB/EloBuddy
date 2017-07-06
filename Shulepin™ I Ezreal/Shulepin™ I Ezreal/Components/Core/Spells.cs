namespace Ezreal.Components.Core
{
    #region Imports

    using System;
    using System.Collections.Generic;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;

    using Enumerations;
    using Utils;

    #endregion

    /// <summary>
    ///     The Spells
    /// </summary>
    internal class Spells
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initialize Ezreal Spells
        /// </summary>
        public Spells()
        {
            try
            {
                Q = new Spell.Skillshot(SpellSlot.Q, 1150, SkillShotType.Linear, 250, 2000, 60, DamageType.Physical)
                {
                    AllowedCollisionCount = 0
                };
                W = new Spell.Skillshot(SpellSlot.W, 1000, SkillShotType.Linear, 250, 1600, 80, DamageType.Magical)
                {
                    AllowedCollisionCount = Int32.MaxValue
                };
                E = new Spell.Skillshot(SpellSlot.E, 475, SkillShotType.Linear);
                R = new Spell.Skillshot(SpellSlot.R, 1500, SkillShotType.Linear, 1100, 2000, 160, DamageType.Magical)
                {
                    AllowedCollisionCount = Int32.MaxValue
                };

                SpellList.AddRange(new[] { Q, W, E, R });

                Logging.AddEntry(LoggingEntryType.Debug, "@Spells.cs: Spell class initialized");
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@Spells.cs: Can't initialize spell class - {0}", e);
                throw;
            }
        }

        #endregion

        #region Public Static Fields

        /// <summary>
        ///     The Q Spell
        /// </summary>
        public static Spell.Skillshot Q { get; set; }

        /// <summary>
        ///     The Q Spell
        /// </summary>
        public static Spell.Skillshot W { get; set; }

        /// <summary>
        ///     The Q Spell
        /// </summary>
        public static Spell.Skillshot E { get; set; }

        /// <summary>
        ///     The Q Spell
        /// </summary>
        public static Spell.Skillshot R { get; set; }

        /// <summary>
        ///     The Spell List
        /// </summary>
        public static readonly List<Spell.SpellBase> SpellList = new List<Spell.SpellBase>();

        #endregion
    }
}
