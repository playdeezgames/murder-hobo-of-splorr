Public Interface IFeature
    Inherits IEntity(Of (LocationId As Integer, FeatureId As Integer))
    Function CreateVerb(verbType As String) As IVerb
    ReadOnly Property Verbs As IEnumerable(Of IVerb)
End Interface
