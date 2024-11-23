Friend Module FeatureExtensionMethods
    <Extension>
    Function Descriptor(feature As IFeature) As BaseFeatureTypeDescriptor
        Return FeatureTypes.Descriptors(feature.EntityType)
    End Function
    <Extension>
    Function DescriptionLines(feature As IFeature) As IEnumerable(Of String)
        Return feature.Descriptor.DescriptionLines(feature)
    End Function
End Module
