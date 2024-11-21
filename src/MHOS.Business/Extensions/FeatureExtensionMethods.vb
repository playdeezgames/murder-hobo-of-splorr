Friend Module FeatureExtensionMethods
    <Extension>
    Function Descriptor(feature As IFeature) As BaseFeatureTypeDescriptor
        Return FeatureTypes.Descriptors(feature.EntityType)
    End Function
End Module
