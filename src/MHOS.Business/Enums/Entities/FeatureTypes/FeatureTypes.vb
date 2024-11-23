Friend Module FeatureTypes
    Friend ReadOnly Sign As String = NameOf(Sign)
    Friend ReadOnly NPC As String = NameOf(NPC)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseFeatureTypeDescriptor) =
        New List(Of BaseFeatureTypeDescriptor) From
        {
            New SignFeatureTypeDescriptor(),
            New NPCFeatureTypeDescriptor()
        }.ToDictionary(Function(x) x.FeatureType, Function(x) x)
End Module
