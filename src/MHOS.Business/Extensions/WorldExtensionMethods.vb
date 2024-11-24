Imports System.Data

Friend Module WorldExtensionMethods
    Private Function InitializeCharacter(world As IWorld, characterType As String, location As ILocation) As ICharacter
        Dim character = world.CreateCharacter(characterType, location)
        character.Initialize()
        Return character
    End Function
    Private Sub InitializeTown(world As IWorld)
        world.AddInitializationStep(AddressOf InitializeInn)
        Const TownColumns = 5
        Const TownRows = 5
        Const TownCenterColumn = TownColumns \ 2
        Const TownCenterRow = TownRows \ 2
        Dim townLocations(TownColumns - 1, TownRows - 1) As ILocation
        world.InitializeLocationGrid(townLocations, LocationTypes.Town)
        townLocations.Mazeify(RouteTypes.Road)
        RouteifyLocationGrid((0, TownCenterRow), TownColumns - 1, Business.Directions.East, townLocations, RouteTypes.Road)
        RouteifyLocationGrid((TownColumns - 1, TownCenterRow), TownColumns - 1, Business.Directions.West, townLocations, RouteTypes.Road)
        RouteifyLocationGrid((TownCenterColumn, 0), TownRows - 1, Business.Directions.South, townLocations, RouteTypes.Road)
        RouteifyLocationGrid((TownCenterColumn, TownRows - 1), TownRows - 1, Business.Directions.North, townLocations, RouteTypes.Road)
        townLocations(0, TownCenterRow).TownGateDirection(Business.Directions.West) = True
        townLocations(TownColumns - 1, TownCenterRow).TownGateDirection(Business.Directions.East) = True
        townLocations(TownCenterColumn, 0).TownGateDirection(Business.Directions.North) = True
        townLocations(TownCenterColumn, TownRows - 1).TownGateDirection(Business.Directions.South) = True
    End Sub
    Private Sub InitializeInn(world As IWorld)
        world.AddInitializationStep(AddressOf InitializeCellar)
        Dim entrance = RNG.FromEnumerable(world.Locations.Where(Function(x) x.EntityType = LocationTypes.Town AndAlso Not x.HasRoute(Directions.In)))
        Dim location = world.CreateLocation(LocationTypes.Inn)
        entrance.CreateRoute(Directions.In, RouteTypes.Door, location)
        location.CreateRoute(Directions.Out, RouteTypes.Door, entrance)
        InitializeInnSign(entrance)
        InitializeGorachan(location)
    End Sub

    Private Sub InitializeGorachan(location As ILocation)
        Dim gorachan = location.CreateFeature(FeatureTypes.NPC)
        gorachan.Metadata(MetadataTypes.Name) = "Gorachan the Innkeeper"
        gorachan.Metadata(MetadataTypes.ShortName) = "the innkeeper"
        'TODO: cellar quest
    End Sub

    Private Sub InitializeInnSign(entrance As ILocation)
        Dim signFeature = entrance.CreateFeature(FeatureTypes.Sign)
        signFeature.Metadata(MetadataTypes.SignText) = "Jusdatip Inn, Gorachan: Proprietor"
    End Sub

    Private Sub InitializeCellar(world As IWorld)
        Dim entrance = world.Locations.Single(Function(x) x.EntityType = LocationTypes.Inn)
        Dim location = world.CreateLocation(LocationTypes.InnCellar)
        Dim downStairs = entrance.CreateRoute(Directions.Down, RouteTypes.Stairs, location)
        location.CreateRoute(Directions.Up, RouteTypes.Stairs, entrance)
    End Sub

    <Extension>
    Private Sub InitializeLocationGrid(world As IWorld, locationGrid As ILocation(,), locationType As String)
        For Each townColumn In Enumerable.Range(0, locationGrid.GetLength(0))
            For Each townRow In Enumerable.Range(0, locationGrid.GetLength(1))
                locationGrid(townColumn, townRow) = world.CreateLocation(locationType)
            Next
        Next
    End Sub
    <Extension>
    Friend Sub Mazeify(locationGrid As ILocation(,), routeType As String)
        Dim size As (Columns As Integer, Rows As Integer) = (locationGrid.GetLength(0), locationGrid.GetLength(1))
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
    Private Sub RouteifyLocationGrid(start As (Column As Integer, Row As Integer), steps As Integer, direction As String, locations As ILocation(,), routeType As String)
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
        Business.Directions.Descriptors.Where(Function(x) x.Value.HasMazeDirection).ToDictionary(
            Function(x) x.Key,
            Function(x) x.Value.ToMazeDirection())
    Friend Sub Initialize(world As IWorld)
        world.AddInitializationStep(AddressOf PopulateLocations)
        world.AddInitializationStep(AddressOf InitializeCharacter)
        world.AddInitializationStep(AddressOf InitializeLocations)
    End Sub
    Private Sub PopulateLocations(world As IWorld)
        'TODO: add enemies
    End Sub
    Private Sub InitializeCharacter(world As IWorld)
        world.AddInitializationStep(Sub(w)
                                        Dim location = RNG.FromEnumerable(w.Locations.Where(Function(x) x.EntityType = LocationTypes.Town AndAlso x.HasRoute(Directions.In)))
                                        Dim character = InitializeCharacter(w,
                                            CharacterTypes.Player,
                                            location)
                                        w.SetAvatar(character)
                                    End Sub)
    End Sub
    Private Function CreateMaze(columns As Integer, rows As Integer) As Maze(Of String)
        Dim maze As New Maze(Of String)(columns, rows, mazeDirections)
        maze.Generate()
        Return maze
    End Function
    Private Sub InitializeLocations(world As IWorld)
        world.AddInitializationStep(AddressOf InitializeWilderness)
        world.AddInitializationStep(AddressOf InitializeTown)
    End Sub
    Private Sub InitializeWilderness(world As IWorld)
        Const WildernessColumns = 15
        Const WildernessRows = 15
        Const WildernessCenterColumn = WildernessColumns \ 2
        Const WildernessCenterRow = WildernessRows \ 2
        Dim wildernessLocations(WildernessColumns - 1, WildernessRows - 1) As ILocation
        world.InitializeLocationGrid(wildernessLocations, LocationTypes.Wilderness)
        wildernessLocations.Mazeify(RouteTypes.Road)
        Dim centerWildernessLocation = wildernessLocations(WildernessCenterColumn, WildernessCenterRow)
        For Each route In centerWildernessLocation.Routes
            Dim townLocation = world.Locations.Single(Function(x) x.TownGateDirection(route.Id.Direction))
            townLocation.CreateRoute(route.Id.Direction, RouteTypes.Gate, route.Destination)
            route.Destination.EntityType = LocationTypes.TownGate
            Dim oppositeDirection = Directions.Descriptors(route.Id.Direction).OppositeDirection
            Dim oppositeRoute = route.Destination.GetRoute(oppositeDirection)
            oppositeRoute.EntityType = RouteTypes.Gate
            oppositeRoute.Destination = townLocation
        Next
        centerWildernessLocation.Recycle()
    End Sub
End Module
