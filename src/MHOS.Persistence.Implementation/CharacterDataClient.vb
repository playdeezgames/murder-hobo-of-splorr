Imports MHOS.Data

Friend MustInherit Class CharacterDataClient
    Inherits Entity(Of CharacterData)
    Protected ReadOnly worldData As WorldData
    Protected CharacterId As Integer
    Protected ReadOnly Property CharacterData As CharacterData
        Get
            Return WorldData.Characters(CharacterId)
        End Get
    End Property

    Public Sub New(worldData As Data.WorldData, characterId As Integer)
        Me.worldData = worldData
    End Sub
End Class
