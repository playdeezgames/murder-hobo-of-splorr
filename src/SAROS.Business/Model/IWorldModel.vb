Public Interface IWorldModel
    Sub Embark()
    Sub Abandon()
    Sub Load(filename As String)
    Sub Save(filename As String)
    Sub TurnLeft()
    Sub TurnRight()
    Sub MoveAhead()
    Function IsBoardCellTrigger(column As Integer, row As Integer) As Boolean
    Sub PreviousBoardRow()
    Sub NextBoardRow()
    Sub EnemyMove()
    Function IsBoardCellVisible(column As Integer, row As Integer) As Boolean
    ReadOnly Property BoardRow As Integer
    ReadOnly Property BoardColumn As Integer
    ReadOnly Property Facing As String
    ReadOnly Property RoomString As String
    ReadOnly Property Column As Integer
    ReadOnly Property Row As Integer
    ReadOnly Property SectionName As String
    Sub TurnAround()
End Interface
