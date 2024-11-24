Imports MHOS.Data

Friend MustInherit Class Entity(Of TEntityData As EntityData, TIdentifier)
    Implements IEntity(Of TIdentifier)
    Protected ReadOnly WorldData As WorldData
    Protected ReadOnly EntityId As TIdentifier
    Sub New(worldData As WorldData, entityId As TIdentifier)
        Me.WorldData = worldData
        Me.EntityId = entityId
    End Sub
    Public MustOverride Sub Recycle() Implements IEntity(Of TIdentifier).Recycle
    Protected MustOverride ReadOnly Property EntityData As TEntityData
    ReadOnly Property Id As TIdentifier Implements IEntity(Of TIdentifier).Id
        Get
            Return EntityId
        End Get
    End Property

    Public Property EntityType As String Implements IEntity(Of TIdentifier).EntityType
        Get
            Return EntityData.EntityType
        End Get
        Set(value As String)
            EntityData.EntityType = value
        End Set
    End Property

    Public Property Flag(flagType As String) As Boolean Implements IEntity(Of TIdentifier).Flag
        Get
            Return EntityData.Flags.Contains(flagType)
        End Get
        Set(value As Boolean)
            If value Then
                EntityData.Flags.Add(flagType)
            Else
                EntityData.Flags.Remove(flagType)
            End If
        End Set
    End Property

    Public ReadOnly Property World As IWorld Implements IEntity(Of TIdentifier).World
        Get
            Return New World(WorldData)
        End Get
    End Property
End Class
