Friend Class OutDirectionDescriptor
    Inherits DirectionDescriptor

    Public Sub New()
        MyBase.New(
            Directions.Out,
            "out",
            False,
            Choices.MoveOut,
            Nothing,
            Directions.In,
            Nothing,
            0,
            0)
    End Sub
End Class
