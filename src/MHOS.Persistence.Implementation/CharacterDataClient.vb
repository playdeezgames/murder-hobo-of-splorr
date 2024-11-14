Imports MHOS.Data

Friend MustInherit Class CharacterDataClient
    Inherits Entity(Of CharacterData, Integer)
    Protected Overrides ReadOnly Property EntityData As CharacterData
        Get
            Return WorldData.Characters(EntityId)
        End Get
    End Property

    Public Sub New(worldData As Data.WorldData, entityId As Integer)
        MyBase.New(worldData, entityId)
    End Sub
End Class
