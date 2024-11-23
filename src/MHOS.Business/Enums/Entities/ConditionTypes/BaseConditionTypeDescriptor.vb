Friend MustInherit Class BaseConditionTypeDescriptor
    Friend ReadOnly Property ConditionType As String
    Sub New(conditionType As String)
        Me.ConditionType = conditionType
    End Sub
    Friend MustOverride Function Allows(condition As ICondition, character As ICharacter) As Boolean
End Class
