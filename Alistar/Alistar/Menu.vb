Imports EloBuddy.SDK
Imports EloBuddy.SDK.Menu
Imports EloBuddy.SDK.Menu.Values

Module Menu

    Public Settings, Combo, Flash, Misc As EloBuddy.SDK.Menu.Menu

    Public Sub Initialize()
        Settings = MainMenu.AddMenu("Alistar", "Alistar")

        Combo = Settings.AddSubMenu("Combo", "Combo")
        Combo.AddGroupLabel("Combo Settings")
        Combo.Add("Q", New CheckBox("Use Q", True))
        Combo.Add("W", New CheckBox("Use W", True))
        Combo.Add("E", New CheckBox("Use E", True))
        Combo.Add("R", New CheckBox("Use R", True))

        Flash = Settings.AddSubMenu("Flash Combo", "Flash")
        Flash.AddGroupLabel("Flash Combo Settings")
        Flash.Add("Enabled", New CheckBox("Enabled", True))
        Flash.Add("Key", New KeyBind("Key", False, KeyBind.BindTypes.HoldActive, 90))
        Flash.AddGroupLabel("Instruction :")
        Flash.AddLabel("1. Select Target.")
        Flash.AddLabel("2. Pless Flash Combo Key.")

        Misc = Settings.AddSubMenu("Misc", "Misc")
        Misc.AddGroupLabel("Drawings")
        Misc.Add("Disable", New CheckBox("Disable All Drawings", False))
        Misc.AddGroupLabel("Interrupter")
        Misc.Add("IE", New CheckBox("Enabled", True))
        Misc.Add("IQ", New CheckBox("Use Q", True))
        Misc.Add("IW", New CheckBox("Use W", True))
        Misc.AddGroupLabel("AntiGapCloser")
        Misc.Add("AE", New CheckBox("Enabled", True))
        Misc.Add("AQ", New CheckBox("Use Q", True))
        Misc.Add("AW", New CheckBox("Use W", True))
    End Sub
End Module
