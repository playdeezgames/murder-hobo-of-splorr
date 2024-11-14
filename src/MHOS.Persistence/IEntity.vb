Public Interface IEntity(Of TIdentifier)
    ReadOnly Property Id As TIdentifier
    Property EntityType As String
    Property Counter(counterType As String) As Integer?
    ReadOnly Property CounterTypes As IEnumerable(Of String)
End Interface
