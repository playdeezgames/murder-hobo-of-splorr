Public Interface IEntity(Of TIdentifier)
    ReadOnly Property Id As TIdentifier
    Property EntityType As String
    Property Flag(flagType As String) As Boolean
    Sub Recycle()
    ReadOnly Property World As IWorld
End Interface
