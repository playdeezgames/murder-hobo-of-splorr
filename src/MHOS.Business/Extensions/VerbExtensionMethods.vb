Friend Module VerbExtensionMethods
    <Extension>
    Function Allows(verb As IVerb, character As ICharacter) As Boolean
        Return verb.Conditions.All(Function(x) x.Allows(character))
    End Function
End Module
