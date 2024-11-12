Public Interface ICharacter
    ReadOnly Property Id As Integer
    Property Location As ILocation
    Property Facing As String
    ReadOnly Property World As IWorld
    Sub SetAttribute(attributeType As String, value As Integer)
    Function GetAttribute(attributeType As String) As Integer
    ReadOnly Property Attributes As IEnumerable(Of String)
    ReadOnly Property CharacterType As String
End Interface
