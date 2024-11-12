Friend Module WorldInitializer
    Const MazeColumns = 7
    Const MazeRows = 7
    Private ReadOnly directions As IReadOnlyDictionary(Of String, MazeDirection(Of String)) =
        Direction.All.ToDictionary(
            Function(x) x,
            Function(x) Direction.ToMazeDirection(x))
    Friend Sub Initialize(world As IWorld, options As IEmbarkOptions)
        InitializeLocations(world)
        InitializeCharacter(world, options)
    End Sub

    Private Sub InitializeCharacter(world As IWorld, options As IEmbarkOptions)
        Dim character = world.CreateCharacter(
                        RNG.FromEnumerable(world.Locations),
                        RNG.FromEnumerable(Direction.All))
        For Each attributeType In options.Attributes
            character.SetAttribute(attributeType.AttributeType, attributeType.Value)
        Next
        world.SetAvatar(
            character)
    End Sub

    Private Sub InitializeLocations(world As IWorld)
        Dim maze As New Maze(Of String)(MazeColumns, MazeRows, directions)
        maze.Generate()
        Dim locations(MazeColumns, MazeRows) As ILocation
        For Each column In Enumerable.Range(0, MazeColumns)
            For Each row In Enumerable.Range(0, MazeRows)
                locations(column, row) = world.CreateLocation(column, row)
            Next
        Next
        For Each column In Enumerable.Range(0, MazeColumns)
            For Each row In Enumerable.Range(0, MazeRows)
                Dim location = locations(column, row)
                Dim mazeCell = maze.GetCell(column, row)
                For Each direction In mazeCell.Directions
                    If mazeCell.GetDoor(direction).Open Then
                        Dim nextColumn = CInt(column + directions(direction).DeltaX)
                        Dim nextRow = CInt(row + directions(direction).DeltaY)
                        Dim nextLocation = locations(nextColumn, nextRow)
                        location.SetNeighbor(direction, nextLocation)
                        Location.SetDoor(direction, Door.Open)
                    End If
                Next
            Next
        Next
    End Sub
End Module
