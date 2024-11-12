Public Interface IWorldModel
    Sub Embark()
    Sub Abandon()
    Sub Load(filename As String)
    Sub Save(filename As String)

    Sub TurnLeft()
    Sub TurnRight()
    Sub TurnAround()
    Sub MoveAhead()
    ReadOnly Property Facing As String
    ReadOnly Property RoomString As String

    ReadOnly Property Options As IEmbarkOptions
    ReadOnly Property Avatar As IAvatarModel
End Interface
