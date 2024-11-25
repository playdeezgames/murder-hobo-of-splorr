Public Interface IEntity(Of TIdentifier)
    ReadOnly Property Id As TIdentifier
    Property EntityType As String
    Sub Recycle()
    ReadOnly Property World As IWorld
End Interface
