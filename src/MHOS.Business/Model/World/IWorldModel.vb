Public Interface IWorldModel
    ReadOnly Property Session As IWorldSessionModel
    Sub Embark()
    Sub Abandon()
    Sub Load(filename As String)
    Sub Save(filename As String)

    ReadOnly Property Avatar As IAvatarModel
End Interface
