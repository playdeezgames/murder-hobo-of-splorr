Friend Class WorldSessionModel
    Implements IWorldSessionModel
    Private setWorld As Action(Of IWorld)
    Private getWorld As Func(Of IWorld)
    Sub New(setWorld As Action(Of IWorld), getWorld As Func(Of IWorld), options As IEmbarkOptions)
        Me.setWorld = setWorld
        Me.getWorld = getWorld
        Me.Options = options
    End Sub

    Public ReadOnly Property Options As IEmbarkOptions Implements IWorldSessionModel.Options

    Public Sub Embark() Implements IWorldSessionModel.Embark
        setWorld(New World(New WorldData))
        WorldInitializer.Initialize(getWorld(), Options)
    End Sub

    Public Sub Abandon() Implements IWorldSessionModel.Abandon
        setWorld(Nothing)
    End Sub

    Public Sub Load(filename As String) Implements IWorldSessionModel.Load
        setWorld(New World(JsonSerializer.Deserialize(Of WorldData)(File.ReadAllText(filename))))
    End Sub

    Public Sub Save(filename As String) Implements IWorldSessionModel.Save
        File.WriteAllText(filename, getWorld().Serialized)
    End Sub
End Class
