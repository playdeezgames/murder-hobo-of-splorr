Public Interface ICharacter
    ReadOnly Property Id As Integer
    Property Location As ILocation
    Property Facing As String
    ReadOnly Property World As IWorld
    Property Attribute(attributeType As String) As Integer
    ReadOnly Property Attributes As IEnumerable(Of String)
    ReadOnly Property CharacterType As String
End Interface
