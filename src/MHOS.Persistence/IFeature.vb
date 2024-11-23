Public Interface IFeature
    Inherits IEntity(Of (LocationId As Integer, FeatureId As Integer))
    Function CreateVerb(verbType As String) As IVerb
End Interface
