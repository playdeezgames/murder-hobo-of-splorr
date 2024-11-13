Imports System.Reflection

Friend Class AvatarModel
    Implements IAvatarModel

    Private ReadOnly world As IWorld

    Public Sub New(world As IWorld)
        Me.world = world
    End Sub

    Public ReadOnly Property Attributes As IEnumerable(Of IAttributeModel) Implements IAvatarModel.Attributes
        Get
            Return world.Avatar.Attributes.Select(Function(x) New AttributeModel(x, world.Avatar.Attribute(x)))
        End Get
    End Property

    Public ReadOnly Property HasDoorAhead As Boolean Implements IAvatarModel.HasDoorAhead
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasRoute(character.AheadDirection)
        End Get
    End Property

    Public ReadOnly Property HasDoorToLeft As Boolean Implements IAvatarModel.HasDoorToLeft
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasRoute(character.LeftDirection)
        End Get
    End Property

    Public ReadOnly Property HasDoorToRight As Boolean Implements IAvatarModel.HasDoorToRight
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasRoute(character.RightDirection)
        End Get
    End Property

    Public ReadOnly Property HasDoorBehind As Boolean Implements IAvatarModel.HasDoorBehind
        Get
            Dim character = world.Avatar
            Dim location = character.Location
            Return location.HasRoute(character.OppositeDirection)
        End Get
    End Property

    Public ReadOnly Property Name As String Implements IAvatarModel.Name
        Get
            Return CharacterTypes.Descriptors(world.Avatar.CharacterType).Name
        End Get
    End Property

    Public ReadOnly Property Location As ILocationModel Implements IAvatarModel.Location
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
        If location.HasRoute(facing) Then
            character.Location = location.GetRoute(facing).Destination
        End If
    End Sub
End Class
