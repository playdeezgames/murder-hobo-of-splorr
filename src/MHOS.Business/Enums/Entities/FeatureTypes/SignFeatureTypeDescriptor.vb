Friend Class SignFeatureTypeDescriptor
    Inherits BaseFeatureTypeDescriptor

    Public Sub New()
        MyBase.New(FeatureTypes.Sign)
    End Sub

    Friend Overrides Function DescriptionLines(feature As IFeature) As IEnumerable(Of String)
        Return {
            $"The sign says '{feature.Metadata(MetadataTypes.SignText)}'."
            }
    End Function
End Class
