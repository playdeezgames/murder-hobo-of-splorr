Public Interface IAvatarModel
    ReadOnly Property Attributes As IEnumerable(Of IAttributeModel)
    ReadOnly Property Location As ILocationModel
    Sub TurnLeft()
    Sub TurnRight()
    Sub TurnAround()
    Sub MoveAhead()
    ReadOnly Property HasDoorAhead As Boolean
    ReadOnly Property HasDoorToLeft As Boolean
    ReadOnly Property HasDoorToRight As Boolean
    ReadOnly Property HasDoorBehind As Boolean
    ReadOnly Property Name As String
End Interface
