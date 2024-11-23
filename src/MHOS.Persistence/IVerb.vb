Public Interface IVerb
    Inherits IEntity(Of Integer)
    Function CreateCondition(conditionType As String) As ICondition
End Interface
