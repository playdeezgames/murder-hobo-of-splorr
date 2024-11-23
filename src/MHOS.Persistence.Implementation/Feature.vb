
Imports MHOS.Data

Friend Class Feature
    Inherits Entity(Of FeatureData, (LocationId As Integer, FeatureId As Integer))
    Implements IFeature

    Public Sub New(worldData As WorldData, LocationId As Integer, FeatureId As Integer)
        MyBase.New(worldData, (LocationId, FeatureId))
    End Sub

    Protected Overrides ReadOnly Property EntityData As FeatureData
        Get
            Return WorldData.Locations(EntityId.LocationId).Features(EntityId.FeatureId)
        End Get
    End Property

    Public Overrides Sub Recycle()
        WorldData.Locations(EntityId.LocationId).Features(EntityId.FeatureId) = Nothing
    End Sub

    Public Function CreateVerb(verbType As String) As IVerb Implements IFeature.CreateVerb
        Dim verb As IVerb = World.CreateVerb(verbType)
        EntityData.Verbs.Add(verb.Id)
        Return verb
    End Function
End Class
