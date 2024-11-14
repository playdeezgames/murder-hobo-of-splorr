Friend Module MetadataTypes
    Friend ReadOnly Race As String = NameOf(Race)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseMetadataTypeDescriptor) =
        New List(Of BaseMetadataTypeDescriptor) From
        {
            New RaceMetadataTypeDescriptor()
        }.ToDictionary(Function(x) x.MetadataType, Function(x) x)
End Module
