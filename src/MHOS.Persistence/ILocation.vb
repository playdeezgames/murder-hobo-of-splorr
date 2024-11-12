Public Interface ILocation
    ReadOnly Property Column As Integer
    ReadOnly Property Row As Integer
    Sub SetNeighbor(direction As String, nextLocation As ILocation)
    Function GetNeighbor(direction As String) As ILocation
    ReadOnly Property Id As Integer
    Sub SetDoor(direction As String, door As String)
    Sub AddCharacter(result As ICharacter)
    Function HasDoor(direction As String) As Boolean
    Sub RemoveCharacter(character As ICharacter)
    ReadOnly Property HasCharacter As Boolean
End Interface
