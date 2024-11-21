Friend Class MoveUpChoiceDescriptor
    Inherits BaseMoveChoiceDescriptor

    Public Sub New()
        MyBase.New(Choices.MoveUp, "Up", Directions.Up)
    End Sub
End Class
