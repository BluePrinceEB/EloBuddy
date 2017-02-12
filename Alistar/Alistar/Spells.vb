Imports EloBuddy
Imports EloBuddy.SDK

Module Spells

    Public Q As Spell.Active, W As Spell.Targeted, E As Spell.Active, R As Spell.Active

    Public Sub Initialize()
        Q = New Spell.Active(SpellSlot.Q, 365)
        W = New Spell.Targeted(SpellSlot.W, 650)
        E = New Spell.Active(SpellSlot.E, 365)
        R = New Spell.Active(SpellSlot.R)
    End Sub
End Module
