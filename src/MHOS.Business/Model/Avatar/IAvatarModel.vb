Public Interface IAvatarModel
    ReadOnly Property Attributes As IEnumerable(Of IAttributeModel)
    Sub TurnLeft()
    Sub TurnRight()
    Sub TurnAround()
    Sub MoveAhead()
    ReadOnly Property Facing As String
    ReadOnly Property RoomString As String
End Interface
