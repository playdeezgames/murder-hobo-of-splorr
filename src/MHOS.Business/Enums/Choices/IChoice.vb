Public Interface IChoice
    ReadOnly Property Choice As String
    ReadOnly Property Text As String
    Function LegacyChoose(dialog As String, world As IWorld) As String
    Function Choose() As String
End Interface
