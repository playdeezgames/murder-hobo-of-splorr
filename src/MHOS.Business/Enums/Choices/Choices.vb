Friend Module Choices
    Friend ReadOnly Status As String = NameOf(Status)
    Friend ReadOnly Cancel As String = NameOf(Cancel)
    Friend ReadOnly Initialize As String = NameOf(Initialize)
    Friend ReadOnly [Next] As String = NameOf([Next])
    Friend ReadOnly MoveMenu As String = NameOf(MoveMenu)
    Friend ReadOnly MoveNorth As String = NameOf(MoveNorth)
    Friend ReadOnly MoveEast As String = NameOf(MoveEast)
    Friend ReadOnly MoveSouth As String = NameOf(MoveSouth)
    Friend ReadOnly MoveWest As String = NameOf(MoveWest)
    Friend ReadOnly Dwarf As String = NameOf(Dwarf)
    Friend ReadOnly Elf As String = NameOf(Elf)
    Friend ReadOnly Halfling As String = NameOf(Halfling)
    Friend ReadOnly Man As String = NameOf(Man)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseChoiceDescriptor) =
        New List(Of BaseChoiceDescriptor) From
        {
            New StatusChoiceDescriptor(),
            New CancelChoiceDescriptor(),
            New InitializeChoiceDescriptor(),
            New NextChoiceDescriptor(),
            New MoveMenuChoiceDescriptor(),
            New MoveNorthChoiceDescriptor(),
            New MoveEastChoiceDescriptor(),
            New MoveSouthChoiceDescriptor(),
            New MoveWestChoiceDescriptor(),
            New DwarfChoiceDescriptor(),
            New ElfChoiceDescriptor(),
            New HalflingChoiceDescriptor(),
            New ManChoiceDescriptor()
        }.ToDictionary(Function(x) x.Choice, Function(x) x)
End Module
