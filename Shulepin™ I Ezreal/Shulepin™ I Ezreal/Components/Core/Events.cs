namespace Ezreal.Components.Core
{
    #region Imports

    using System;
    using System.Linq;

    using SharpDX;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Events;
    using EloBuddy.SDK.Rendering;

    using Ezreal.Components.Utility;
    using Extensions = Utility.Extensions;

    using static Spells;

    using Enumerations;
    using Utils;

    #endregion

    /// <summary>
    ///     The Events
    /// </summary>
    internal class Events
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initialize Ezreal Events
        /// </summary>
        public Events() 
        {
            Game.OnUpdate += OnUpdate;
            Drawing.OnDraw += OnDraw;
            Drawing.OnEndScene += OnEndScene;
            Orbwalker.OnPostAttack += OnPostAttack;
            Gapcloser.OnGapcloser += OnGapcloser;
            Obj_AI_Base.OnBuffGain += OnBuffGain;
            Orbwalker.OnUnkillableMinion += OnUnkillableMinion;
        }

        #endregion

        #region Private Methods And Operators

        /// <summary>
        ///     The Damage Indicator
        /// </summary>
        private static readonly DamageIndicator Indicator = new DamageIndicator();

        /// <summary>
        ///     On Update
        /// </summary>
        /// <param name="args">The Args</param>
        private void OnUpdate(EventArgs args)
        {
            try
            {
                Modes.KS.Execute();
                Modes.Combo.ExecuteForceR();
                Modes.Other.ExecuteSkinHack();
                Modes.AutoHarass.Execute();

                switch (Orbwalker.ActiveModesFlags)
                {
                    case Orbwalker.ActiveModes.None:
                        Modes.Other.ExecuteTearLogic();
                        break;
                    case Orbwalker.ActiveModes.Combo:
                        if (Config.Combo.GetList("Style") == 0)
                        {
                            Modes.Combo.ExecuteInRangeNormalCombo();
                        }
                        Modes.Combo.ExecuteOutOfRangeCombo();
                        Modes.Combo.ExecuteRLogic();
                        break;
                    case Orbwalker.ActiveModes.Harass:
                        Modes.Harass.Execute();
                        break;
                    case Orbwalker.ActiveModes.LastHit:
                        if (Config.LastHit.GetList("Mode") == 0)
                        {
                            Modes.LastHit.ExecuteNormalLastHit();
                        }
                        break;
                    case Orbwalker.ActiveModes.LaneClear:
                        Modes.LaneClear.Execute();
                        break;
                    case Orbwalker.ActiveModes.JungleClear:
                        Modes.JungleClear.Execute();
                        break;
                    case Orbwalker.ActiveModes.Flee:
                        Modes.Flee.Execute();
                        break;
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@Events.cs: Can't run OnUpdate - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     On Post Attack
        /// </summary>
        /// <param name="target">The Target</param>
        /// <param name="args">The Args</param>
        private void OnPostAttack(AttackableUnit target, EventArgs args)
        {
            try
            {
                switch (Orbwalker.ActiveModesFlags)
                {
                    case Orbwalker.ActiveModes.Combo:
                        if (Config.Combo.GetList("Style") == 1)
                        {
                            Modes.Combo.ExecuteInRangeWeavingCombo(target, args);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@Events.cs: Can't run OnPostAttack - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     On Unkillable Minion
        /// </summary>
        /// <param name="target">The Target</param>
        /// <param name="args">The Args</param>
        private void OnUnkillableMinion(Obj_AI_Base target, Orbwalker.UnkillableMinionArgs args)
        {
            try
            {
                if ((target is AIHeroClient) || !target.IsValidTarget() || target == null)
                {
                    return;
                }

                if (Orbwalker.ActiveModesFlags.Equals(Orbwalker.ActiveModes.LastHit) && Config.LastHit.GetList("Mode") == 1)
                {
                    Modes.LastHit.ExecuteUnkillableLastHit(target, args);
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@Events.cs: Can't run OnUnkillableMinion - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     On Draw
        /// </summary>
        /// <param name="args">The Args</param>
        private void OnDraw(EventArgs args)
        {
            try
            {
                if (Extensions.Player.IsDead)
                {
                    return;
                }

                foreach (var Spell in SpellList)
                {
                    if (!Spell.IsReady())
                    {
                        continue;
                    }

                    if (Config.Draw.GetBool(Spell.Slot.ToString() + ".Draw"))
                    {
                        Circle.Draw(Color.Aqua, Spell.Range, Extensions.Player);
                    }
                }

                if (Config.Draw.GetBool("AutoHarass.Status"))
                {
                    Extensions.DrawAutoHarassText();
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@Events.cs: Can't run OnDraw - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     On End Scene
        /// </summary>
        /// <param name="args">The Args</param>
        private void OnEndScene(EventArgs args)
        {
            try
            {
                if (Config.Draw.GetBool("DamageIndicator.Enabled"))
                {
                    foreach (var enemy in EntityManager.Heroes.Enemies.Where(x => x.IsValidTarget(20000)))
                    {
                        Indicator.Unit = enemy;
                        Indicator.DrawDamage(Extensions.Player.GetTotalDamage(enemy), new Color(255, 170, 30, 200));
                    }
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@Events.cs: Can't run OnEndScene - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     On Gapcloser
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="args">The Args</param>
        private void OnGapcloser(AIHeroClient sender, Gapcloser.GapcloserEventArgs args)
        {
            try
            {
                if (!args.Sender.IsValidTarget() ||
                args.Sender.Type != GameObjectType.AIHeroClient ||
                !Config.Misc.GetBool("E.Gap") ||
                !args.Sender.IsEnemy ||
                Extensions.Player.IsDead)
                {
                    return;
                }

                if (E.IsReady() && args.Sender.IsMelee && Extensions.Player.Distance(args.End) <= 350f)
                {
                    E.Cast(Extensions.Player.ServerPosition.Extend(args.Sender.ServerPosition, -E.Range).To3D());
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@Events.cs: Can't run OnGapcloser - {0}", e);
                throw;
            }
        }

        /// <summary>
        ///     On Buff Gain
        /// </summary>
        /// <param name="sender">The Sender</param>
        /// <param name="args">The Args</param>
        private void OnBuffGain(Obj_AI_Base sender, Obj_AI_BaseBuffGainEventArgs args)
        {
            try
            {
                if (!sender.IsMe)
                {
                    return;
                }

                if (E.IsReady())
                {
                    if (args.Buff.Name.Equals("ThreshQ"))
                    {
                        E.Cast(Extensions.Player.ServerPosition.Extend(Extensions.Player.ServerPosition, -E.Range).To3D());
                    }
                }
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@Events.cs: Can't run OnBuffGain - {0}", e);
                throw;
            }
        }

        #endregion
    }
}
