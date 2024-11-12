Public Interface IWorldModel
    Sub Embark()
    Sub Abandon()
    Sub Load(filename As String)
    Sub Save(filename As String)

    Sub TurnLeft()
    Sub TurnRight()
    Sub TurnAround()

    ReadOnly Property Options As IEmbarkOptions
    ReadOnly Property Avatar As IAvatarModel
End Interface
