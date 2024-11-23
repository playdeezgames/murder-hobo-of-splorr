
Imports MHOS.Data

Friend Class Condition
    Inherits Entity(Of ConditionData, Integer)
    Implements ICondition

    Public Sub New(worldData As WorldData, entityId As Integer)
        MyBase.New(worldData, entityId)
    End Sub

    Protected Overrides ReadOnly Property EntityData As ConditionData
        Get
            Return WorldData.Conditions(EntityId)
        End Get
    End Property

    Public Overrides Sub Recycle()
        WorldData.Conditions(EntityId) = Nothing
    End Sub
End Class
