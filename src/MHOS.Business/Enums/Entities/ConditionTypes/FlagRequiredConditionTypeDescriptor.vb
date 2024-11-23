Friend Class FlagRequiredConditionTypeDescriptor
    Inherits BaseConditionTypeDescriptor

    Public Sub New()
        MyBase.New(ConditionTypes.FlagRequired)
    End Sub

    Friend Overrides Function Allows(condition As ICondition, character As ICharacter) As Boolean
        Dim flag = condition.Metadata(MetadataTypes.Flag)
        Return character.Flag(flag)
    End Function
End Class
