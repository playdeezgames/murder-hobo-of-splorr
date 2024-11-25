Public Class CharacterData
    Inherits EntityData
    Public Property LocationId As Integer
    Public Property Messages As New List(Of MessageData)
    Public Property ExperiencePoints As Integer
    Public Property HitPoints As Integer
    Public Property LevelHitDieRoll As New Dictionary(Of Integer, Integer)
    Public Property Race As String
    Public Property [Class] As String
End Class
