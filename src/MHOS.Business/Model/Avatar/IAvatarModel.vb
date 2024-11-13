Public Interface IAvatarModel
    ReadOnly Property Attributes As IEnumerable(Of IAttributeModel)
    ReadOnly Property Location As ILocationModel
    ReadOnly Property Name As String
    ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String))
    ReadOnly Property CanEnterGameMenu As Boolean
    ReadOnly Property AvailableChoices As (Text As String, Choice As String)()
    Sub MakeChoice(choice As String)
End Interface
