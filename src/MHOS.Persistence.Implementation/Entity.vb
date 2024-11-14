Imports MHOS.Data

Friend MustInherit Class Entity(Of TEntityData As EntityData, TIdentifier)
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

    Public Property EntityType As String Implements IEntity(Of TIdentifier).EntityType
        Get
            Return EntityData.EntityType
        End Get
        Set(value As String)
            EntityData.EntityType = value
        End Set
    End Property

    Public ReadOnly Property CounterTypes As IEnumerable(Of String) Implements IEntity(Of TIdentifier).CounterTypes
        Get
            Return EntityData.Counters.Keys
        End Get
    End Property

    Public Property Counter(counterType As String) As Integer? Implements IEntity(Of TIdentifier).Counter
        Get
            Dim counterValue As Integer = 0
            If EntityData.Counters.TryGetValue(counterType, counterValue) Then
                Return counterValue
            End If
            Return Nothing
        End Get
        Set(value As Integer?)
            If value.HasValue Then
                EntityData.Counters(counterType) = value.Value
            Else
                EntityData.Counters.Remove(counterType)
            End If
        End Set
    End Property
End Class
