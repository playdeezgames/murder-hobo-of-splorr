Friend Module WorldInitializer
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
        Dim location = RNG.FromEnumerable(world.Locations.Where(Function(x) x.EntityType = LocationTypes.Town))
        Dim character = world.InitializeCharacter(
                        CharacterTypes.Player,
                        location)
        world.SetAvatar(character)
    End Sub
    Friend Function CreateMaze(columns As Integer, rows As Integer) As Maze(Of String)
        Dim maze As New Maze(Of String)(columns, rows, directions)
        maze.Generate()
        Return maze
    End Function
    Private Sub InitializeLocations(world As IWorld)
        world.InitializeTown()
    End Sub
End Module
