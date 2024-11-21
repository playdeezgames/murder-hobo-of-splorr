Friend Module CharacterTypes
    Friend ReadOnly Player As String = NameOf(Player)
    Friend Descriptors As IReadOnlyDictionary(Of String, BaseCharacterTypeDescriptor) =
        New List(Of BaseCharacterTypeDescriptor) From
        {
            New PlayerCharacterTypeDescriptor()
        }.ToDictionary(Function(x) x.CharacterType, Function(x) x)
End Module
