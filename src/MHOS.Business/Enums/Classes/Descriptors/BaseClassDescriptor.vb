Friend MustInherit Class BaseClassDescriptor
    ReadOnly Property [Class] As String
    ReadOnly Property Name As String
    ReadOnly Property Choice As String
    ReadOnly Property ClassLevelDescriptors As IReadOnlyDictionary(Of Integer, ClassLevelDescriptor)
    ReadOnly Property HitDie As Integer
    Sub New(
           [class] As String,
           name As String,
           choice As String,
           hitDie As Integer,
           classLevelDescriptors As IReadOnlyDictionary(Of Integer, ClassLevelDescriptor))
        Me.Class = [class]
        Me.Name = name
        Me.Choice = choice
        Me.ClassLevelDescriptors = classLevelDescriptors
        Me.HitDie = hitDie
    End Sub
    MustOverride Function IsQualified(character As ICharacter) As Boolean
End Class
