Friend Class NPCFeatureTypeDescriptor
    Inherits BaseFeatureTypeDescriptor

    Public Sub New()
        MyBase.New(FeatureTypes.NPC)
    End Sub

    Friend Overrides Function DescriptionLines(feature As IFeature) As IEnumerable(Of String)
        Return {
            feature.Metadata(MetadataTypes.Name)
            }
    End Function
End Class
