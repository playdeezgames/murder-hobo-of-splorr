Friend Module LocationExtensionMethods
    <Extension>
    Function Descriptor(location As ILocation) As BaseLocationTypeDescriptor
        Return LocationTypes.Descriptors(location.EntityType)
    End Function
    <Extension>
    Function AllowedRoutes(location As ILocation, character As ICharacter) As IEnumerable(Of IRoute)
        Return location.Routes.Where(Function(x) x.Allows(character))
    End Function
End Module
