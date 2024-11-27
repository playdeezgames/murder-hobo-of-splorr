Public Interface IWorld
    ReadOnly Property Serialized As String
    ReadOnly Property MurderSkill As Integer
    ReadOnly Property MurderDifficulty As Integer
    ReadOnly Property MurderCounter As Integer
    ReadOnly Property AttemptCounter As Integer
    ReadOnly Property SuccessRate As Integer?
    Sub AttemptMurder()
    ReadOnly Property Messages As IEnumerable(Of (Text As String, Mood As String))
    ReadOnly Property ExperiencePoints As Integer

    ReadOnly Property CanBuySkillIncrease As Boolean
    ReadOnly Property SkillIncreaseCost As Integer
    Sub BuySkillIncrease()

    ReadOnly Property CanBuyDifficultyIncrease As Boolean
    ReadOnly Property DifficultyIncreaseCost As Integer
    Sub BuyDifficultyIncrease()

    ReadOnly Property SuccessStreak As Integer
    Sub AutoMurder()
End Interface
