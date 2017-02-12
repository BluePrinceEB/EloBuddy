Imports EloBuddy
Imports EloBuddy.SDK
Imports EloBuddy.SDK.Menu.Values
Imports SharpDX

Module Helper

    Public Sub Update()
        QMana()
        WMana()
    End Sub

    Public Function WQMana() As Boolean
        Return Player.Instance.Mana > QMana() + WMana()
    End Function

    Public Function QMana() As Single
        Return 60 + 5 * Q.Level
    End Function

    Public Function WMana() As Single
        Return 60 + 5 * W.Level
    End Function

    Public Function CCType(unit As Obj_AI_Base) As Boolean
        If unit.HasBuffOfType(BuffType.Flee) Then
            Return True
        End If
        If unit.HasBuffOfType(BuffType.Charm) Then
            Return True
        End If
        If unit.HasBuffOfType(BuffType.Polymorph) Then
            Return True
        End If
        If unit.HasBuffOfType(BuffType.Snare) Then
            Return True
        End If
        If unit.HasBuffOfType(BuffType.Stun) Then
            Return True
        End If
        If unit.HasBuffOfType(BuffType.Taunt) Then
            Return True
        End If
        If unit.HasBuffOfType(BuffType.Suppression) Then
            Return True
        End If
        Return False
    End Function

End Module
