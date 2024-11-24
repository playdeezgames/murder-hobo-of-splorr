Friend Module FeatureExtensionMethods
    <Extension>
    Function Descriptor(feature As IFeature) As BaseFeatureTypeDescriptor
        Return FeatureTypes.Descriptors(feature.EntityType)
    End Function
    <Extension>
    Function DescriptionLines(feature As IFeature) As IEnumerable(Of String)
        Return feature.Descriptor.DescriptionLines(feature)
    End Function
    <Extension>
    Function BriefDescription(feature As IFeature) As String
        Return feature.Descriptor.BriefDescription(feature)
    End Function
    <Extension>
    Function AllowedVerbs(feature As IFeature, character As ICharacter) As IEnumerable(Of IVerb)
        Return feature.Verbs.Where(Function(x) x.Allows(character))
    End Function
End Module
