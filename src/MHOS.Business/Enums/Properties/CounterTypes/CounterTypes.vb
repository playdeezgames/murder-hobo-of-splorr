Friend Module CounterTypes
    Private ReadOnly HitDieRoll As String = NameOf(HitDieRoll)
    Private Const FirstLevel As Integer = 1
    Private Const LevelCount As Integer = 20
    Friend ReadOnly HitPoints As String = NameOf(HitPoints)
    Friend ReadOnly Property LevelHitDieRoll(level As Integer) As String
        Get
            Return $"{HitDieRoll}{level}"
        End Get
    End Property

    Friend ReadOnly Descriptors As IReadOnlyDictionary(Of String, BaseCounterTypeDescriptor) =
        GenerateDescriptors()

    Private Function GenerateDescriptors() As IReadOnlyDictionary(Of String, BaseCounterTypeDescriptor)
        Dim result = New List(Of BaseCounterTypeDescriptor) From
        {
            New HitPointsCounterTypeDescriptor()
        }
        For Each level In Enumerable.Range(FirstLevel, LevelCount)
            result.Add(New LevelHitDieRollCounterTypeDescriptor(level))
        Next
        Return result.ToDictionary(Function(x) x.AttributeType, Function(x) x)
    End Function
End Module
