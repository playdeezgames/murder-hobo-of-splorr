Friend Module WorldInitializer
    Const MazeColumns = 7
    Const MazeRows = 7
    Const MaximumSanity = 99
    Private ReadOnly directions As IReadOnlyDictionary(Of String, MazeDirection(Of String)) =
        Direction.All.ToDictionary(
            Function(x) x,
            Function(x) Direction.ToMazeDirection(x))
    Friend Sub Initialize(world As IWorld)
        InitializeLocations(world)
        InitializeCharacter(world)
        InitializeItems(world)
    End Sub

    Private Sub InitializeItems(world As IWorld)
        For Each itemType In ItemTypes.All
            Dim descriptor = ItemTypes.GetDescriptor(itemType)
            For Each dummy In Enumerable.Range(0, descriptor.SpawnCount)
                Dim location = RNG.FromEnumerable(world.Locations)
                Dim item = world.CreateItem(itemType)
                location.AddItem(item)
            Next
        Next
    End Sub

    Private Sub InitializeCharacter(world As IWorld)
        Dim character = world.CreateCharacter(
                        RNG.FromEnumerable(world.Locations),
                        RNG.FromEnumerable(Direction.All),
                        MaximumSanity)
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
