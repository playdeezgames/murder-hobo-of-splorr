Friend Module LocationExtensionMethods
    <Extension>
    Function Descriptor(location As ILocation) As LocationTypeDescriptor
        Return LocationTypes.Descriptors(location.LocationType)
    End Function
End Module
