Public Interface IAvatarModel
    ReadOnly Property Attributes As IEnumerable(Of IAttributeModel)
    Sub TurnLeft()
    Sub TurnRight()
    Sub TurnAround()
    Sub MoveAhead()
    ReadOnly Property RoomString As String
    ReadOnly Property HasDoorAhead As Boolean
    ReadOnly Property HasDoorToLeft As Boolean
    ReadOnly Property HasDoorToRight As Boolean
    ReadOnly Property HasDoorBehind As Boolean
End Interface
