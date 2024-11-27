Public Class WorldData
    Public Property MurderCounter As Integer = 0
    Public Property AttemptCounter As Integer = 0
    Public Property Messages As New List(Of MessageData)
    Public Property ExperiencePoints As Integer = 0
    Public Property MurderSkill As Integer = 1
    Public Property MurderDifficulty As Integer = 1
    Public Property SkillIncreaseCost As Integer = 50
    Public Property DifficultyIncreaseCost As Integer = 25
    Public Property SuccessStreak As Integer = 0
    Public Property RecordSuccessStreak As Integer = 0
End Class
