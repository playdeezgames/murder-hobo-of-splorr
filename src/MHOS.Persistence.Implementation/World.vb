Imports System.Security.Cryptography
Imports System.Text.Json
Imports MHOS.Data
Imports SPLORR.Game

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

    Public ReadOnly Property AttemptCounter As Integer Implements IWorld.AttemptCounter
        Get
            Return WorldData.AttemptCounter
        End Get
    End Property

    Public ReadOnly Property SuccessRate As Integer? Implements IWorld.SuccessRate
        Get
            If AttemptCounter = 0 Then
                Return Nothing
            End If
            Return 100 * MurderCounter \ AttemptCounter
        End Get
    End Property

    Public Sub AttemptMurder() Implements IWorld.AttemptMurder
        WorldData.AttemptCounter += 1
        If RNG.FromRange(0, 1) = 1 Then
            WorldData.MurderCounter += 1
        End If
    End Sub
End Class
