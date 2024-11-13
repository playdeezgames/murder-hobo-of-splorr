Friend Module CharacterTypes
    Friend ReadOnly N00b As String = NameOf(N00b)
    Friend Descriptors As IReadOnlyDictionary(Of String, BaseCharacterTypeDescriptor) =
        New List(Of BaseCharacterTypeDescriptor) From
        {
            New N00bCharacterTypeDescriptor()
        }.ToDictionary(Function(x) x.CharacterType, Function(x) x)
End Module
