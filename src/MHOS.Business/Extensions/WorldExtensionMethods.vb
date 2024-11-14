Friend Module WorldExtensionMethods
    <Extension>
    Function InitializeCharacter(world As IWorld, characterType As String, location As ILocation) As ICharacter
        Dim character = world.CreateCharacter(characterType, location)
        character.Initialize()
        Return character
    End Function
End Module
