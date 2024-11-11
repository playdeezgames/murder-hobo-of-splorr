Friend Module ItemTypes
    Friend Const SanityPotion = "SanityPotion"
    Friend Const IdeaBomb = "IdeaBomb"
    Friend Const Avoidance = "Avoidance"
    Private ReadOnly descriptors As IReadOnlyDictionary(Of String, ItemTypeDescriptor) =
        New Dictionary(Of String, ItemTypeDescriptor) From
        {
            {SanityPotion, New ItemTypeDescriptor("Red Pill", AddressOf OnUseRedPill, (96, 160), ChrW(0), 4, spawnCount:=10)},
            {IdeaBomb, New ItemTypeDescriptor("Mindfulness", AddressOf OnUseMindfulness, (288, 160), ChrW(1), 14, spawnCount:=25)},
            {Avoidance, New ItemTypeDescriptor("Avoidance", AddressOf OnUseAvoidance, (240, 160), ChrW(4), 13, spawnCount:=15, isConsumed:=False)}
        }

    Private Sub OnUseAvoidance(character As ICharacter)
        'do nothing!
    End Sub

    Private Sub OnUseMindfulness(character As ICharacter)
        Dim trauma = character.Location.Trauma
        If Not String.IsNullOrEmpty(trauma) Then
            character.SetAwarenessLevel(trauma, character.GetAwarenessLevel(trauma) + 5)
        End If
    End Sub

    Private Sub OnUseRedPill(character As ICharacter)
        character.Sanity += 25
        'TODO: side effect
    End Sub

    Friend ReadOnly Property All As IEnumerable(Of String)
        Get
            Return descriptors.Keys
        End Get
    End Property
    Friend Function GetDescriptor(itemType As String) As ItemTypeDescriptor
        Return descriptors(itemType)
    End Function
End Module
