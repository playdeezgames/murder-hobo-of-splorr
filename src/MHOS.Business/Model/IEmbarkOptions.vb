Public Interface IEmbarkOptions
    Sub Initialize()
    ReadOnly Property Attributes As IEnumerable(Of IAttributeModel)
End Interface
