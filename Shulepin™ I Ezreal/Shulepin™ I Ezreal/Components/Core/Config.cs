namespace Ezreal.Components.Core
{
    #region Imports

    using System;

    using EloBuddy.SDK;
    using EloBuddy.SDK.Menu;
    using EloBuddy.SDK.Menu.Values;

    using Enumerations;
    using Utils;

    #endregion

    /// <summary>
    ///     The Config
    /// </summary>
    internal class Config
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initialize Ezreal Config
        /// </summary>
        public Config()
        {
            try
            {
                Main = MainMenu.AddMenu("Shulepin™ | Ezreal", "Ezreal");
                {
                    Main.AddLabel("███████╗ ███████╗ ██████╗   ███████╗   █████╗   ██╗");
                    Main.AddLabel("██╔════════╝ ╚═════███╔╝  ██╔════██╗ ██╔════════╝ ██╔════██╗██╗", 0);
                    Main.AddLabel("█████╗             ███╔╝    ██████╔╝  █████╗     ███████║ ██╗", 25);
                    Main.AddLabel("██╔════╝            ███╔╝      ██╔════██╗ ██╔═════╝     ██╔════██║ ██╗", 0);
                    Main.AddLabel("██████╗    ███████╗ ██║      ██║ ███████╗██║      ██║ ███████╗", 25);
                    Main.AddLabel("╚══════════╝    ╚════════════╝ ╚══╝       ╚══╝  ╚═══════════╝  ╚══╝       ╚══╝ ╚════════════╝", 0);

                    Main.AddGroupLabel("Welcome Shulepin™ Ezreal");
                    Main.AddLabel("You can configure the addon on the left by navigating through the menu entries.");
                    Main.AddLabel("Any suggestions or feedback write in the topic on the forum");
                    Main.AddLabel("Thanks for using Ezreal addon.");

                    Combo = Main.AddSubMenu("» Combo");
                    {
                        Combo.AddGroupLabel("● Combo Settings ●");
                        Combo.Add("Style", new ComboBox("Combo Style", 1, new[] { "Normal", "Weaving" }));
                        Combo.AddGroupLabel("● Spell Settings ●");
                        Combo.Add("Q.Use", new CheckBox("Use Q", true));
                        Combo.Add("W.Use", new CheckBox("Use W", true));
                        Combo.Add("E.Use", new CheckBox("Use E", false));
                        Combo.Add("R.Use", new CheckBox("Use R", true));
                        Combo.Add("R.Mode", new ComboBox("R Mode", 0, new[] { "Enemy HP", "Hit Count Enemies" }));
                        Combo.Add("R.HP", new Slider("Min. HP(%) For R ({0}%)", 35, 0, 100));
                        Combo.Add("R.Count", new Slider("Min. Enemies For R ({0})", 3, 1, 5));
                        Combo.Add("R.Range", new Slider("Max R Range ({0})", 1500, 500, 5000));
                        Combo.AddLabel("Key Settings:");
                        Combo.Add("R.Force", new KeyBind("Force R Key", false, KeyBind.BindTypes.HoldActive, 'G'));
                        Combo.AddGroupLabel("● Mana Settings ●");
                        Combo.Add("Q.Mana", new Slider("Min. Mana For Q [{0}%]", 0, 0, 100));
                        Combo.Add("W.Mana", new Slider("Min. Mana For W [{0}%]", 30, 0, 100));
                        Combo.Add("E.Mana", new Slider("Min. Mana For E [{0}%]", 30, 0, 100));
                        Combo.Add("R.Mana", new Slider("Min. Mana For R [{0}%]", 0, 0, 100));
                        Combo.AddGroupLabel("● Prediction Settings ●");
                        Combo.Add("Q.Hit", new ComboBox("HitChance For Q", 2, new[] { "Low", "Medium", "High" }));
                        Combo.Add("W.Hit", new ComboBox("HitChance For W", 2, new[] { "Low", "Medium", "High" }));
                        Combo.Add("R.Hit", new ComboBox("HitChance For R", 2, new[] { "Low", "Medium", "High" }));
                    }

                    Harass = Main.AddSubMenu("» Harass");
                    {
                        Harass.AddGroupLabel("● Harass Settings ●");
                        Harass.Add("Enabled", new CheckBox("Enabled", true));
                        Harass.AddLabel("Use On:");
                        foreach (var enemy in EntityManager.Heroes.Enemies)
                        {
                            Harass.Add(enemy.ChampionName, new CheckBox(enemy.ChampionName, true));
                        }
                        Harass.AddGroupLabel("● Spell Settings ●");
                        Harass.Add("Q.Use", new CheckBox("Use Q", true));
                        Harass.Add("W.Use", new CheckBox("Use W", false));
                        Harass.AddGroupLabel("● Mana Settings ●");
                        Harass.Add("Q.Mana", new Slider("Min. Mana For Q [{0}%]", 50, 0, 100));
                        Harass.Add("W.Mana", new Slider("Min. Mana For W [{0}%]", 75, 0, 100));
                        Harass.AddGroupLabel("● Prediction Settings ●");
                        Harass.Add("Q.Hit", new ComboBox("HitChance For Q", 2, new[] { "Low", "Medium", "High" }));
                        Harass.Add("W.Hit", new ComboBox("HitChance For W", 2, new[] { "Low", "Medium", "High" }));
                    }

                    AutoHarass = Main.AddSubMenu("» Auto Harass");
                    {
                        AutoHarass.AddGroupLabel("● Auto Harass Settings ●");
                        AutoHarass.Add("Enabled", new CheckBox("Enabled", true));
                        AutoHarass.Add("AutoHarass.Key", new KeyBind("Toggle Key", true, KeyBind.BindTypes.PressToggle, 'A')).OnValueChange += (sender, args) =>
                        {
                            AutoHarass.Get<CheckBox>("Enabled").CurrentValue = args.NewValue;
                        };
                        AutoHarass.AddLabel("Use On:");
                        foreach (var enemy in EntityManager.Heroes.Enemies)
                        {
                            AutoHarass.Add(enemy.ChampionName, new CheckBox(enemy.ChampionName, true));
                        }
                        AutoHarass.AddGroupLabel("● Spell Settings ●");
                        AutoHarass.Add("Q.Use", new CheckBox("Use Q", true));
                        AutoHarass.AddGroupLabel("● Mana Settings ●");
                        AutoHarass.Add("Q.Mana", new Slider("Min. Mana For Q [{0}%]", 50, 0, 100));
                        AutoHarass.AddGroupLabel("● Prediction Settings ●");
                        AutoHarass.Add("Q.Hit", new ComboBox("HitChance For Q", 2, new[] { "Low", "Medium", "High" }));
                    }

                    LastHit = Main.AddSubMenu("» Last Hit");
                    {
                        LastHit.AddGroupLabel("● Last Hit Settings ●");
                        LastHit.Add("Enabled", new CheckBox("Enabled", true));
                        LastHit.Add("Mode", new ComboBox("Last Hit Mode", 1, new[] { "Use Always As Possible", "Use On Unkillable Minion" }));
                        LastHit.AddGroupLabel("● Spell Settings ●");
                        LastHit.Add("Q.Use", new CheckBox("Use Q", true));
                        LastHit.AddGroupLabel("● Mana Settings ●");
                        LastHit.Add("Q.Mana", new Slider("Min. Mana For Q [{0}%]", 50, 0, 100));
                    }

                    LaneClear = Main.AddSubMenu("» Lane Clear");
                    {
                        LaneClear.AddGroupLabel("● Lane Clear Settings ●");
                        LaneClear.Add("Enabled", new CheckBox("Enabled", true));
                        LaneClear.AddGroupLabel("● Spell Settings ●");
                        LaneClear.Add("Q.Use", new CheckBox("Use Q", true));
                        LaneClear.AddGroupLabel("● Mana Settings ●");
                        LaneClear.Add("Q.Mana", new Slider("Min. Mana For Q [{0}%]", 50, 0, 100));
                    }

                    JungleClear = Main.AddSubMenu("» Jungle Clear");
                    {
                        JungleClear.AddGroupLabel("● Jungle Clear Settings ●");
                        JungleClear.Add("Enabled", new CheckBox("Enabled", true));
                        JungleClear.AddGroupLabel("● Spell Settings ●");
                        JungleClear.Add("Q.Use", new CheckBox("Use Q", true));
                        JungleClear.AddGroupLabel("● Mana Settings ●");
                        JungleClear.Add("Q.Mana", new Slider("Min. Mana For Q [{0}%]", 50, 0, 100));
                    }

                    KS = Main.AddSubMenu("» Kill Steal");
                    {
                        KS.AddGroupLabel("● Kill Steal Settings ●");
                        KS.Add("Enabled", new CheckBox("Enabled", true));
                        KS.AddLabel("Use On:");
                        foreach (var enemy in EntityManager.Heroes.Enemies)
                        {
                            KS.Add(enemy.ChampionName, new CheckBox(enemy.ChampionName, true));
                        }
                        KS.AddGroupLabel("● Spell Settings ●");
                        KS.Add("Q.Use", new CheckBox("Use Q", true));
                        KS.Add("W.Use", new CheckBox("Use W", false));
                        KS.Add("R.Use", new CheckBox("Use R", true));
                        KS.AddGroupLabel("● Mana Settings ●");
                        KS.Add("Q.Mana", new Slider("Min. Mana For Q [{0}%]", 0, 0, 100));
                        KS.Add("W.Mana", new Slider("Min. Mana For W [{0}%]", 0, 0, 100));
                        KS.Add("R.Mana", new Slider("Min. Mana For R [{0}%]", 0, 0, 100));
                        KS.AddGroupLabel("● Prediction Settings ●");
                        KS.Add("Q.Hit", new ComboBox("HitChance For Q", 2, new[] { "Low", "Medium", "High" }));
                        KS.Add("W.Hit", new ComboBox("HitChance For W", 2, new[] { "Low", "Medium", "High" }));
                        KS.Add("R.Hit", new ComboBox("HitChance For R", 2, new[] { "Low", "Medium", "High" }));
                    }

                    Flee = Main.AddSubMenu("» Flee");
                    {
                        Flee.AddGroupLabel("● Flee Settings ●");
                        Flee.Add("Enabled", new CheckBox("Enabled", true));
                        Flee.AddGroupLabel("● Spell Settings ●");
                        Flee.Add("E.Use", new CheckBox("Use E", true));
                    }

                    Draw = Main.AddSubMenu("» Drawings");
                    {
                        Draw.AddGroupLabel("● Draw Settings ●");
                        Draw.Add("Q.Draw", new CheckBox("Draw Q Range", true));
                        Draw.Add("W.Draw", new CheckBox("Draw W Range", true));
                        Draw.Add("E.Draw", new CheckBox("Draw E Range", true));
                        Draw.Add("R.Draw", new CheckBox("Draw R Range", false));
                        Draw.Add("AutoHarass.Status", new CheckBox("Draw Auto Harass Status", true));
                        Draw.AddGroupLabel("● Damage Indicator Settings ●");
                        Draw.Add("DamageIndicator.Enabled", new CheckBox("Enabled", false));
                    }

                    Misc = Main.AddSubMenu("» Miscellaneous");
                    {
                        Misc.AddGroupLabel("● Skin Changer ●");
                        Misc.Add("SkinChanger.Enabled", new CheckBox("Enabled", true));
                        Misc.Add("SkinChanger.ID", new ComboBox("Select Skin", 0, new[] { "Classic", "Nottingham Ezreal", "Striker Ezreal", "Frosted Ezreal", "Explorer Ezreal", "Pulsefire Ezreal", "TPA Ezreal", "Debonair Ezreal", "Ace of Spades Ezreal", "Arcade Ezreal", "Amethyst Chroma", "Meteorite Chroma", "Obsidian Chroma", "Pearl Chroma", "Rose Quartz Chroma", "Ruby Chroma", "Sandstone Chroma", "Striped Chroma" }));
                        Misc.AddGroupLabel("● Anti Gapcloser ●");
                        Misc.Add("E.Gap", new CheckBox("Use E", true));
                        Misc.AddGroupLabel("● Tear Stack ●");
                        Misc.Add("Tear.Use", new CheckBox("Enabled", true));
                        Misc.Add("Tear.Mana", new Slider("Min. Mana For Tear Stacking [{0}%]", 80, 0, 100));
                    }
                }

                Logging.AddEntry(LoggingEntryType.Debug, "@Config.cs: Menu class initialized");
            }
            catch (Exception e)
            {
                Logging.AddEntry(LoggingEntryType.Error, "@Config.cs: Can't initialize menu - {0}", e);
                throw;
            }
        }

        #endregion

        #region Public Static Fields

        /// <summary>
        ///     The Main Menu
        /// </summary>
        public static Menu Main { get; set; }

        /// <summary>
        ///     The Combo Menu
        /// </summary>
        public static Menu Combo { get; set; }

        /// <summary>
        ///     The Harass Menu
        /// </summary>
        public static Menu Harass { get; set; }

        /// <summary>
        ///     The Auto Harass Menu
        /// </summary>
        public static Menu AutoHarass { get; set; }

        /// <summary>
        ///     The Last Hit Menu
        /// </summary>
        public static Menu LastHit { get; set; }

        /// <summary>
        ///     The Lane Clear Menu
        /// </summary>
        public static Menu LaneClear { get; set; }

        /// <summary>
        ///     The Jungle Clear Menu
        /// </summary>
        public static Menu JungleClear { get; set; }

        /// <summary>
        ///     The Kill Steal Menu
        /// </summary>
        public static Menu KS { get; set; }

        /// <summary>
        ///     The Flee Menu
        /// </summary>
        public static Menu Flee { get; set; }

        /// <summary>
        ///     The Draw Menu
        /// </summary>
        public static Menu Draw { get; set; }

        /// <summary>
        ///     The Misc Menu
        /// </summary>
        public static Menu Misc { get; set; }

        #endregion
    }
}
