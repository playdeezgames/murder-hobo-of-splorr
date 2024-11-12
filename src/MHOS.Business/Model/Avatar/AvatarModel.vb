Friend Class AvatarModel
    Implements IAvatarModel

    Private ReadOnly world As IWorld

    Public Sub New(world As IWorld)
        Me.world = world
    End Sub

    Public ReadOnly Property Attributes As IEnumerable(Of IAttributeModel) Implements IAvatarModel.Attributes
        Get
            Return world.Avatar.Attributes.Select(Function(x) New AttributeModel(x, world.Avatar.GetAttribute(x)))
        End Get
    End Property

    Public ReadOnly Property RoomString As String Implements IAvatarModel.RoomString
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Dim frame As Integer = 0
            If location.HasDoor(character.LeftDirection) Then
                frame += 1
            End If
            If location.HasDoor(character.AheadDirection) Then
                frame += 2
            End If
            If location.HasDoor(character.RightDirection) Then
                frame += 4
            End If
            Return ChrW(frame)
        End Get
    End Property

    Public ReadOnly Property HasDoorAhead As Boolean Implements IAvatarModel.HasDoorAhead
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasDoor(character.AheadDirection)
        End Get
    End Property

    Public ReadOnly Property HasDoorToLeft As Boolean Implements IAvatarModel.HasDoorToLeft
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasDoor(character.LeftDirection)
        End Get
    End Property

    Public ReadOnly Property HasDoorToRight As Boolean Implements IAvatarModel.HasDoorToRight
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasDoor(character.RightDirection)
        End Get
    End Property

    Public ReadOnly Property HasDoorBehind As Boolean Implements IAvatarModel.HasDoorBehind
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasDoor(character.OppositeDirection)
        End Get
    End Property

    Public Sub TurnLeft() Implements IAvatarModel.TurnLeft
        world.Avatar.Facing = world.Avatar.LeftDirection
    End Sub

    Public Sub TurnRight() Implements IAvatarModel.TurnRight
        world.Avatar.Facing = world.Avatar.RightDirection
    End Sub

    Public Sub TurnAround() Implements IAvatarModel.TurnAround
        TurnRight()
        TurnRight()
    End Sub

    Public Sub MoveAhead() Implements IAvatarModel.MoveAhead
        Dim character = world.Avatar
        Dim location = character.Location
        Dim facing = character.AheadDirection
        If location.HasDoor(facing) Then
            character.Location = location.GetNeighbor(facing)
        End If
    End Sub
End Class
