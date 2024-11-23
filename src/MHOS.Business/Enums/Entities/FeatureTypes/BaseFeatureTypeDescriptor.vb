Friend MustInherit Class BaseFeatureTypeDescriptor
    Friend ReadOnly Property FeatureType As String
    Friend Sub New(featureType As String)
        Me.FeatureType = featureType
    End Sub
    Friend MustOverride Function DescriptionLines(feature As IFeature) As IEnumerable(Of String)
    Friend MustOverride Function BriefDescription(feature As IFeature) As String
End Class
