namespace Ezreal.Components.Core
{
    #region Imports

    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu.Values;
    using EloBuddy.SDK.Enumerations;

    using Ezreal.Components.Utility;
    using Extensions = Utility.Extensions;

    using static Spells;

    using Enumerations;
    using Utils;

    #endregion

    /// <summary>
    ///     The Modes
    /// </summary>
    internal class Modes
    {
        /// <summary>
        ///     The Combo Mode
        /// </summary>
        internal class Combo
        {
            #region Public Methods

            /// <summary>
            ///     Execute Out Of AA Range Combo
            /// </summary>
            public static void ExecuteOutOfRangeCombo()
            {
                try
                {
                    var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

                    if (!target.IsValidTarget() || target.Distance(Extensions.Player.ServerPosition) <= Extensions.Player.GetAutoAttackRange() || target == null)
                    {
                        return;
                    }

                    foreach (var Spell in SpellList)
                    {
                        if (!Spell.IsReady())
                        {
                            continue;
                        }

                        if (Spell.Slot.Equals(SpellSlot.Q) && Config.Combo.GetBool("Q.Use") && Extensions.Player.EnoughMana(Config.Combo.GetSlider("Q.Mana")))
                        {
                            Q.CastMinimumHitchance(target, Extensions.GetHitChance(Config.Combo, Q));
                        }
                        else if (Spell.Slot.Equals(SpellSlot.E) && Config.Combo.GetBool("E.Use") && Extensions.Player.EnoughMana(Config.Combo.GetSlider("E.Mana")))
                        {
                            E.Cast(Extensions.Player.ServerPosition.Extend(target, 475).To3D());
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: Combo -> Can't execute out of range combo - {0}", e);
                    throw;
                }
            }

            /// <summary>
            ///     Execute Normal In AA Range Combo
            /// </summary>
            public static void ExecuteInRangeNormalCombo()
            {
                try
                {
                    var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

                    if (!target.IsValidTarget() || target.Distance(Extensions.Player.ServerPosition) >= Extensions.Player.GetAutoAttackRange() || target == null)
                    {
                        return;
                    }

                    foreach (var Spell in SpellList)
                    {
                        if (!Spell.IsReady())
                        {
                            continue;
                        }

                        if (Spell.Slot.Equals(SpellSlot.Q) && Config.Combo.GetBool("Q.Use") && Extensions.Player.EnoughMana(Config.Combo.GetSlider("Q.Mana")))
                        {
                            if (target.Distance(Extensions.Player.ServerPosition) <= Q.Range)
                            {
                                Q.CastMinimumHitchance(target, Extensions.GetHitChance(Config.Combo, Q));
                            }
                        }
                        else if (Spell.Slot.Equals(SpellSlot.W) && Config.Combo.GetBool("W.Use") && Extensions.Player.EnoughMana(Config.Combo.GetSlider("W.Mana")))
                        {
                            if (target.Distance(Extensions.Player.ServerPosition) <= W.Range)
                            {
                                W.CastMinimumHitchance(target, Extensions.GetHitChance(Config.Combo, W));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: Combo -> Can't execute in range normal combo - {0}", e);
                    throw;
                }
            }

            /// <summary>
            ///     Execute R Logic
            /// </summary>
            public static void ExecuteRLogic()
            {
                try
                {
                    var target = TargetSelector.GetTarget(Config.Combo.GetSlider("R.Range"), DamageType.Magical);

                    var hitChancePercent = Extensions.GetHitChance(Config.Combo, R) == HitChance.High ?
                            75 : Extensions.GetHitChance(Config.Combo, R) == HitChance.Medium ? 50 : 25;

                    if (!target.IsValidTarget(Config.Combo.GetSlider("R.Range")) || target == null)
                    {
                        return;
                    }

                    if (R.IsReady() && Config.Combo.GetBool("R.Use") && Extensions.Player.EnoughMana(Config.Combo.GetSlider("R.Mana")))
                    {
                        switch (Config.Combo.GetList("R.Mode"))
                        {
                            case 0:
                                if (target.HealthPercent <= Config.Combo.GetSlider("R.HP"))
                                {
                                    R.CastMinimumHitchance(target, Extensions.GetHitChance(Config.Combo, R));
                                }
                                break;
                            case 1:
                                R.CastIfItWillHit(Config.Combo.GetSlider("R.Count"), hitChancePercent);
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: Combo -> Can't execute R logic - {0}", e);
                    throw;
                }
            }

            public static void ExecuteForceR()
            {
                try
                {
                    var target = TargetSelector.GetTarget(Config.Combo.GetSlider("R.Range"), DamageType.Magical);

                    if (!target.IsValidTarget(Config.Combo.GetSlider("R.Range")) || target == null)
                    {
                        return;
                    }

                    if (R.IsReady() && Config.Combo.GetKey("R.Force"))
                    {
                        R.CastMinimumHitchance(target, Extensions.GetHitChance(Config.Combo, R));
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: Combo -> Can't execute force R - {0}", e);
                    throw;
                }
            }

            /// <summary>
            ///     Execute Weaving In AA Range Combo
            /// </summary>
            /// <param name="target">The Target</param>
            /// <param name="args">The Args</param>
            public static void ExecuteInRangeWeavingCombo(AttackableUnit target, EventArgs args)
            {
                try
                {
                    if (!target.IsValidTarget() || !(target is AIHeroClient) || target == null)
                    {
                        return;
                    }

                    foreach (var Spell in SpellList)
                    {
                        if (!Spell.IsReady())
                        {
                            continue;
                        }

                        if (Spell.Slot.Equals(SpellSlot.Q) && Config.Combo.GetBool("Q.Use") && Extensions.Player.EnoughMana(Config.Combo.GetSlider("Q.Mana")))
                        {
                            if (target.Distance(Extensions.Player.ServerPosition) <= Q.Range)
                            {
                                Q.CastMinimumHitchance((AIHeroClient)target, Extensions.GetHitChance(Config.Combo, Q));
                            }
                        }
                        else if (Spell.Slot.Equals(SpellSlot.W) && Config.Combo.GetBool("W.Use") && Extensions.Player.EnoughMana(Config.Combo.GetSlider("W.Mana")))
                        {
                            if (target.Distance(Extensions.Player.ServerPosition) <= W.Range)
                            {
                                W.CastMinimumHitchance((AIHeroClient)target, Extensions.GetHitChance(Config.Combo, W));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: Combo -> Can't execute weaving combo - {0}", e);
                    throw;
                }
            }

            #endregion
        }

        /// <summary>
        ///     The Harass Mode
        /// </summary>
        internal class Harass
        {
            #region Public Methods

            /// <summary>
            ///     Execute Harass
            /// </summary>
            public static void Execute()
            {
                try
                {
                    if (!Config.Harass.GetBool("Enabled"))
                    {
                        return;
                    }

                    var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

                    if (!target.IsValidTarget() || !Config.Harass.GetBool(target.ChampionName) || target == null)
                    {
                        return;
                    }

                    foreach (var Spell in SpellList)
                    {
                        if (!Spell.IsReady())
                        {
                            continue;
                        }

                        if (Spell.Slot.Equals(SpellSlot.Q) && Config.Harass.GetBool("Q.Use") && Extensions.Player.EnoughMana(Config.Harass.GetSlider("Q.Mana")))
                        {
                            if (target.Distance(Extensions.Player.ServerPosition) <= Q.Range)
                            {
                                Q.CastMinimumHitchance(target, Extensions.GetHitChance(Config.Harass, Q));
                            }
                        }
                        else if (Spell.Slot.Equals(SpellSlot.W) && Config.Harass.GetBool("W.Use") && Extensions.Player.EnoughMana(Config.Harass.GetSlider("W.Mana")))
                        {
                            if (target.Distance(Extensions.Player.ServerPosition) <= W.Range)
                            {
                                W.CastMinimumHitchance(target, Extensions.GetHitChance(Config.Harass, W));
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: Harass -> Can't execute harass - {0}", e);
                    throw;
                }
            }

            #endregion
        }

        /// <summary>
        ///     The Auto Harass Mode
        /// </summary>
        internal class AutoHarass
        {
            #region Public Methods

            /// <summary>
            ///     Execute Auto Harass
            /// </summary>
            public static void Execute()
            {
                try
                {
                    if (!Config.AutoHarass.GetBool("Enabled"))
                    {
                        return;
                    }

                    if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Combo) ||
                        Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Harass) ||
                        Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.Flee))
                    {
                        return;
                    }

                    var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);

                    if (!target.IsValidTarget() || !Config.AutoHarass.GetBool(target.ChampionName) || Extensions.Player.IsRecalling() || target == null)
                    {
                        return;
                    }

                    if (Q.IsReady() && Config.AutoHarass.GetBool("Q.Use") && Extensions.Player.EnoughMana(Config.AutoHarass.GetSlider("Q.Mana")))
                    {
                        if (target.Distance(Extensions.Player.ServerPosition) <= Q.Range)
                        {
                            Q.CastMinimumHitchance(target, Extensions.GetHitChance(Config.AutoHarass, Q));
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: AutoHarass -> Can't execute auto harass - {0}", e);
                    throw;
                }
            }

            #endregion
        }

        /// <summary>
        ///     The Last Hit Mode
        /// </summary>
        internal class LastHit
        {
            #region Public Methods

            /// <summary>
            ///     Execute Normal Last Hit
            /// </summary>
            public static void ExecuteNormalLastHit()
            {
                try
                {
                    if (!Config.LastHit.GetBool("Enabled"))
                    {
                        return;
                    }

                    var minion = EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .OrderBy(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && !a.IsDead && !a.IsInvulnerable && a.IsValidTarget(Q.Range) && a.Health <= Q.GetSpellDamage(a));

                    if (minion == null)
                    {
                        return;
                    }

                    if (Q.IsReady() && Config.LastHit.GetBool("Q.Use") && Extensions.Player.EnoughMana(Config.LastHit.GetSlider("Q.Mana")))
                    {
                        Q.CastMinimumHitchance(minion, HitChance.High);
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: LastHit -> Can't execute normal last hit - {0}", e);
                    throw;
                }
            }

            /// <summary>
            ///     Execute Last Hit On Unkillable Minion
            /// </summary>
            /// <param name="target">The Target</param>
            /// <param name="args">The Args</param>
            public static void ExecuteUnkillableLastHit(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
            {
                try
                {
                    if (!Config.LastHit.GetBool("Enabled"))
                    {
                        return;
                    }

                    if (target == null)
                    {
                        return;
                    }

                    foreach (var Spell in SpellList)
                    {
                        if (!Spell.IsReady())
                        {
                            continue;
                        }

                        if (Spell.Slot.Equals(SpellSlot.Q) && Config.LastHit.GetBool("Q.Use") && Extensions.Player.EnoughMana(Config.LastHit.GetSlider("Q.Mana")))
                        {
                            if (target.Distance(Extensions.Player.ServerPosition) <= Q.Range && target.TotalShieldHealth() < Q.GetCustomSpellDamage(target))
                            {
                                Q.CastMinimumHitchance(target, HitChance.High);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: LastHit -> Can't execute unkillable last hit - {0}", e);
                    throw;
                }
            }

            #endregion
        }

        /// <summary>
        ///     The Lane Clear Mode
        /// </summary>
        internal class LaneClear
        {
            #region Public Methods

            /// <summary>
            ///     Execute Normal Lane Clear
            /// </summary>
            public static void Execute()
            {
                try
                {
                    if (!Config.LaneClear.GetBool("Enabled"))
                    {
                        return;
                    }

                    var minion = EntityManager.MinionsAndMonsters.GetLaneMinions()
                        .OrderBy(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && !a.IsDead && !a.IsInvulnerable && a.IsValidTarget(Q.Range));

                    if (minion == null)
                    {
                        return;
                    }

                    if (Q.IsReady() && Config.LaneClear.GetBool("Q.Use") && Extensions.Player.EnoughMana(Config.LaneClear.GetSlider("Q.Mana")))
                    {
                        Q.CastMinimumHitchance(minion, HitChance.High);
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: LaneClear -> Can't execute lane clear - {0}", e);
                    throw;
                }
            }

            #endregion
        }

        /// <summary>
        ///     The Jungle Clear Mode
        /// </summary>
        internal class JungleClear
        {
            #region Public Methods

            /// <summary>
            ///     Execute Normal Jungle Clear
            /// </summary>
            public static void Execute()
            {
                try
                {
                    if (!Config.JungleClear.GetBool("Enabled"))
                    {
                        return;
                    }

                    var minion = EntityManager.MinionsAndMonsters.GetJungleMonsters()
                        .OrderBy(a => a.Health)
                        .FirstOrDefault(a => a.IsEnemy && !a.IsDead && !a.IsInvulnerable && a.IsValidTarget(Q.Range));

                    if (minion == null)
                    {
                        return;
                    }

                    if (Q.IsReady() && Config.JungleClear.GetBool("Q.Use") && Extensions.Player.EnoughMana(Config.JungleClear.GetSlider("Q.Mana")))
                    {
                        Q.CastMinimumHitchance(minion, HitChance.High);
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: JungleClear -> Can't execute jungle clear - {0}", e);
                    throw;
                }
            }

            #endregion
        }

        /// <summary>
        ///     The Kill Steal Mode
        /// </summary>
        internal class KS
        {
            #region Public Methods

            /// <summary>
            ///     Execute Kill Steal
            /// </summary>
            public static void Execute()
            {
                try
                {
                    if (!Config.KS.GetBool("Enabled"))
                    {
                        return;
                    }

                    foreach (var target in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(Q.Range) && !x.IsDead))
                    {
                        if (!Config.KS.GetBool(target.ChampionName) || target == null)
                        {
                            return;
                        }

                        foreach (var Spell in SpellList)
                        {
                            if (!Spell.IsReady())
                            {
                                continue;
                            }

                            if (Spell.Slot.Equals(SpellSlot.Q) && Config.KS.GetBool("Q.Use") && Extensions.Player.EnoughMana(Config.KS.GetSlider("Q.Mana")))
                            {
                                if (Q.GetSpellDamage(target) > target.TotalShieldHealth())
                                {
                                    Q.CastMinimumHitchance(target, Extensions.GetHitChance(Config.KS, Q));
                                }
                            }
                            else if (Spell.Slot.Equals(SpellSlot.W) && Config.KS.GetBool("W.Use") && Extensions.Player.EnoughMana(Config.KS.GetSlider("W.Mana")))
                            {
                                if (target.Distance(Extensions.Player.ServerPosition) <= W.Range && W.GetSpellDamage(target) > target.TotalShieldHealth())
                                {
                                    W.CastMinimumHitchance(target, Extensions.GetHitChance(Config.KS, W));
                                }
                            }
                            else if (Spell.Slot.Equals(SpellSlot.R) && Config.KS.GetBool("R.Use") && Extensions.Player.EnoughMana(Config.KS.GetSlider("R.Mana")))
                            {
                                if (target.Distance(Extensions.Player.ServerPosition) <= R.Range && R.GetSpellDamage(target) > target.TotalShieldHealth())
                                {
                                    R.CastMinimumHitchance(target, Extensions.GetHitChance(Config.KS, R));
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: KS -> Can't execute ks - {0}", e);
                    throw;
                }
            }

            #endregion
        }

        /// <summary>
        ///     The Flee Mode
        /// </summary>
        internal class Flee
        {
            #region Public Methods

            /// <summary>
            ///     Execute Flee
            /// </summary>
            public static void Execute()
            {
                try
                {
                    if (!Config.Flee.GetBool("Enabled"))
                    {
                        return;
                    }

                    if (E.IsReady() && Config.Flee.GetBool("E.Use"))
                    {
                        E.Cast(Extensions.Player.ServerPosition.Extend(Game.CursorPos, E.Range).To3D());
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: Flee -> Can't execute flee - {0}", e);
                    throw;
                }
            }

            #endregion
        }

        /// <summary>
        ///     The Other Logics
        /// </summary>
        internal class Other
        {
            #region Public Methods

            /// <summary>
            ///     Execute Tear Stack Logic
            /// </summary>
            public static void ExecuteTearLogic()
            {
                try
                {
                    if (Extensions.Player.IsRecalling())
                    {
                        return;
                    }

                    if (Q.IsReady() &&
                        Config.Misc.GetBool("Tear.Use") &&
                        Extensions.Player.HasTear() &&
                        Extensions.Player.CountEnemyChampionsInRange(1500) == 0 &&
                        Extensions.Player.CountEnemyMinionsInRange(1500) == 0 &&
                        Extensions.Player.EnoughMana(Config.Misc.GetSlider("Tear.Mana")))
                    {
                        Q.Cast(Extensions.Player.ServerPosition.Extend(Game.CursorPos, 475).To3D());
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: Other -> Can't execute tear stack logic - {0}", e);
                    throw;
                }
            }

            /// <summary>
            ///     Execute Skin Hack
            /// </summary>
            public static void ExecuteSkinHack()
            {
                try
                {
                    if (Config.Misc.GetBool("SkinChanger.Enabled"))
                    {
                        Extensions.Player.SetSkinId(Config.Misc.GetList("SkinChanger.ID"));
                    }
                }
                catch (Exception e)
                {
                    Logging.AddEntry(LoggingEntryType.Error, "@Modes.cs: Other -> Can't execute skinhack- {0}", e);
                    throw;
                }
            }

            #endregion
        }
    }
}
