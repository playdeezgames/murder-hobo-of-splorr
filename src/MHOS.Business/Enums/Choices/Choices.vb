Friend Module Choices
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
    Friend ReadOnly Cleric As String = NameOf(Cleric)
    Friend ReadOnly Fighter As String = NameOf(Fighter)
    Friend ReadOnly MagicUser As String = NameOf(MagicUser)
    Friend ReadOnly Thief As String = NameOf(Thief)
    Friend ReadOnly FighterMagicUser As String = NameOf(FighterMagicUser)
    Friend ReadOnly MagicUserThief As String = NameOf(MagicUserThief)
    Friend ReadOnly MoveIn As String = NameOf(MoveIn)
    Friend ReadOnly MoveOut As String = NameOf(MoveOut)
    Friend ReadOnly MoveDown As String = NameOf(MoveDown)
    Friend ReadOnly MoveUp As String = NameOf(MoveUp)
    Friend ReadOnly Interact As String = NameOf(Interact)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseChoiceDescriptor) =
        New List(Of BaseChoiceDescriptor) From
        {
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
            New ManChoiceDescriptor(),
            New ClericChoiceDescriptor(),
            New FighterChoiceDescriptor(),
            New MagicUserChoiceDescriptor(),
            New ThiefChoiceDescriptor(),
            New FighterMagicUserChoiceDescriptor(),
            New MagicUserThiefChoiceDescriptor(),
            New MoveInChoiceDescriptor(),
            New MoveOutChoiceDescriptor(),
            New MoveDownChoiceDescriptor(),
            New MoveUpChoiceDescriptor(),
            New InteractChoiceDescriptor()
        }.ToDictionary(Function(x) x.Choice, Function(x) x)
End Module
