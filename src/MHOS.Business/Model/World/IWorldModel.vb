Public Interface IWorldModel
    'TODO: to IWorldSessionModel
    Sub Embark()
    Sub Abandon()
    Sub Load(filename As String)
    Sub Save(filename As String)
    ReadOnly Property Options As IEmbarkOptions

    ReadOnly Property Avatar As IAvatarModel
End Interface
