Friend Module WorldExtensionMethods
    <Extension>
    Function InitializeCharacter(world As IWorld, characterType As String, location As ILocation, facing As String) As ICharacter
        Dim character = world.CreateCharacter(characterType, location, facing)
        character.Initialize()
        Return character
    End Function
End Module
