
Imports MHOS.Data

Friend Class Feature
    Inherits Entity(Of FeatureData, (LocationId As Integer, FeatureId As Integer))
    Implements IFeature

    Public Sub New(worldData As WorldData, LocationId As Integer, FeatureId As Integer)
        MyBase.New(worldData, (LocationId, FeatureId))
    End Sub

    Public Property SignText As String Implements IFeature.SignText
        Get
            Return EntityData.SignText
        End Get
        Set(value As String)
            EntityData.SignText = value
        End Set
    End Property

    Public Property Name As String Implements IFeature.Name
        Get
            Return EntityData.Name
        End Get
        Set(value As String)
            EntityData.Name = value
        End Set
    End Property

    Public Property ShortName As String Implements IFeature.ShortName
        Get
            Return EntityData.ShortName
        End Get
        Set(value As String)
            EntityData.ShortName = value
        End Set
    End Property

    Protected Overrides ReadOnly Property EntityData As FeatureData
        Get
            Return WorldData.Locations(EntityId.LocationId).Features(EntityId.FeatureId)
        End Get
    End Property

    Public Overrides Sub Recycle()
        WorldData.Locations(EntityId.LocationId).Features(EntityId.FeatureId) = Nothing
    End Sub
End Class
