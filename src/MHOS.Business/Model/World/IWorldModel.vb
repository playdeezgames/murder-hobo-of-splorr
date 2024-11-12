Public Interface IWorldModel
    Sub Embark()
    Sub Abandon()
    Sub Load(filename As String)
    Sub Save(filename As String)
    Sub TurnLeft()
    Sub TurnRight()
    Sub MoveAhead()
    ReadOnly Property Facing As String
    ReadOnly Property RoomString As String
    ReadOnly Property Column As Integer
    ReadOnly Property Row As Integer
    ReadOnly Property SectionName As String
    Sub TurnAround()
    ReadOnly Property Options As IEmbarkOptions
    ReadOnly Property Avatar As IAvatarModel
End Interface
