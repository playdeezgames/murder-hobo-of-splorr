Friend Class NPCFeatureTypeDescriptor
    Inherits BaseFeatureTypeDescriptor

    Public Sub New()
        MyBase.New(FeatureTypes.NPC)
    End Sub

    Friend Overrides Function DescriptionLines(feature As IFeature) As IEnumerable(Of String)
        Return {
            feature.Name
            }
    End Function

    Friend Overrides Function BriefDescription(feature As IFeature) As String
        Return feature.Metadata(MetadataTypes.ShortName)
    End Function
End Class
