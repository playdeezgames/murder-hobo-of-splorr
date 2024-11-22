Friend Class UpDirectionDescriptor
    Inherits DirectionDescriptor

    Public Sub New()
        MyBase.New(
            Directions.Up,
            "up",
            False,
            Nothing,
            Directions.Up,
            Nothing,
            0,
            0)
    End Sub
End Class
