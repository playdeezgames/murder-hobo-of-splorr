Friend Module CounterTypes

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseCounterTypeDescriptor) =
        GenerateDescriptors()

    Private Function GenerateDescriptors() As IReadOnlyDictionary(Of String, BaseCounterTypeDescriptor)
        Return (New List(Of BaseCounterTypeDescriptor)).ToDictionary(Function(x) x.AttributeType, Function(x) x)
    End Function
End Module
