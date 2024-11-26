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

    Public ReadOnly Property MurderCounter As Integer Implements IWorld.MurderCounter
        Get
            Return WorldData.MurderCounter
        End Get
    End Property

    Public Sub Murder() Implements IWorld.Murder
        WorldData.MurderCounter += 1
    End Sub
End Class
