Public Class CharacterData
    Inherits EntityData
    Public Property LocationId As Integer
    Public Property Messages As New List(Of MessageData)
    Public Property Strength As Integer
    Public Property Intelligence As Integer
    Public Property Wisdom As Integer
    Public Property Dexterity As Integer
    Public Property Constitution As Integer
    Public Property Charisma As Integer
End Class
