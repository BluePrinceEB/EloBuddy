Imports EloBuddy
Imports EloBuddy.SDK
Imports EloBuddy.SDK.Menu.Values

Module Orbwalker
    Public Sub Combo()
        Dim UseQ = Menu.Combo("Q").Cast(Of CheckBox)().CurrentValue
        Dim UseW = Menu.Combo("W").Cast(Of CheckBox)().CurrentValue
        Dim UseE = Menu.Combo("E").Cast(Of CheckBox)().CurrentValue

        Dim target = TargetSelector.GetTarget(W.Range, DamageType.Magical)

        If target Is Nothing Then
            Return
        End If

        If UseQ Then
            If Q.IsReady() And target.IsValidTarget(Q.Range) Then
                Q.Cast()
            End If
        End If

        If UseQ And UseW And WQMana() And target.Position.Distance(Player.Instance.Position) > Q.Range Then
            If Q.IsReady() And W.IsReady() And target.IsValidTarget(W.Range) Then
                W.Cast(target)
            End If
        End If

        If UseW And Not UseQ Then
            If W.IsReady() And target.IsValidTarget(W.Range) Then
                W.Cast(target)
            End If
        End If

        If UseE Then
            If E.IsReady() And target.IsValidTarget(Q.Range) Then
                E.Cast()
            End If
        End If
    End Sub

    Public Sub FlashCombo()
        Dim Enabled = Flash("Enabled").Cast(Of CheckBox)().CurrentValue
        Dim Key = Flash("Key").Cast(Of KeyBind)().CurrentValue
        Dim FlashSlot = Player.Instance.GetSpellSlotFromName("SummonerFlash")

        If Enabled And Key Then
            Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos)

            If Q.IsReady() Then
                Dim target = TargetSelector.SelectedTarget

                If target Is Nothing Then
                    Return
                End If

                If target.IsValidTarget(W.Range) And target.Position.Distance(Player.Instance.Position) > Q.Range Then

                    If Player.Instance.Spellbook.CanUseSpell(FlashSlot) = SpellState.Ready Then
                        Player.Instance.Spellbook.CastSpell(FlashSlot, target.ServerPosition)
                        Core.DelayAction(Function() Q.Cast(), 50)
                    End If
                End If
            End If
        End If
    End Sub

    Public Sub Ultimate()
        Dim UseR = Menu.Combo("R").Cast(Of CheckBox)().CurrentValue

        If UseR Then
            If R.IsReady() Then
                Dim Enemy = EntityManager.Enemies.FirstOrDefault(Function(x) x.Distance(Player.Instance.Position) < Q.Range)

                If Enemy IsNot Nothing And CCType(Player.Instance) Then
                    R.Cast()
                End If
            End If
        End If
    End Sub
End Module
