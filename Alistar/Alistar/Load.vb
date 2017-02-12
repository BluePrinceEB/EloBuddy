Imports EloBuddy
Imports EloBuddy.SDK.Events


Module Load

    Sub Main()
        AddHandler Loading.OnLoadingComplete, AddressOf OnLoad
    End Sub

    Private Sub OnLoad()
        If Player.Instance.Hero <> Champion.Alistar Then
            Return
        End If

        Menu.Initialize()
        Events.Initialize()
        Spells.Initialize()
    End Sub

End Module
