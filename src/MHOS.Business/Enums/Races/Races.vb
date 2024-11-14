Friend Module Races
    Friend ReadOnly Dwarf As String = NameOf(Dwarf)
    Friend ReadOnly Elf As String = NameOf(Elf)
    Friend ReadOnly Halfling As String = NameOf(Halfling)
    Friend ReadOnly Human As String = NameOf(Human)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseRaceDescriptor) =
        New List(Of BaseRaceDescriptor) From
        {
            New DwarfRaceDescriptor(),
            New ElfRaceDescriptor(),
            New HalflingRaceDescriptor(),
            New HumanRaceDescriptor()
        }.ToDictionary(Function(x) x.Race, Function(x) x)
End Module
