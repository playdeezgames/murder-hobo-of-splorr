Imports System.Data

Friend Module WorldExtensionMethods
    <Extension>
    Function InitializeCharacter(world As IWorld, characterType As String, location As ILocation) As ICharacter
        Dim character = world.CreateCharacter(characterType, location)
        character.Initialize()
        Return character
    End Function
    <Extension>
    Sub InitializeTown(world As IWorld)
        Const TownColumns = 5
        Const TownRows = 5
        Const TownCenterColumn = TownColumns \ 2
        Const TownCenterRow = TownRows \ 2
        Dim townLocations(TownColumns, TownRows) As ILocation
        world.InitializeLocationGrid((TownColumns, TownRows), townLocations, LocationTypes.Town)
        world.MazeifyLocationGrid((TownColumns, TownRows), townLocations, RouteTypes.Road)
        world.RouteifyLocationGrid((0, TownCenterRow), TownColumns - 1, Business.Directions.East, townLocations, RouteTypes.Road)
        world.RouteifyLocationGrid((TownColumns - 1, TownCenterRow), TownColumns - 1, Business.Directions.West, townLocations, RouteTypes.Road)
        world.RouteifyLocationGrid((TownCenterColumn, 0), TownRows - 1, Business.Directions.South, townLocations, RouteTypes.Road)
        world.RouteifyLocationGrid((TownCenterColumn, TownRows - 1), TownRows - 1, Business.Directions.North, townLocations, RouteTypes.Road)
        townLocations(0, TownCenterRow).Flag(FlagTypes.TownGateDirection(Business.Directions.West)) = True
        townLocations(TownColumns - 1, TownCenterRow).Flag(FlagTypes.TownGateDirection(Business.Directions.East)) = True
        townLocations(TownCenterColumn, 0).Flag(FlagTypes.TownGateDirection(Business.Directions.North)) = True
        townLocations(TownCenterColumn, TownRows - 1).Flag(FlagTypes.TownGateDirection(Business.Directions.South)) = True
    End Sub
    <Extension>
    Friend Sub InitializeLocationGrid(world As IWorld, size As (Columns As Integer, Rows As Integer), locationGrid As ILocation(,), locationType As String)
        For Each townColumn In Enumerable.Range(0, size.Columns)
            For Each townRow In Enumerable.Range(0, size.Rows)
                locationGrid(townColumn, townRow) = world.CreateLocation(locationType)
            Next
        Next
    End Sub
    <Extension>
    Friend Sub MazeifyLocationGrid(world As IWorld, size As (Columns As Integer, Rows As Integer), locationGrid As ILocation(,), routeType As String)
        Dim maze = CreateMaze(size.Columns, size.Rows)
        For Each column In Enumerable.Range(0, size.Columns)
            For Each row In Enumerable.Range(0, size.Rows)
                Dim location = locationGrid(column, row)
                Dim mazeCell = maze.GetCell(column, row)
                For Each direction In mazeCell.Directions
                    If mazeCell.GetDoor(direction).Open Then
                        Dim directionsDescriptor = Business.Directions.Descriptors(direction)
                        Dim nextColumn = column + directionsDescriptor.DeltaX
                        Dim nextRow = row + directionsDescriptor.DeltaY
                        Dim nextLocation = locationGrid(nextColumn, nextRow)
                        location.CreateRoute(direction, routeType, nextLocation)
                    End If
                Next
            Next
        Next
    End Sub
    <Extension>
    Friend Sub RouteifyLocationGrid(world As IWorld, start As (Column As Integer, Row As Integer), steps As Integer, direction As String, locations As ILocation(,), routeType As String)
        Dim directionDescriptor = Business.Directions.Descriptors(direction)
        Dim column = start.Column
        Dim row = start.Row
        For Each [step] In Enumerable.Range(0, steps)
            Dim location = locations(column, row)
            Dim nextColumn = column + directionDescriptor.DeltaX
            Dim nextRow = row + directionDescriptor.DeltaY
            Dim nextLocation = locations(nextColumn, nextRow)
            If Not location.HasRoute(directionDescriptor.Direction) Then
                location.CreateRoute(directionDescriptor.Direction, routeType, nextLocation)
            End If
            column = nextColumn
            row = nextRow
        Next
    End Sub
    Private ReadOnly mazeDirections As IReadOnlyDictionary(Of String, MazeDirection(Of String)) =
        Business.Directions.Descriptors.ToDictionary(
            Function(x) x.Key,
            Function(x) x.Value.ToMazeDirection())
    <Extension>
    Friend Sub Initialize(world As IWorld)
        world.InitializeLocations()
        world.InitializeCharacter()
        world.PopulateLocations()
    End Sub
    <Extension>
    Private Sub PopulateLocations(world As IWorld)
        'TODO: add enemies
    End Sub
    <Extension>
    Private Sub InitializeCharacter(world As IWorld)
        Dim location = RNG.FromEnumerable(world.Locations.Where(Function(x) x.EntityType = LocationTypes.Town))
        Dim character = world.InitializeCharacter(
                        CharacterTypes.Player,
                        location)
        world.SetAvatar(character)
    End Sub
    Private Function CreateMaze(columns As Integer, rows As Integer) As Maze(Of String)
        Dim maze As New Maze(Of String)(columns, rows, mazeDirections)
        maze.Generate()
        Return maze
    End Function
    <Extension>
    Private Sub InitializeLocations(world As IWorld)
        world.InitializeTown()
        world.InitializeWilderness()
    End Sub
    <Extension>
    Private Sub InitializeWilderness(world As IWorld)
        Const WildernessColumns = 15
        Const WildernessRows = 15
        Const WildernessCenterColumn = WildernessColumns \ 2
        Const WildernessCenterRow = WildernessRows \ 2
        Dim wildernessLocations(WildernessColumns, WildernessRows) As ILocation
        world.InitializeLocationGrid((WildernessColumns, WildernessRows), wildernessLocations, LocationTypes.Wilderness)
        world.MazeifyLocationGrid((WildernessColumns, WildernessRows), wildernessLocations, RouteTypes.Road)
        Dim centerWildernessLocation = wildernessLocations(WildernessCenterColumn, WildernessCenterRow)
        For Each route In centerWildernessLocation.Routes
            Dim townLocation = world.Locations.Single(Function(x) x.Flag(FlagTypes.TownGateDirection(route.Id.Direction)))
            townLocation.CreateRoute(route.Id.Direction, RouteTypes.Gate, route.Destination)
            route.Destination.EntityType = LocationTypes.TownGate
            Dim oppositeDirection = Directions.Descriptors(route.Id.Direction).OppositeDirection
            Dim oppositeRoute = route.Destination.GetRoute(oppositeDirection)
            oppositeRoute.EntityType = RouteTypes.Gate
            oppositeRoute.Destination = townLocation
            'TODO: mark transitions into wilderness with a flag, or different location type
        Next
        centerWildernessLocation.Recycle()
    End Sub
End Module
