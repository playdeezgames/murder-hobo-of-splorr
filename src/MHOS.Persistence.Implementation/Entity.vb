Imports MHOS.Data

Friend MustInherit Class Entity(Of TEntityData, TIdentifier)
    Implements IEntity(Of TIdentifier)
    Protected ReadOnly WorldData As WorldData
    Protected ReadOnly EntityId As TIdentifier
    Sub New(worldData As WorldData, entityId As TIdentifier)
        Me.WorldData = worldData
        Me.EntityId = entityId
    End Sub
    Protected MustOverride ReadOnly Property EntityData As TEntityData
    ReadOnly Property Id As TIdentifier Implements IEntity(Of TIdentifier).Id
        Get
            Return EntityId
        End Get
    End Property
End Class
