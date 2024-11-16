Friend Class TownGateFlagTypeDescriptor
    Inherits FlagTypeDescriptor
    Public Sub New(direction As String)
        MyBase.New(FlagTypes.TownGateDirection(direction))
    End Sub
End Class
