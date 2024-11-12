Friend Module Direction
    Friend Const North = "north"
    Friend Const East = "east"
    Friend Const South = "south"
    Friend Const West = "west"
    Friend ReadOnly All As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            North,
            East,
            South,
            West
        }
    Friend Function GetLeft(facing As String) As String
        Select Case facing
            Case Direction.North
                Return Direction.West
            Case Direction.East
                Return Direction.North
            Case Direction.South
                Return Direction.East
            Case Direction.West
                Return Direction.South
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
    Friend Function GetRight(facing As String) As String
        Select Case facing
            Case Direction.North
                Return Direction.East
            Case Direction.East
                Return Direction.South
            Case Direction.South
                Return Direction.West
            Case Direction.West
                Return Direction.North
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
    Friend Function GetOpposite(facing As String) As String
        Select Case facing
            Case Direction.North
                Return Direction.South
            Case Direction.East
                Return Direction.West
            Case Direction.South
                Return Direction.North
            Case Direction.West
                Return Direction.East
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
    Friend Function GetDeltaX(facing As String) As Integer
        Select Case facing
            Case Direction.North
                Return 0
            Case Direction.East
                Return 1
            Case Direction.South
                Return 0
            Case Direction.West
                Return -1
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
    Friend Function GetDeltaY(facing As String) As Integer
        Select Case facing
            Case Direction.North
                Return -1
            Case Direction.East
                Return 0
            Case Direction.South
                Return 1
            Case Direction.West
                Return 0
            Case Else
                Throw New NotImplementedException
        End Select
    End Function
    Friend Function ToMazeDirection(facing As String) As MazeDirection(Of String)
        Return New MazeDirection(Of String)(
            GetOpposite(facing),
            GetDeltaX(facing),
            GetDeltaY(facing))
    End Function
End Module
