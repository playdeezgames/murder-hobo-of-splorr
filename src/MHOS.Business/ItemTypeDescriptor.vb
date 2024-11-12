Public Class ItemTypeDescriptor
    Private ReadOnly onUse As Action(Of ICharacter)
    Sub New(displayName As String, onUse As Action(Of ICharacter), position As (x As Integer, y As Integer), text As String, hue As Integer, Optional spawnCount As Integer = 1, Optional isConsumed As Boolean = True)
        Me.SpawnCount = spawnCount
        Me.onUse = onUse
        Me.DisplayName = displayName
        Me.IsConsumed = isConsumed
        Me.Position = position
        Me.Text = text
        Me.Hue = hue
    End Sub
    ReadOnly Property DisplayName As String
    ReadOnly Property SpawnCount As Integer
    ReadOnly Property IsConsumed As Boolean
    ReadOnly Property Position As (X As Integer, Y As Integer)
    ReadOnly Property Text As String
    ReadOnly Property Hue As Integer
    Friend Sub Use(character As ICharacter)
        onUse(character)
    End Sub
End Class
