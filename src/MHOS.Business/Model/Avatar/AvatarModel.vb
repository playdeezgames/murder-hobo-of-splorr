Friend Class AvatarModel
    Implements IAvatarModel

    Private ReadOnly world As IWorld

    Public Sub New(world As IWorld)
        Me.world = world
    End Sub

    Private ReadOnly Property HasDoorAhead As Boolean
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasRoute(character.AheadDirection)
        End Get
    End Property

    Private ReadOnly Property HasDoorToLeft As Boolean
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasRoute(character.LeftDirection)
        End Get
    End Property

    Private ReadOnly Property HasDoorToRight As Boolean
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasRoute(character.RightDirection)
        End Get
    End Property

    Private ReadOnly Property HasDoorBehind As Boolean
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasRoute(character.OppositeDirection)
        End Get
    End Property

    Private ReadOnly Property Location As ILocationModel
        Get
            Return New LocationModel(world.Avatar.Location)
        End Get
    End Property

    Public ReadOnly Property Description As IEnumerable(Of (Text As String, Mood As String)) Implements IAvatarModel.Description
        Get
            Dim result As New List(Of (Text As String, Mood As String))
            result.Add(($"In {Location.Name}.", Moods.Normal))
            If HasDoorAhead Then
                result.Add(("Door ahead.", Moods.Normal))
            End If
            If HasDoorToLeft Then
                result.Add(("Door to yer left.", Moods.Normal))
            End If
            If HasDoorToRight Then
                result.Add(("Door to yer right.", Moods.Normal))
            End If
            If HasDoorBehind Then
                result.Add(("Door behind you.", Moods.Normal))
            End If
            Return result
        End Get
    End Property

    Public ReadOnly Property CanEnterGameMenu As Boolean Implements IAvatarModel.CanEnterGameMenu
        Get
            Return True
        End Get
    End Property

    Public ReadOnly Property AvailableChoices As (Text As String, Choice As String)() Implements IAvatarModel.AvailableChoices
        Get
            Return {
                    ("Move Ahead", Choices.MoveAhead),
                    ("Turn Right", Choices.TurnRight),
                    ("Turn Left", Choices.TurnLeft),
                    ("Turn Around", Choices.TurnAround)
                }
        End Get
    End Property

    Private Sub TurnLeft()
        world.Avatar.Facing = world.Avatar.LeftDirection
    End Sub

    Private Sub TurnRight()
        world.Avatar.Facing = world.Avatar.RightDirection
    End Sub

    Private Sub TurnAround()
        TurnRight()
        TurnRight()
    End Sub

    Private Sub MoveAhead()
        Dim character = world.Avatar
        Dim location = character.Location
        Dim facing = character.AheadDirection
        If location.HasRoute(facing) Then
            character.Location = location.GetRoute(facing).Destination
        End If
    End Sub

    Public Sub MakeChoice(choice As String) Implements IAvatarModel.MakeChoice
        Select Case choice
            Case Choices.TurnAround
                TurnAround()
            Case Choices.TurnRight
                TurnRight()
            Case Choices.TurnLeft
                TurnLeft()
            Case Choices.MoveAhead
                MoveAhead()
        End Select
    End Sub
End Class
