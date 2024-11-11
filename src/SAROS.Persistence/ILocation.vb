Public Interface ILocation
    ReadOnly Property Column As Integer
    ReadOnly Property Row As Integer
    Sub SetNeighbor(direction As String, nextLocation As ILocation)
    ReadOnly Property Id As Integer
    Sub SetDoor(direction As String, door As String)
    Sub AddCharacter(result As ICharacter)
    Function HasDoor(direction As String) As Boolean
    Sub RemoveCharacter(character As ICharacter)
    Sub AddItem(item As IItem)
    Sub RemoveItem(item As IItem)
    ReadOnly Property HasCharacter As Boolean
    ReadOnly Property HasItems As Boolean
    ReadOnly Property Items As IEnumerable(Of IItem)
End Interface
