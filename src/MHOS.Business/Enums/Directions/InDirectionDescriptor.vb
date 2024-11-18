Friend Class InDirectionDescriptor
    Inherits DirectionDescriptor

    Public Sub New()
        MyBase.New(
            Directions.In,
            "in",
            False,
            Choices.MoveIn,
            Nothing,
            Directions.Out,
            Nothing,
            0,
            0)
    End Sub
End Class
