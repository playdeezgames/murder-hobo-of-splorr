Public Interface ICharacter
    ReadOnly Property Id As Integer
    Property Location As ILocation
    Property Facing As String
    ReadOnly Property World As IWorld
End Interface
