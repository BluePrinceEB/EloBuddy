namespace Ezreal.Components.Utility
{
    #region Imports

    using System;
    using System.Linq;

    using EloBuddy;
    using EloBuddy.SDK;
    using EloBuddy.SDK.Enumerations;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using SharpDX;
    using SharpDX.Direct3D9;

    using Components.Core;

    using static Core.Spells;

    #endregion

    /// <summary>
    ///     The Extensions
    /// </summary>
    internal static class Extensions
    {
        #region Public Properties

        /// <summary>
        ///     My Player
        /// </summary>
        public static AIHeroClient Player => ObjectManager.Player;

        /// <summary>
        ///     Has Sheen Buff
        /// </summary>
        /// <param name="unit">The Unit</param>
        /// <returns></returns>
        public static bool HasSheenBuff(this AIHeroClient unit)
            =>
                unit.HasBuff("sheen") || unit.HasBuff("LichBane")
                || unit.HasBuff("dianaarcready") || unit.HasBuff("ItemFrozenFist")
                || unit.HasBuff("sonapassiveattack");

        /// <summary>
        ///     Has Tear
        /// </summary>
        /// <param name="unit">The Unit</param>
        /// <returns></returns>
        public static bool HasTear(this AIHeroClient unit)
            =>
                unit.InventoryItems.Any(
                    item =>
                    item.Id.Equals(ItemId.Tear_of_the_Goddess) || item.Id.Equals(ItemId.Archangels_Staff)
                    || item.Id.Equals(ItemId.Manamune));

        /// <summary>
        ///     Has Sheen
        /// </summary>
        /// <param name="unit">The Unit</param>
        /// <returns></returns>
        public static bool HasSheen(this AIHeroClient unit)
            =>
                unit.InventoryItems.Any(
                    item =>
                    item.Id.Equals(ItemId.Sheen) || item.Id.Equals(ItemId.Trinity_Force)
                    || item.Id.Equals(ItemId.Iceborn_Gauntlet));

        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///     The Main Font
        /// </summary>
        public static Font MainFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Tahoma", 12, System.Drawing.FontStyle.Bold));

        /// <summary>
        ///     Draw Text
        /// </summary>
        public static void DrawAutoHarassText()
        {
            var mark = Config.AutoHarass.GetBool("Enabled") ? "✓" : "✘";
            var color = Config.AutoHarass.GetBool("Enabled") ? Color.Green : Color.Red;

            MainFont.DrawText(null, "» Auto Harass", (int)Player.HPBarPosition.X - 15, (int)Player.HPBarPosition.Y + 160, Color.White);
            MainFont.DrawText(null, "[", (int)Player.HPBarPosition.X + 105, (int)Player.HPBarPosition.Y + 160, Color.White);
            MainFont.DrawText(null, mark, (int)Player.HPBarPosition.X + 115, (int)Player.HPBarPosition.Y + 160, color);
            MainFont.DrawText(null, "]", (int)Player.HPBarPosition.X + 130, (int)Player.HPBarPosition.Y + 160, Color.White);
        }

        /// <summary>
        ///     Get HitChance
        /// </summary>
        /// <param name="Menu">The Menu</param>
        /// <param name="Spell">The Spell</param>
        /// <returns></returns>
        public static HitChance GetHitChance(Menu Menu, Spell.SpellBase Spell)
        {
            var value = Menu.Get<ComboBox>(Spell.Slot.ToString() + ".Hit").CurrentValue;
            switch (value)
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
                default:
                    return HitChance.Medium;
            }
        }

        /// <summary>
        ///     Custom Spell Damage Calculations
        /// </summary>
        /// <param name="Spell">The Spell</param>
        /// <param name="target">The Target</param>
        /// <returns></returns>
        public static float GetCustomSpellDamage(this Spell.SpellBase Spell , Obj_AI_Base target)
        {
            var damage = 0f;

            var level = Player.Spellbook.GetSpell(Spell.Slot).Level - 1;
            var totalAD = Player.TotalAttackDamage;
            var totalAP = Player.TotalMagicalDamage;
            var baseAD = Player.BaseAttackDamage;
            var bonusAD = Player.FlatPhysicalDamageMod;
            var damageType = Spell.Slot == SpellSlot.Q ? DamageType.Physical : DamageType.Magical;
            var sheenDmg = Player.HasSheen() ? baseAD : 0;

            switch (Spell.Slot)
            {
                case SpellSlot.Q:
                    damage += new float[] { 35, 55, 75, 95, 115 }[level] + 1.1f * totalAD + 0.4f * totalAP + sheenDmg;
                    break;
                case SpellSlot.W:
                    damage += new float[] { 70, 115, 160, 205, 250 }[level] + 0.8f * totalAP;
                    break;
                case SpellSlot.E:
                    damage += new float[] { 75, 125, 175, 225, 275 }[level] + 0.5f * bonusAD + 0.75f * totalAP;
                    break;
                case SpellSlot.R:
                    damage += new float[] { 105, 150, 195 }[level] + 0.3f * bonusAD + 0.27f * totalAP;
                    break;
            }

            return Player.CalculateDamageOnUnit(target, damageType, damage);
        }

        /// <summary>
        ///     Total Damage Calculations
        /// </summary>
        /// <param name="player"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static float GetTotalDamage(this AIHeroClient player, Obj_AI_Base target)
        {
            var damage = player.GetAutoAttackDamage(target);

            foreach (var Spell in SpellList)
            {
                if (!Spell.IsReady())
                {
                    continue;
                }

                damage += Spell.GetCustomSpellDamage(target);
            }

            return damage;
        }

        /// <summary>
        ///     Get Check Box Menu Value
        /// </summary>
        /// <param name="mainMenu">The Menu</param>
        /// <param name="name">The Name</param>
        /// <returns></returns>
        public static bool GetBool(this Menu mainMenu, string name)
        {
            return mainMenu.Get<CheckBox>(name) != null && mainMenu.Get<CheckBox>(name).CurrentValue;
        }

        /// <summary>
        ///     Get Key Bind Menu Value
        /// </summary>
        /// <param name="mainMenu">The Menu</param>
        /// <param name="name">The Name</param>
        /// <returns></returns>
        public static bool GetKey(this Menu mainMenu, string name)
        {
            return mainMenu.Get<KeyBind>(name) != null && mainMenu.Get<KeyBind>(name).CurrentValue;
        }

        /// <summary>
        ///     Get Slider Menu Value
        /// </summary>
        /// <param name="mainMenu">The Menu</param>
        /// <param name="name">The Name</param>
        /// <returns></returns>
        public static int GetSlider(this Menu mainMenu, string name)
        {
            return mainMenu.Get<Slider>(name) != null ? mainMenu.Get<Slider>(name).CurrentValue : Int32.MaxValue;
        }

        /// <summary>
        ///     Get Combo Box Menu Value
        /// </summary>
        /// <param name="mainMenu">The Menu</param>
        /// <param name="name">The Name</param>
        /// <returns></returns>
        public static int GetList(this Menu mainMenu, string name)
        {
            return mainMenu.Get<ComboBox>(name) != null ? mainMenu.Get<ComboBox>(name).CurrentValue : Int32.MaxValue;
        }

        /// <summary>
        ///     Enough Mana
        /// </summary>
        /// <param name="unit">The Unit</param>
        /// <param name="mana">The Mana</param>
        /// <returns></returns>
        public static bool EnoughMana(this AIHeroClient unit, int mana)
        {
            return unit.ManaPercent >= mana;
        }

        #endregion
    }
}
