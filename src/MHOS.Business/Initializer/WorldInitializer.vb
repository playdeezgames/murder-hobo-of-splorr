Friend Module WorldInitializer
    Const MazeColumns = 7
    Const MazeRows = 7
    Private ReadOnly directions As IReadOnlyDictionary(Of String, MazeDirection(Of String)) =
        Direction.All.ToDictionary(
            Function(x) x,
            Function(x) Direction.ToMazeDirection(x))
    Friend Sub Initialize(world As IWorld)
        InitializeLocations(world)
        InitializeCharacter(world)
    End Sub

    Private Sub InitializeCharacter(world As IWorld)
        Dim character = world.CreateCharacter(
                        RNG.FromEnumerable(world.Locations),
                        RNG.FromEnumerable(Direction.All))
        world.SetAvatar(
            character)
    End Sub

    Private Sub InitializeLocations(world As IWorld)
        Dim maze As New Maze(Of String)(MazeColumns, MazeRows, directions)
        maze.Generate()
        For Each column In Enumerable.Range(0, MazeColumns)
            For Each row In Enumerable.Range(0, MazeRows)
                world.CreateLocation(column, row)
            Next
        Next
        For Each location In world.Locations
            Dim mazeCell = maze.GetCell(location.Column, location.Row)
            For Each direction In mazeCell.Directions
                If mazeCell.GetDoor(direction).Open Then
                    Dim nextColumn = location.Column + directions(direction).DeltaX
                    Dim nextRow = location.Row + directions(direction).DeltaY
                    Dim nextLocation = world.Locations.Single(Function(x) x.Column = nextColumn AndAlso x.Row = nextRow)
                    location.SetNeighbor(direction, nextLocation)
                    location.SetDoor(direction, Door.Open)
                End If
            Next
        Next
    End Sub
End Module
