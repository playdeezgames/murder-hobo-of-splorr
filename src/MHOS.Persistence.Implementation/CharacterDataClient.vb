Imports MHOS.Data

Friend MustInherit Class CharacterDataClient
    Inherits Entity(Of CharacterData)
    Protected ReadOnly worldData As WorldData
    Protected ReadOnly EntityId As Integer
    Protected ReadOnly Property EntityData As CharacterData
        Get
            Return worldData.Characters(EntityId)
        End Get
    End Property

    Public Sub New(worldData As Data.WorldData, entityId As Integer)
        Me.worldData = worldData
        Me.EntityId = entityId
    End Sub
End Class
