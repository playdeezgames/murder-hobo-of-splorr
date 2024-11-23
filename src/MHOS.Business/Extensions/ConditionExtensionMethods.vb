Friend Module ConditionExtensionMethods
    <Extension>
    Friend Function Allows(condition As ICondition, character As ICharacter) As Boolean
        Return ConditionTypes.Descriptors(condition.EntityType).Allows(condition, character)
    End Function
End Module
