Public Interface IWorldSessionModel
    Sub Embark()
    Sub Abandon()
    Sub Load(filename As String)
    Sub Save(filename As String)
    ReadOnly Property Options As IEmbarkOptions
End Interface
