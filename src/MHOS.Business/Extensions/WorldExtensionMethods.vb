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
        world.RouteifyLocationGrid((0, TownCenterRow), TownColumns - 1, Directions.East, townLocations, RouteTypes.Road)
        world.RouteifyLocationGrid((TownColumns - 1, TownCenterRow), TownColumns - 1, Directions.West, townLocations, RouteTypes.Road)
        world.RouteifyLocationGrid((TownCenterColumn, 0), TownRows - 1, Directions.South, townLocations, RouteTypes.Road)
        world.RouteifyLocationGrid((TownCenterColumn, TownRows - 1), TownRows - 1, Directions.North, townLocations, RouteTypes.Road)
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
        Dim maze = WorldInitializer.CreateMaze(size.Columns, size.Rows)
        For Each column In Enumerable.Range(0, size.Columns)
            For Each row In Enumerable.Range(0, size.Rows)
                Dim location = locationGrid(column, row)
                Dim mazeCell = maze.GetCell(column, row)
                For Each direction In mazeCell.Directions
                    Dim directionsDescriptor = Directions.Descriptors(direction)
                    Dim nextColumn = column + directionsDescriptor.DeltaX
                    Dim nextRow = row + directionsDescriptor.DeltaY
                    Dim nextLocation = locationGrid(nextColumn, nextRow)
                    location.CreateRoute(direction, routeType, nextLocation)
                Next
            Next
        Next
    End Sub
    <Extension>
    Friend Sub RouteifyLocationGrid(world As IWorld, start As (Column As Integer, Row As Integer), steps As Integer, direction As String, locations As ILocation(,), routeType As String)
        Dim directionDescriptor = Directions.Descriptors(direction)
        Dim column = start.Column
        Dim row = start.Row
        For Each [step] In Enumerable.Range(0, steps)
            Dim location = locations(column, row)
            Dim nextColumn = column + directionDescriptor.DeltaX
            Dim nextRow = row + directionDescriptor.DeltaY
            Dim nextLocation = locations(nextColumn, nextRow)
            location.CreateRoute(directionDescriptor.Direction, routeType, nextLocation)
            column = nextColumn
            row = nextRow
        Next
    End Sub
End Module
