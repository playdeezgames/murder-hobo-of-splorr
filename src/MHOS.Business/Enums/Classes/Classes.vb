Friend Module Classes
    Friend ReadOnly Fighter As String = NameOf(Fighter)
    Friend ReadOnly Cleric As String = NameOf(Cleric)
    Friend ReadOnly Thief As String = NameOf(Thief)
    Friend ReadOnly MagicUser As String = NameOf(MagicUser)
    Friend ReadOnly FighterMagicUser As String = NameOf(FighterMagicUser)
    Friend ReadOnly MagicUserThief As String = NameOf(MagicUserThief)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseClassDescriptor) =
        New List(Of BaseClassDescriptor) From
        {
            New FighterClassDescriptor(),
            New ClericClassDescriptor(),
            New ThiefClassDescriptor(),
            New MagicUserClassDescriptor(),
            New FighterMagicUserClassDescriptor(),
            New MagicUserThiefClassDescriptor()
        }.ToDictionary(Function(x) x.Class, Function(x) x)
End Module
