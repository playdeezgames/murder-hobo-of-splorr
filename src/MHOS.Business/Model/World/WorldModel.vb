Public Class WorldModel
    Implements IWorldModel

    Private _world As IWorld
    Private ReadOnly _options As IEmbarkOptions = New EmbarkOptions()
    Sub New()
    End Sub

    Private Property World As IWorld
        Get
            Return _world
        End Get
        Set(value As IWorld)
            _world = value
        End Set
    End Property
    Public Sub Load(filename As String) Implements IWorldModel.Load
        World = New World(JsonSerializer.Deserialize(Of WorldData)(File.ReadAllText(filename)))
    End Sub
    Public Sub Save(filename As String) Implements IWorldModel.Save
        File.WriteAllText(filename, World.Serialized)
    End Sub

    Public ReadOnly Property Avatar As IAvatarModel Implements IWorldModel.Avatar
        Get
            Return New AvatarModel(_world)
        End Get
    End Property

    Public ReadOnly Property Session As IWorldSessionModel Implements IWorldModel.Session
        Get
            Return New WorldSessionModel(Sub(w) World = w, Function() World, _options)
        End Get
    End Property
End Class
