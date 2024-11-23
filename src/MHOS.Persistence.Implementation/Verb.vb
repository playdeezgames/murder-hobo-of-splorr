
Imports MHOS.Data

Friend Class Verb
    Inherits Entity(Of VerbData, Integer)
    Implements IVerb

    Public Sub New(worldData As WorldData, entityId As Integer)
        MyBase.New(worldData, entityId)
    End Sub

    Protected Overrides ReadOnly Property EntityData As VerbData
        Get
            Return WorldData.Verbs(EntityId)
        End Get
    End Property

    Public Overrides Sub Recycle()
        WorldData.Verbs(EntityId) = Nothing
    End Sub

    Public Function CreateCondition(conditionType As String) As ICondition Implements IVerb.CreateCondition
        Dim condition As ICondition = World.CreateCondition(conditionType)
        EntityData.Conditions.Add(condition.Id)
        Return condition
    End Function
End Class
