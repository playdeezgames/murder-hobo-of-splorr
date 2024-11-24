Public Interface IFeature
    Inherits IEntity(Of (LocationId As Integer, FeatureId As Integer))
    Property SignText As String
    Property Name As String
    Property ShortName As String
End Interface
