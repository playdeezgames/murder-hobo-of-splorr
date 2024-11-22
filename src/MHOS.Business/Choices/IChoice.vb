Public Interface IChoice
    ReadOnly Property Text As String
    Function LegacyChoose() As String
    Function Choose() As IDialog
End Interface
