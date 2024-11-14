Friend Class WestDirectionDescriptor
    Inherits DirectionDescriptor
    Public Sub New()
        MyBase.New(Directions.West, "west", Directions.North, Directions.East, Directions.South, -1, 0)
    End Sub
End Class
