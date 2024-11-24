Friend Module FlagTypes
    Private ReadOnly TownGate As String = NameOf(TownGate)
    Friend Function TownGateDirection(direction As String) As String
        Return $"{TownGate}{direction}"
    End Function
End Module
