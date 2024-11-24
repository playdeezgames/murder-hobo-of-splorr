Friend Class SignFeatureTypeDescriptor
    Inherits BaseFeatureTypeDescriptor

    Public Sub New()
        MyBase.New(FeatureTypes.Sign)
    End Sub

    Friend Overrides Function DescriptionLines(feature As IFeature) As IEnumerable(Of String)
        Return {
            $"The sign says '{feature.SignText}'."
            }
    End Function

    Friend Overrides Function BriefDescription(feature As IFeature) As String
        Return "a sign"
    End Function
End Class
