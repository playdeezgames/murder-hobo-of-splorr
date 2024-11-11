Public Interface IWorldModel
    Sub Embark()
    Sub Abandon()
    Sub Load(filename As String)
    Sub Save(filename As String)
    Sub TurnLeft()
    Sub TurnRight()
    Sub MoveAhead()
    Sub BeginCombat(trauma As String)
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
    ReadOnly Property Trauma As String
    ReadOnly Property PreviousCombat As String
    ReadOnly Property HasGroundItems As Boolean
    ReadOnly Property GroundItems As IReadOnlyDictionary(Of String, Integer)
    ReadOnly Property ItemGlyphs As IEnumerable(Of (Position As (X As Integer, Y As Integer), Text As String, Hue As Integer))
    Function GetItemTypeName(itemType As String) As String
    ReadOnly Property Map As IEnumerable(Of (Column As Integer, Row As Integer, Text As String, TriggerLevel As Integer, HasItems As Boolean))
    ReadOnly Property SectionName As String
    Sub TurnAround()
End Interface
