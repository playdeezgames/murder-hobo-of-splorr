Public Class LocationData
    Inherits EntityData
    Property Routes As New Dictionary(Of String, RouteData)
    Property Characters As New HashSet(Of Integer)
    Property Features As New List(Of FeatureData)
End Class
