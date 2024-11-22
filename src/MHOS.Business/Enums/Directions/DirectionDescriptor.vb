Friend MustInherit Class DirectionDescriptor
    ReadOnly Property Direction As String
    ReadOnly Property LeftDirection As String
    ReadOnly Property RightDirection As String
    ReadOnly Property OppositeDirection As String
    ReadOnly Property DeltaX As Integer
    ReadOnly Property DeltaY As Integer
    ReadOnly Property Name As String
    ReadOnly Property HasMazeDirection As Boolean
    Sub New(
           direction As String,
           name As String,
           hasMazeDirection As Boolean,
           rightDirection As String,
           oppositeDirection As String,
           leftDirection As String,
           deltaX As Integer,
           deltaY As Integer)
        Me.Direction = direction
        Me.RightDirection = rightDirection
        Me.OppositeDirection = oppositeDirection
        Me.LeftDirection = leftDirection
        Me.DeltaX = deltaX
        Me.DeltaY = deltaY
        Me.Name = name
        Me.HasMazeDirection = hasMazeDirection
    End Sub
    Friend Function ToMazeDirection() As MazeDirection(Of String)
        Return New MazeDirection(Of String)(
            OppositeDirection,
            DeltaX,
            DeltaY)
    End Function
End Class
