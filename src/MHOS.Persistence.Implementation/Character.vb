Imports MHOS.Data

Friend Class Character
    Inherits Entity(Of CharacterData, Integer)
    Implements ICharacter
    Protected Overrides ReadOnly Property EntityData As CharacterData
        Get
            Return WorldData.Characters(EntityId)
        End Get
    End Property

    Public Sub New(worldData As Data.WorldData, characterId As Integer)
        MyBase.New(worldData, characterId)
    End Sub

    Public Property Location As ILocation Implements ICharacter.Location
        Get
            Return New Location(worldData, EntityData.LocationId)
        End Get
        Set(value As ILocation)
            If value.Id <> EntityData.LocationId Then
                Location.RemoveCharacter(Me)
                EntityData.LocationId = value.Id
                Location.AddCharacter(Me)
            End If
        End Set
    End Property

    Public ReadOnly Property Messages As IEnumerable(Of (Text As String, Mood As String)) Implements ICharacter.Messages
        Get
            Return EntityData.Messages.Select(Function(x) (x.Text, x.Mood))
        End Get
    End Property

    Public Property ExperiencePoints As Integer Implements ICharacter.ExperiencePoints
        Get
            Return EntityData.ExperiencePoints
        End Get
        Set(value As Integer)
            EntityData.ExperiencePoints = value
        End Set
    End Property

    Public Property HitPoints As Integer Implements ICharacter.HitPoints
        Get
            Return EntityData.HitPoints
        End Get
        Set(value As Integer)
            EntityData.HitPoints = value
        End Set
    End Property

    Public Property LevelHitDieRoll(level As Integer) As Integer Implements ICharacter.LevelHitDieRoll
        Get
            Return EntityData.LevelHitDieRoll(level)
        End Get
        Set(value As Integer)
            EntityData.LevelHitDieRoll(level) = value
        End Set
    End Property

    Public Property Race As String Implements ICharacter.Race
        Get
            Return EntityData.Race
        End Get
        Set(value As String)
            EntityData.Race = value
        End Set
    End Property

    Public Property [Class] As String Implements ICharacter.Class
        Get
            Return EntityData.Class
        End Get
        Set(value As String)
            EntityData.Class = value
        End Set
    End Property

    Public Sub AddMessage(text As String, mood As String) Implements ICharacter.AddMessage
        EntityData.Messages.Add(New Data.MessageData With {.Text = text, .Mood = mood})
    End Sub

    Public Sub ClearMessages() Implements ICharacter.ClearMessages
        EntityData.Messages.Clear()
    End Sub

    Public Overrides Sub Recycle()
        WorldData.Characters(EntityId) = Nothing
    End Sub
End Class