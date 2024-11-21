Friend Module LocationTypes
    Friend ReadOnly Town As String = NameOf(Town)
    Friend ReadOnly TownGate As String = NameOf(TownGate)
    Friend ReadOnly Wilderness As String = NameOf(Wilderness)
    Friend ReadOnly Inn As String = NameOf(Inn)
    Friend ReadOnly InnCellar As String = NameOf(InnCellar)

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, LocationTypeDescriptor) =
        New List(Of LocationTypeDescriptor) From
        {
            New TownLocationTypeDescriptor(),
            New WildernessLocationTypeDescriptor(),
            New TownGateLocationTypeDescriptor(),
            New InnLocationTypeDescriptor(),
            New InnCellarLocationTypeDescriptor()
        }.ToDictionary(Function(x) x.LocationType, Function(x) x)
End Module
