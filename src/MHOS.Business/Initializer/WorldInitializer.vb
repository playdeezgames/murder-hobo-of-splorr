Friend Module WorldInitializer
    Const MazeColumns = 7
    Const MazeRows = 7
    Private ReadOnly directions As IReadOnlyDictionary(Of String, MazeDirection(Of String)) =
        Business.Directions.Descriptors.ToDictionary(
            Function(x) x.Key,
            Function(x) x.Value.ToMazeDirection())
    Friend Sub Initialize(world As IWorld)
        InitializeLocations(world)
        InitializeCharacter(world)
        PopulateLocations(world)
    End Sub

    Private Sub PopulateLocations(world As IWorld)
        'TODO: add enemies
    End Sub

    Private Sub InitializeCharacter(world As IWorld)
        Dim location = RNG.FromEnumerable(world.Locations.Where(Function(x) x.EntityType = LocationTypes.Room))
        location.EntityType = LocationTypes.Initial
        Dim character = world.InitializeCharacter(
                        CharacterTypes.Player,
                        location)
        world.SetAvatar(character)
    End Sub

    Private Sub InitializeLocations(world As IWorld)
        Dim maze As New Maze(Of String)(MazeColumns, MazeRows, directions)
        maze.Generate()
        Dim locations(MazeColumns, MazeRows) As ILocation
        For Each column In Enumerable.Range(0, MazeColumns)
            For Each row In Enumerable.Range(0, MazeRows)
                locations(column, row) = world.CreateLocation(LocationTypes.Room)
            Next
        Next
        For Each column In Enumerable.Range(0, MazeColumns)
            For Each row In Enumerable.Range(0, MazeRows)
                Dim location = locations(column, row)
                Dim mazeCell = maze.GetCell(column, row)
                Dim doorCount As Integer = 0
                For Each direction In mazeCell.Directions
                    If mazeCell.GetDoor(direction).Open Then
                        doorCount += 1
                        Dim nextColumn = CInt(column + directions(direction).DeltaX)
                        Dim nextRow = CInt(row + directions(direction).DeltaY)
                        Dim nextLocation = locations(nextColumn, nextRow)
                        location.CreateRoute(direction, RouteTypes.Door, nextLocation)
                    End If
                Next
                If doorCount = 1 Then
                    location.EntityType = LocationTypes.DeadEnd
                End If
            Next
        Next
    End Sub
End Module
