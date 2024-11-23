Friend Class FlagForbiddenConditionTypeDescriptor
    Inherits BaseConditionTypeDescriptor

    Public Sub New()
        MyBase.New(ConditionTypes.FlagForbidden)
    End Sub

    Friend Overrides Function Allows(condition As ICondition, character As ICharacter) As Boolean
        Dim flag = condition.Metadata(MetadataTypes.Flag)
        Return Not character.Flag(flag)
    End Function
End Class
