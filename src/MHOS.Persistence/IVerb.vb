Public Interface IVerb
    Inherits IEntity(Of Integer)
    Function CreateCondition(conditionType As String) As ICondition
    ReadOnly Property Conditions As IEnumerable(Of ICondition)
End Interface
