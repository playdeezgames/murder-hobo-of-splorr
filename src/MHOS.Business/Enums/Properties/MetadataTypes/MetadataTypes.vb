Friend Module MetadataTypes
    Friend ReadOnly Race As String = NameOf(Race)
    Friend ReadOnly [Class] As String = NameOf([Class])
    Friend ReadOnly SignText As String = NameOf(SignText)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseMetadataTypeDescriptor) =
        New List(Of BaseMetadataTypeDescriptor) From
        {
            New RaceMetadataTypeDescriptor(),
            New ClassMetadataTypeDescriptor(),
            New SignTextMetadataTypeDescriptor()
        }.ToDictionary(Function(x) x.MetadataType, Function(x) x)
End Module
