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
    Sub CompleteCombat()
    Function IsBoardCellVisible(column As Integer, row As Integer) As Boolean
    ReadOnly Property BoardRow As Integer
    ReadOnly Property BoardColumn As Integer
    ReadOnly Property Facing As String
    ReadOnly Property RoomString As String
    ReadOnly Property Column As Integer
    ReadOnly Property Row As Integer
    ReadOnly Property Sanity As Integer
    ReadOnly Property MaximumSanity As Integer
    ReadOnly Property IsInsane As Boolean
    ReadOnly Property Trauma As String
    ReadOnly Property PreviousCombat As String
    ReadOnly Property TriggerLevel As Integer
    ReadOnly Property HasGroundItems As Boolean
    ReadOnly Property GroundItems As IReadOnlyDictionary(Of String, Integer)
    ReadOnly Property ItemGlyphs As IEnumerable(Of (Position As (X As Integer, Y As Integer), Text As String, Hue As Integer))
    Function GetItemTypeName(itemType As String) As String
    Sub PickUpItems(itemType As String)
    Sub UseItem(itemType As String)
    ReadOnly Property HasInventory As Boolean
    ReadOnly Property Inventory As IReadOnlyDictionary(Of String, Integer)
    ReadOnly Property Win As Boolean
    ReadOnly Property Map As IEnumerable(Of (Column As Integer, Row As Integer, Text As String, TriggerLevel As Integer, HasItems As Boolean))
    ReadOnly Property EnemyCombatDamage As Integer
    ReadOnly Property PlayerCombatDamage As Integer
    ReadOnly Property PostCombatSanity As Integer
    ReadOnly Property PostCombatTriggerLevel As Integer
    ReadOnly Property SectionName As String
    ReadOnly Property CanAvoid As Boolean
    Sub Avoid()
    Sub TurnAround()
    ReadOnly Property TraumaStates As IEnumerable(Of (Trauma As String, Awareness As Integer, TriggerLevel As Integer))
End Interface
