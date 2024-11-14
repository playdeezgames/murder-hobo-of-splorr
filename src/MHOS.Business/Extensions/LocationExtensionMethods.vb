Friend Module LocationExtensionMethods
    <Extension>
    Function Descriptor(location As ILocation) As LocationTypeDescriptor
        Return LocationTypes.Descriptors(location.EntityType)
    End Function
End Module
