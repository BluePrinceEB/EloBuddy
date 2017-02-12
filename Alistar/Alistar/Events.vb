Imports EloBuddy
Imports EloBuddy.SDK
Imports EloBuddy.SDK.Enumerations
Imports EloBuddy.SDK.Events
Imports EloBuddy.SDK.Menu.Values
Imports EloBuddy.SDK.Rendering
Imports Microsoft.VisualBasic.ApplicationServices
Imports SharpDX

Module Events
    Public Sub Initialize()
        AddHandler Game.OnUpdate, AddressOf OnUpdate
        AddHandler Obj_AI_Base.OnProcessSpellCast, AddressOf OnProcessSpell
        AddHandler Interrupter.OnInterruptableSpell, AddressOf InterrupterOnInterruptableSpell
        AddHandler Gapcloser.OnGapcloser, AddressOf OnGapcloser
        AddHandler Drawing.OnDraw, AddressOf OnDraw
    End Sub

    Private Sub OnUpdate()
        Helper.Update()

        If SDK.Orbwalker.ActiveModesFlags.Equals(SDK.Orbwalker.ActiveModes.Combo) Then
            Orbwalker.Combo()
        End If

        FlashCombo()
        Ultimate()
    End Sub

    Private Sub OnProcessSpell(sender As Obj_AI_Base, args As GameObjectProcessSpellCastEventArgs)
        Dim UseQ = Menu.Combo("Q").Cast(Of CheckBox)().CurrentValue

        If sender.IsMe And args.SData.Name = W.Name Then
            If UseQ And Q.IsReady() Then
                Q.Cast()
            End If
        End If
    End Sub

    Private Sub InterrupterOnInterruptableSpell(sender As Obj_AI_Base, args As Interrupter.InterruptableSpellEventArgs)
        If Not sender.IsEnemy Or sender.Distance(Player.Instance.Position) > W.Range Or args.DangerLevel <> DangerLevel.High Or Not Misc("IE").Cast(Of CheckBox)().CurrentValue Then
            Return
        End If

        If Misc("IQ").Cast(Of CheckBox)().CurrentValue And sender.IsValidTarget(Q.Range) And Q.IsReady() Then
            Q.Cast()
        End If

        If Misc("IW").Cast(Of CheckBox)().CurrentValue And sender.IsValidTarget(W.Range) And W.IsReady() Then
            W.Cast(sender)
        End If
    End Sub

    Private Sub OnGapcloser(sender As Obj_AI_Base, args As Gapcloser.GapcloserEventArgs)
        If Not Misc("IE").Cast(Of CheckBox)().CurrentValue Then
            Return
        End If

        If Misc("AQ").Cast(Of CheckBox)().CurrentValue And args.Sender.Distance(Player.Instance.Position) < Q.Range And Q.IsReady() Then
            Q.Cast()
        End If

        If Misc("AW").Cast(Of CheckBox)().CurrentValue And args.Sender.Distance(Player.Instance.Position) < W.Range And W.IsReady() Then
            W.Cast(args.Sender)
        End If
    End Sub

    Private Sub OnDraw(args As EventArgs)
        If Misc("Disable").Cast(Of CheckBox)().CurrentValue Or Player.Instance.IsDead Then
            Return
        End If

        Dim target = TargetSelector.GetTarget(2000, DamageType.Magical)

        Drawing.DrawCircle(Player.Instance.Position, W.Range, System.Drawing.Color.Red)
        Drawing.DrawCircle(Player.Instance.Position, 425, System.Drawing.Color.Yellow)

        If target Is Nothing Then
            Return
        End If

        Dim Pos As Vector3 = Player.Instance.Position - (target.Position - Player.Instance.Position).Normalized() * -W.Range
        Dim Pos2 As Vector3 = target.Position - (Player.Instance.Position - target.Position).Normalized() * W.Range

        If target.Position.Distance(Player.Instance.Position) >= W.Range Then
            Drawing.DrawLine(Player.Instance.Position.WorldToScreen().X, Player.Instance.Position.WorldToScreen().Y, target.Position.WorldToScreen().X, target.Position.WorldToScreen().Y, 3, System.Drawing.Color.Red)
            Drawing.DrawCircle(Pos, 159, System.Drawing.Color.Blue)
            Drawing.DrawLine(Player.Instance.Position.WorldToScreen().X, Player.Instance.Position.WorldToScreen().Y, Pos.WorldToScreen().X, Pos.WorldToScreen().Y, 3, System.Drawing.Color.Green)
        Else
            Drawing.DrawCircle(target.Position, 159, System.Drawing.Color.Blue)
            Drawing.DrawLine(Player.Instance.Position.WorldToScreen().X, Player.Instance.Position.WorldToScreen().Y, target.Position.WorldToScreen().X, target.Position.WorldToScreen().Y, 3, System.Drawing.Color.Green)
        End If

        Drawing.DrawCircle(Pos2, 159, System.Drawing.Color.YellowGreen)
        Drawing.DrawLine(target.Position.WorldToScreen().X, target.Position.WorldToScreen().Y, Pos2.WorldToScreen().X, Pos2.WorldToScreen().Y, 3, System.Drawing.Color.YellowGreen)

    End Sub
End Module
