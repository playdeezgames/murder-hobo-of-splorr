Public Interface IEntity(Of TIdentifier)
    ReadOnly Property Id As TIdentifier
    Property EntityType As String
    Property Metadata(metadataType As String) As String
    ReadOnly Property MetadataTypes As IEnumerable(Of String)
    Property Flag(flagType As String) As Boolean
    Sub Recycle()
    ReadOnly Property World As IWorld
End Interface
