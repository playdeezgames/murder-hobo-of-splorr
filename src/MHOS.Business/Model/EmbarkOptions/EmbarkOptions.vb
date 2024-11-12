Public Class EmbarkOptions
    Implements IEmbarkOptions
    Private ReadOnly _attributes As New Dictionary(Of String, Integer)

    Public ReadOnly Property Attributes As IEnumerable(Of IAttributeModel) Implements IEmbarkOptions.Attributes
        Get
            Return _attributes.Select(Function(x) New AttributeModel(x.Key, x.Value))
        End Get
    End Property

    Public Sub Initialize() Implements IEmbarkOptions.Initialize
        _attributes.Clear()
        For Each entry In AttributeTypes.Descriptors
            _attributes(entry.Key) = RNG.RollDice("3d6")
        Next
    End Sub
End Class
