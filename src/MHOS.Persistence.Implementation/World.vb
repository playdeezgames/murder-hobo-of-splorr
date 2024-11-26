Imports System.Text.Json
Imports MHOS.Data

Public Class World
    Implements IWorld
    Protected ReadOnly WorldData As WorldData
    Sub New(worldData As WorldData)
        Me.WorldData = worldData
    End Sub

    Public ReadOnly Property Serialized As String Implements IWorld.Serialized
        Get
            Return JsonSerializer.Serialize(WorldData)
        End Get
    End Property

    Public ReadOnly Property MoveCounter As Integer Implements IWorld.MoveCounter
        Get
            Return WorldData.MoveCounter
        End Get
    End Property
End Class
