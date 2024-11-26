Public Interface IWorld
    ReadOnly Property Serialized As String
    ReadOnly Property MoveCounter As Integer
    Sub KeepGoing()
End Interface
