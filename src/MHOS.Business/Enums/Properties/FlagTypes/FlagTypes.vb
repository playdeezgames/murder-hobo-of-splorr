Friend Module FlagTypes
    Private ReadOnly TownGate As String = NameOf(TownGate)
    Private ReadOnly TownGateDirections As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            Directions.North,
            Directions.East,
            Directions.South,
            Directions.West
        }
    Friend Function TownGateDirection(direction As String) As String
        Return $"{TownGate}{direction}"
    End Function
    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, FlagTypeDescriptor) =
        CreateDescriptors()
    Private Function CreateDescriptors() As IReadOnlyDictionary(Of String, FlagTypeDescriptor)
        Dim result As New List(Of FlagTypeDescriptor)
        For Each direction In TownGateDirections
            result.Add(New TownGateFlagTypeDescriptor(direction))
        Next
        Return result.ToDictionary(Function(x) x.FlagType, Function(x) x)
    End Function
End Module
